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
    /// ActualTimeSerie entity
    /// </summary>
    internal sealed class ActualTimeSerie : ITimeserieWritable
    {
        private readonly IMarketDataService _marketDataService;
        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier;
        private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();

        /// <summary>
        /// ActualTimeSerie Constructor
        /// </summary>
        internal ActualTimeSerie(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            var entity = Guard.IsNotNull(marketData._entity);
            var marketDataService = Guard.IsNotNull(marketData._marketDataService);

            _entity = entity;
            _marketDataService = marketDataService;

            _identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);

            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        /// <summary>
        /// ActualTimeSerie Curve Values
        /// </summary>
        public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _values.Clear();
        }

        /// <summary>
        /// ActualTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with LocalDate
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
        public AddTimeSerieOperationResult AddData(LocalDate localDate, double? value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, double? value)");

            var localTime = localDate.AtMidnight();

            return _add(localTime, value);
        }
        /// <summary>
        /// ActualTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        [Obsolete("AddData is deprecated. Use TryAddData(...)", false)]
        public AddTimeSerieOperationResult AddData(Instant time, double? value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, double? value)");

            var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, value);
        }

        /// <summary>
        /// Attempts to add a data point to the ActualTimeSerie for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="LocalDate"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="localDate">The date of the value to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddTimeSerieOperationResult TryAddData(LocalDate localDate, double? value, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Time granularity. Use TryAddData(Instant time, double? value, KeyConflictPolicy keyConflictPolicy)");

            var localTime = localDate.AtMidnight();

            return _tryAdd(localTime, value, keyConflictPolicy);
        }

        /// <summary>
        /// Attempts to add a data point to the ActualTimeSerie for a specific date.
        /// </summary>
        /// <remarks>
        /// Adds a value to the series keyed by <see cref="Instant"/>. The behavior when a value for the same date already exists
        /// is controlled by <paramref name="keyConflictPolicy"/>.
        /// </remarks>
        /// <param name="time">The date of the value to add.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="keyConflictPolicy">Specifies what to do if a value already exists for the given date (Throw, Overwrite, Skip). Default value is Skip.</param>
        /// <returns>An <see cref="AddTimeSerieOperationResult"/> indicating the outcome of the operation.</returns>
        public AddTimeSerieOperationResult TryAddData(Instant time, double? value, KeyConflictPolicy keyConflictPolicy = KeyConflictPolicy.Skip)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Date granularity. Use TryAddData(LocalDate date, double? value, KeyConflictPolicy keyConflictPolicy)");

            var localTime = time.InUtc().LocalDateTime;

            return _tryAdd(localTime, value, keyConflictPolicy);
        }

        /// <summary>
        /// ActualTimeSerie SetData (bulk operation).
        /// Sets the internal data of the ActualTimeSerie using the provided values,
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
        public void SetData(Dictionary<LocalDateTime, double?> values, BulkSetPolicy bulkSetPolicy)
        {
            _setData(values, bulkSetPolicy);
        }

        private AddTimeSerieOperationResult _add(LocalDateTime localTime, double? value)
        {
            if (_values.ContainsKey(localTime))
                return AddTimeSerieOperationResult.TimeAlreadyPresent;

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, _identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, _identifier, period);
            }

            _values.Add(localTime, value);
            return AddTimeSerieOperationResult.ValueAdded;
        }

        private AddTimeSerieOperationResult _tryAdd(LocalDateTime localTime, double? value, KeyConflictPolicy keyConflictPolicy)
        {
            var exists = _values.ContainsKey(localTime);

            switch (keyConflictPolicy)
            {
                case KeyConflictPolicy.Overwrite:
                    if (exists)
                    {
                        _values[localTime] = value;
                        return AddTimeSerieOperationResult.ValueAdded;
                    }
                    return _add(localTime, value);
                case KeyConflictPolicy.Throw:
                    if (exists)
                        throw new ArtesianSdkClientException("Data already present, cannot be updated!");
                    return _add(localTime, value);
                case KeyConflictPolicy.Skip:
                    if (exists)
                        return AddTimeSerieOperationResult.TimeAlreadyPresent;
                    return _add(localTime, value);
                default:
                    throw new NotSupportedException("KeyConflictPolicy not supported " + keyConflictPolicy);
            }
        }

        private void _setData(Dictionary<LocalDateTime, double?> values, BulkSetPolicy bulkSetPolicy)
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

        private async Task _save(
            Instant downloadedAt,
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
                    Rows = _values,
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
                RangeStart = rangeStart ?? default,
                RangeEnd = rangeEnd ?? LocalDateTime.MaxIsoValue.Date.AtMidnight(),
                DeferCommandExecution = deferCommandExecution,
                DeferDataGeneration = deferDataGeneration,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk).ConfigureAwait(false);
        }
    }
}