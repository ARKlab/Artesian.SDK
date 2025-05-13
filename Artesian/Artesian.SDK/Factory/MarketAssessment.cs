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
    /// MarketAssessment entity
    /// </summary>
    internal sealed class MarketAssessment : IMarketAssessmentWritable
    {
        private readonly IMarketDataService _marketDataService;
        private readonly MarketDataEntity.Output _entity;
        private readonly MarketDataIdentifier _identifier;
        private readonly List<AssessmentElement> _values = new();

        /// <summary>
        /// MarketAssessment Constructor
        /// </summary>
        internal MarketAssessment(MarketData marketData)
        {
            Guard.IsNotNull(marketData);
            Guard.IsNotNull(marketData._entity);
            Guard.IsNotNull(marketData._marketDataService);

            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

            Assessments = new ReadOnlyCollection<AssessmentElement>(_values);
        }

        /// <summary>
        /// MarketData AssessmentElement
        /// </summary>
        public IReadOnlyCollection<AssessmentElement> Assessments { get; internal set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _values.Clear();
        }

        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddAssessmentOperationResult</returns>
        public AddAssessmentOperationResult AddData(LocalDate localDate, string product, MarketAssessmentValue value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Time granularity. Use AddData(Instant time...)");

            return _addAssessment(localDate.AtMidnight(), product, value);
        }
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddAssessmentOperationResult</returns>
        public AddAssessmentOperationResult AddData(Instant time, string product, MarketAssessmentValue value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new MarketAssessmentException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            return _addAssessment(time.InUtc().LocalDateTime, product, value);
        }

        private AddAssessmentOperationResult _addAssessment(LocalDateTime reportTime, string product, MarketAssessmentValue value)
        {
            //Relative products
            if (ArtesianConstants._relativeProductValidator.IsMatch(product))
                throw new NotSupportedException("Relative Products are not supported");

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new MarketAssessmentException("Trying to insert Report Time {0} with the wrong format to Assessment {1}. Should be of period {2}", reportTime, _identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new MarketAssessmentException("Trying to insert Report Time {0} with wrong the format to Assessment {1}. Should be of period {2}", reportTime, _identifier, period);
            }

            //if (reportTime.Date >= product.ReferenceDate)
            //    return AddAssessmentOperationResult.IllegalReferenceDate;

            if (_values.Any(row => row.ReportTime == reportTime && row.Product.Equals(product, StringComparison.Ordinal)))
                return AddAssessmentOperationResult.ProductAlreadyPresent;

            _values.Add(new AssessmentElement(reportTime, product, value));
            return AddAssessmentOperationResult.AssessmentAdded;
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
            bool deferCommandExecution ,
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
                    MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>(),
                    KeepNulls = keepNulls,
                    UpsertMode = upsertMode
                };

                foreach (var reportTime in _values.GroupBy(g => g.ReportTime))
                {
                    var assessments = reportTime.ToDictionary(key => key.Product, value => value.Value, StringComparer.Ordinal);
                    data.MarketAssessment.Add(reportTime.Key, assessments);
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
        /// <param name="product">Product of the MarketAssessment Time Serie</param>
        /// <param name="timezone">For DateSeries if provided must be equal to MarketData OrignalTimezone Default:MarketData OrignalTimezone. For TimeSeries Default:CET</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, List<string> product = null, string timezone = "CET", bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
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
        /// AssessmentElement entity
        /// </summary>
        public sealed class AssessmentElement
        {
            /// <summary>
            /// AssessmentElement constructor
            /// </summary>
            public AssessmentElement(LocalDateTime reportTime, string product, MarketAssessmentValue value)
            {
                ReportTime = reportTime;
                Product = product;
                Value = value;
            }

            /// <summary>
            /// AssessmentElement ReportTime
            /// </summary>
            public LocalDateTime ReportTime { get; set; }
            /// <summary>
            /// AssessmentElement Product
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// AssessmentElement MarketAssessmentValue
            /// </summary>
            public MarketAssessmentValue Value { get; set; }
        }
    }
}
