using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using NodaTime;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// AuctionTimeSerie entity
    /// </summary>
    internal sealed class AuctionTimeSerie : IAuctionMarketDataWritable
    {
        private readonly IMarketDataService _marketDataService;
        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier;
        private readonly Dictionary<LocalDateTime, AuctionBids> _bids = new Dictionary<LocalDateTime, AuctionBids>();

        /// <summary>
        /// AuctionTimeSerie Constructor
        /// </summary>
        internal AuctionTimeSerie(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            var entity = Guard.IsNotNull(marketData._entity);
            var marketDataService = Guard.IsNotNull(marketData._marketDataService);

            _entity = entity;
            _marketDataService = marketDataService;

            _identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);

            Bids = new ReadOnlyDictionary<LocalDateTime, AuctionBids>(_bids);
        }

        /// <summary>
        /// AuctionData Bid
        /// </summary>
        public IReadOnlyDictionary<LocalDateTime, AuctionBids> Bids { get; private set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _bids.Clear();
        }

        /// <summary>
        /// AuctionTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddAuctionTimeSerieOperationResult</returns>
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
        public AddAuctionTimeSerieOperationResult AddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new AuctionTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer)");

            var localTime = localDate.AtMidnight();

            return _add(localTime, bid, offer);
        }

        /// <summary>
        /// AuctionTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddAuctionTimeSerieOperationResult</returns>
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
        public AddAuctionTimeSerieOperationResult AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new AuctionTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, AuctionBidValue[] bid, AuctionBidValue[] offer)");

                var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, bid, offer);
        }

        /// <summary>
        /// Attempts to add a data point to the AuctionTimeSerie for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="LocalDate"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="localDate">The date of the value to add.</param>
        /// <param name="bid">The bid to add.</param>
        /// <param name="offer">The offer to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddAuctionTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddAuctionTimeSerieOperationResult TryAddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new AuctionTimeSerieException("This MarketData has Time granularity. Use TryAddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer, KeyConflictPolicy keyConflictPolicy)");

            var localTime = localDate.AtMidnight();

            return _tryAdd(localTime, bid, offer, keyConflictPolicy);
        }

        /// <summary>
        /// Attempts to add a data point to the AuctionTimeSerie for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="Instant"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="time">The date of the value to add.</param>
        /// <param name="bid">The bid to add.</param>
        /// <param name="offer">The offer to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddAuctionTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddAuctionTimeSerieOperationResult TryAddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new AuctionTimeSerieException("This MarketData has Date granularity. Use TryAddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer, KeyConflictPolicy keyConflictPolicy)");

            var localTime = time.InUtc().LocalDateTime;

            return _tryAdd(localTime, bid, offer, keyConflictPolicy);
        }

        private AddAuctionTimeSerieOperationResult _add(LocalDateTime bidTime, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            if (_bids.ContainsKey(bidTime))
                return AddAuctionTimeSerieOperationResult.TimeAlreadyPresent;

            foreach (var element in _bids)
            {
                foreach (var item in element.Value.Bid)
                    if (item.Quantity < 0)
                        throw new AuctionTimeSerieException($"Auction[{element.Key}] contains invalid Bid Quantity < 0");

                foreach (var item in element.Value.Offer)
                    if (item.Quantity < 0)
                        throw new AuctionTimeSerieException($"Auction[{element.Key}] contains invalid Offer Quantity < 0");
            }

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!bidTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", bidTime, _identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!bidTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", bidTime, _identifier, period);
            }

            _bids.Add(bidTime, new AuctionBids { BidTimestamp = bidTime, Bid = bid, Offer = offer });
            return AddAuctionTimeSerieOperationResult.ValueAdded;
        }

        private AddAuctionTimeSerieOperationResult _tryAdd(LocalDateTime bidTime, AuctionBidValue[] bid, AuctionBidValue[] offer, KeyConflictPolicy keyConflictPolicy)
        {
            var valueToAdd = new AuctionBids { BidTimestamp = bidTime, Bid = bid, Offer = offer };
            var exists = _bids.ContainsKey(bidTime);

            switch (keyConflictPolicy)
            {
                case KeyConflictPolicy.Overwrite:
                    if (exists)
                    {
                        _bids[bidTime] = valueToAdd;
                        return AddAuctionTimeSerieOperationResult.ValueAdded;
                    }
                    return _add(bidTime, bid, offer);
                case KeyConflictPolicy.Throw:
                    if (exists)
                        throw new ArtesianSdkClientException("Data already present, cannot be updated!");
                    return _add(bidTime, bid, offer);
                case KeyConflictPolicy.Skip:
                    if (exists)
                        return AddAuctionTimeSerieOperationResult.TimeAlreadyPresent;
                    return _add(bidTime, bid, offer);
                default:
                    throw new NotSupportedException("KeyConflictPolicy not supported " + keyConflictPolicy);
            }
        }

        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">Downloaded at</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="keepNulls">if <see langword="false"/> nulls are ignored (server-side). That is the default behaviour.</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution, bool deferDataGeneration, bool keepNulls, CancellationToken ctk = default) =>
            await _save(downloadedAt, deferCommandExecution, deferDataGeneration, keepNulls, null, ctk).ConfigureAwait(false);

        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">Downloaded at</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="keepNulls">if <see langword="false"/> nulls are ignored (server-side). That is the default behaviour.</param>
        /// <param name="upsertMode">Upsert Mode</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, UpsertMode? upsertMode = null, CancellationToken ctk = default) =>
            await _save(downloadedAt, deferCommandExecution, deferDataGeneration, keepNulls, upsertMode, ctk).ConfigureAwait(false);

        private async Task _save(
            Instant downloadedAt, 
            bool deferCommandExecution,
            bool deferDataGeneration,
            bool keepNulls,
            UpsertMode? upsertMode,
            CancellationToken ctk)
        {
            if (Bids.Any())
            {
                var data = new UpsertCurveData
                {
                    ID = _identifier,
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    AuctionRows = _bids,
                    DeferCommandExecution = deferCommandExecution,
                    DeferDataGeneration = deferDataGeneration,
                    KeepNulls = keepNulls,
                    UpsertMode = upsertMode
                };

                await _marketDataService.UpsertCurveDataAsync(data, ctk).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="timezone">For DateSeries if provided must be equal to MarketData OrignalTimezone Default:MarketData OrignalTimezone. For TimeSeries Default:CET</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, string? timezone = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
        {
            var data = new DeleteCurveData
            {
                ID = _identifier,
                Timezone = timezone,
                // LocalDate.MinIsoValue has year -9998 and yearOfEra 9999. Using it without any string formatting, we got date 01-01-9999.
                // So we use default(LocalDateTime) 01/01/0001
                RangeStart = rangeStart ?? default(LocalDateTime),
                RangeEnd = rangeEnd ?? LocalDateTime.MaxIsoValue.Date.AtMidnight(),
                DeferCommandExecution = deferCommandExecution,
                DeferDataGeneration = deferDataGeneration,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk).ConfigureAwait(false);
        }
    }
}