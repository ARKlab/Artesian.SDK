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
        private readonly IMarketDataService _marketDataService;
        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier;
        private List<BidAskElement> _values = new();

        /// <summary>
        /// BidAsks Constructor
        /// </summary>
        internal BidAsk(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            var entity = Guard.IsNotNull(marketData._entity);
            var marketDataService = Guard.IsNotNull(marketData._marketDataService);

            _entity = entity;
            _marketDataService = marketDataService;

            _identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);

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
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
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
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
        public AddBidAskOperationResult AddData(Instant time, string product, BidAskValue value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            return _addBidAsk(time.InUtc().LocalDateTime, product, value);
        }

        /// <summary>
        /// Attempts to add a data point to the BidAsk for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="LocalDate"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="localDate">The date of the value to add.</param>
        /// <param name="product">The product to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddTimeSerieOperationResult TryAddData(LocalDate localDate, string product, BidAskValue value, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Time granularity. Use TryAddData(Instant time, string product, BidAskValue value, KeyConflictPolicy keyConflictPolicy)");

            var localTime = localDate.AtMidnight();

            return _tryAdd(localTime, product, value, keyConflictPolicy);
        }

        /// <summary>
        /// Attempts to add a data point to the BidAsk for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="Instant"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="time">The date of the value to add.</param>
        /// <param name="product">The product to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddTimeSerieOperationResult TryAddData(Instant time, string product, BidAskValue value, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Date granularity. Use TryAddData(LocalDate localDate, string product, BidAskValue value, KeyConflictPolicy keyConflictPolicy)");

            var localTime = time.InUtc().LocalDateTime;

            return _tryAdd(localTime, product, value, keyConflictPolicy);
        }

        /// <summary>
        /// BidAsk SetData (bulk operation).
        /// Sets the internal data of the BidAsk using the provided values,
        /// keyed by LocalDateTime.
        /// 
        /// This method performs a bulk operation and does not apply per-record
        /// conflict resolution or validation on the input dictionary.
        /// </summary>
        /// <remarks>
        /// BulkSetPolicy options:
        /// Init:
        ///   Initializes the internal data only if it is empty;
        ///   otherwise an exception is thrown.
        /// 
        /// Replace:
        ///   Clears and completely replaces the internal data with the provided values.
        /// 
        /// This method is intended as a fast-path for scenarios where the caller
        /// has already constructed and validated the dictionary.
        /// Any remaining validations (e.g. granularity constraints) are enforced
        /// by server-side logic outside of this method.
        /// </remarks>
        public void SetData(List<BidAskElement> values, BulkSetPolicy bulkSetPolicy)
        {
            _setData(values, bulkSetPolicy);
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

        private AddTimeSerieOperationResult _tryAdd(LocalDateTime reportTime, string product, BidAskValue value, KeyConflictPolicy keyConflictPolicy)
        {
            var valueToAdd = new BidAskElement(reportTime, product, value);
            var existing = _values.FirstOrDefault(x => x.ReportTime == reportTime && x.Product == product);

            switch (keyConflictPolicy)
            {
                case KeyConflictPolicy.Overwrite:
                    if (existing != null)
                        _values.Remove(existing);
                    _values.Add(valueToAdd);
                    return AddTimeSerieOperationResult.ValueAdded;

                case KeyConflictPolicy.Throw:
                    if (existing != null)
                        throw new ArtesianSdkClientException("Data already present, cannot be updated!");
                    _values.Add(valueToAdd);
                    return AddTimeSerieOperationResult.ValueAdded;

                case KeyConflictPolicy.Skip:
                    if (existing != null)
                        return AddTimeSerieOperationResult.TimeAlreadyPresent;
                    _values.Add(valueToAdd);
                    return AddTimeSerieOperationResult.ValueAdded;

                default:
                    throw new NotSupportedException("KeyConflictPolicy not supported " + keyConflictPolicy);
            }
        }

        private void _setData(List<BidAskElement> values, BulkSetPolicy bulkSetPolicy)
        {
            switch (bulkSetPolicy)
            {
                case BulkSetPolicy.Replace:
                    _values = values;
                    break;
                case BulkSetPolicy.Init:
                    if (_values.Any())
                        throw new ArtesianSdkClientException("Data already present, cannot be updated!");
                    else
                        _values = values;
                    break;
                default:
                    throw new NotSupportedException("BulkSetPolicy not supported " + bulkSetPolicy);
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

        private async Task _save(Instant downloadedAt,
            bool deferCommandExecution,
            bool deferDataGeneration,
            bool keepNulls,
            UpsertMode? upsertMode,
            CancellationToken ctk)
        {
            if (_values.Count != 0)
            {
                var data = new UpsertCurveData
                {
                    ID = _identifier,
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
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, List<string>? product = null, string? timezone = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
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
                Product = product,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// BidAskElement entity
    /// </summary>
    public sealed record BidAskElement
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
        public LocalDateTime ReportTime { get; init; }
        /// <summary>
        /// BidAskElement Product
        /// </summary>
        public string Product { get; init; }
        /// <summary>
        /// BidAskElement BidAskValue
        /// </summary>
        public BidAskValue Value { get; init; }
    }
}
