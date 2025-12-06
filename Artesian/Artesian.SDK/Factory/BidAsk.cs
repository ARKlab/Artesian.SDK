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
    /// BidAsk entity
    /// </summary>
    internal sealed class BidAsk : IBidAskWritable
    {
        private readonly IMarketDataService _marketDataService = null!;

        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier = null!;

        private readonly List<BidAskElement> _values = new();

        /// <summary>
        /// BidAsks Constructor
        /// </summary>
        internal BidAsk(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            Guard.IsNotNull(marketData._entity);
            Guard.IsNotNull(marketData._marketDataService);

            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

            BidAsks = new ReadOnlyCollection<BidAskElement>(_values);
        }

        /// <summary>
        /// BidAsk BidAskElement
        /// </summary>
        public IReadOnlyCollection<BidAskElement> BidAsks { get; internal set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _values.Clear();
        }

        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddBidAskOperationResult</returns>
        public AddBidAskOperationResult AddData(LocalDate localDate, string product, BidAskValue value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Time granularity. Use AddData(Instant time...)");

            return _addBidAsk(localDate.AtMidnight(), product, value);
        }
        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddBidAskOperationResult</returns>
        public AddBidAskOperationResult AddData(Instant time, string product, BidAskValue value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            return _addBidAsk(time.InUtc().LocalDateTime, product, value);
        }

        private AddBidAskOperationResult _addBidAsk(LocalDateTime reportTime, string product, BidAskValue value)
        {
            //Relative products
            if (ArtesianConstants._relativeProductValidator.IsMatch(product))
                throw new NotSupportedException("Relative Products are not supported");

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new BidAskException("Trying to insert Report Time {0} with the wrong format to BidAsk {1}. Should be of period {2}", reportTime, _identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new BidAskException("Trying to insert Report Time {0} with wrong the format to BidAsk {1}. Should be of period {2}", reportTime, _identifier, period);
            }


            if (BidAsks.Any(row => row.ReportTime == reportTime && row.Product.Equals(product, StringComparison.Ordinal)))
                return AddBidAskOperationResult.ProductAlreadyPresent;

            _values.Add(new BidAskElement(reportTime, product, value));
            return AddBidAskOperationResult.BidAskAdded;
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

        private async Task _save(Instant downloadedAt,
            bool deferCommandExecution,
            bool deferDataGeneration,
            bool keepNulls,
            UpsertMode? upsertMode,
            CancellationToken ctk)
        {
            if (_values.Count != 0)
            {
                var data = new UpsertCurveData(_identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    DeferCommandExecution = deferCommandExecution,
                    BidAsk = new Dictionary<LocalDateTime, IDictionary<string, BidAskValue>>(),
                    KeepNulls = keepNulls,
                    UpsertMode = upsertMode
                };

                foreach (var reportTime in _values.GroupBy(g => g.ReportTime))
                {
                    var bidAsks = reportTime.ToDictionary(key => key.Product, value => value.Value, StringComparer.Ordinal);
                    data.BidAsk.Add(reportTime.Key, bidAsks);
                }

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
        /// <param name="product">Product of the BidAsk Time Serie</param>
        /// <param name="timezone">For DateSeries if provided must be equal to MarketData OrignalTimezone Default:MarketData OrignalTimezone. For TimeSeries Default:CET</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, List<string> product = null!, string timezone = null!, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
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
                Product = product,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk).ConfigureAwait(false);
        }

        /// <summary>
        /// BidAskElement entity
        /// </summary>
        public sealed class BidAskElement
        {
            /// <summary>
            /// BidAskElement constructor
            /// </summary>
            public BidAskElement(LocalDateTime reportTime, string product, BidAskValue value)
            {
                ReportTime = reportTime;
                Product = product;
                Value = value;
            }

            /// <summary>
            /// BidAskElement ReportTime
            /// </summary>
            public LocalDateTime ReportTime { get; set; } = default;

            /// <summary>
            /// BidAskElement Product
            /// </summary>
            public string Product { get; set; } = null!;

            /// <summary>
            /// BidAskElement BidAskValue
            /// </summary>
            public BidAskValue Value { get; set; } = null!;

        }
    }
}
