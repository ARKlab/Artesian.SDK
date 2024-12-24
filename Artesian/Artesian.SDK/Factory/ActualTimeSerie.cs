using Artesian.SDK.Dto;
using Artesian.SDK.Common;
using Artesian.SDK.Service;
using NodaTime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();

        /// <summary>
        /// ActualTimeSerie Constructor
        /// </summary>
        internal ActualTimeSerie(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            Guard.IsNotNull(marketData._entity);
            Guard.IsNotNull(marketData._marketDataService);

            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

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
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
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
        public AddTimeSerieOperationResult AddData(Instant time, double? value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, double? value)");

            var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, value);
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
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default)
        {
            if (_values.Count != 0)
            {
                var data = new UpsertCurveData(_identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    Rows = _values,
                    DeferCommandExecution = deferCommandExecution,
                    DeferDataGeneration = deferDataGeneration,
                    KeepNulls = keepNulls
                };

                await _marketDataService.UpsertCurveDataAsync(data, ctk);
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
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, string timezone = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
        {
            var data = new DeleteCurveData(_identifier)
            {
                Timezone = timezone,
                // LocalDate.MinIsoValue has year -9998 and yearOfEra 9999. Using it without any string formatting, we got date 01-01-9999.
                // So we use default(LocalDateTime) 01/01/0001
                RangeStart = rangeStart ?? default,
                RangeEnd = rangeEnd ?? LocalDateTime.MaxIsoValue.Date.AtMidnight(),
                DeferCommandExecution = deferCommandExecution,
                DeferDataGeneration = deferDataGeneration,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk);
        }
    }
}