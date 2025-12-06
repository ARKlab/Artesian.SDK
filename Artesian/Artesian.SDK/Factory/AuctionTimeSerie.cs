using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using NodaTime;

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
        private readonly IMarketDataService _marketDataService = null!;

        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier = null!;

        private readonly Dictionary<LocalDateTime, AuctionBids> _bids = new Dictionary<LocalDateTime, AuctionBids>();

        /// <summary>
        /// AuctionTimeSerie Constructor
        /// </summary>
        internal AuctionTimeSerie(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            Guard.IsNotNull(marketData._entity);
            Guard.IsNotNull(marketData._marketDataService);

            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

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
        public AddAuctionTimeSerieOperationResult AddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer)");

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
        public AddAuctionTimeSerieOperationResult AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new AuctionTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, AuctionBidValue[] bid, AuctionBidValue[] offer)");

                var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, bid, offer);
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

            _bids.Add(bidTime, new AuctionBids(bidTime, bid, offer));
            return AddAuctionTimeSerieOperationResult.ValueAdded;
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
                var data = new UpsertCurveData(_identifier)
                {
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
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, string timezone = null!, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
        {
            var data = new DeleteCurveData(_identifier)
            {
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