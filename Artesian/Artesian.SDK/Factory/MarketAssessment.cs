using Ark.Tools.Nodatime;

using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// MarketAssessment entity
    /// </summary>
    internal sealed class MarketAssessment : IMarketAssessmentWritable
    {
        private IMarketDataService _marketDataService;
        private MarketDataEntity.Output _entity = null;
        private readonly MarketDataIdentifier _identifier = null;

        /// <summary>
        /// MarketAssessment Constructor
        /// </summary>
        internal MarketAssessment(MarketData marketData)
        {
            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

            Assessments = new List<AssessmentElement>();
        }

        /// <summary>
        /// MarketData AssessmentElement
        /// </summary>
        public List<AssessmentElement> Assessments { get; internal set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            Assessments.Clear();
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
            if (Regex.IsMatch(product, @"\+\d+$"))
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

            if (Assessments.Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                return AddAssessmentOperationResult.ProductAlreadyPresent;

            Assessments.Add(new AssessmentElement(reportTime, product, value));
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
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default)
        {
            Ensure.Any.IsNotNull(_entity);

            if (Assessments.Any())
            {
                var data = new UpsertCurveData(_identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    DeferCommandExecution = deferCommandExecution,
                    MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>(),
                    KeepNulls = keepNulls
                };

                foreach (var reportTime in Assessments.GroupBy(g => g.ReportTime))
                {
                    var assessments = reportTime.ToDictionary(key => key.Product.ToString(), value => value.Value);
                    data.MarketAssessment.Add(reportTime.Key, assessments);
                }

                await _marketDataService.UpsertCurveDataAsync(data, ctk);
            }
        }

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeToDelete">Range to be deleted</param>
        /// <param name="version">Version of the Time Serie</param>
        /// <param name="product">Product of the MarketAssessment, BidAsk or Auction Time Serie</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Delete(LocalDateTimeRange rangeToDelete, LocalDateTime? version = null, List<string> product = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
        {
            Ensure.Any.IsNotNull(_entity);

            var data = new DeleteCurveData(_identifier)
            {
                Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                Range = rangeToDelete,
                DeferCommandExecution = deferCommandExecution,
                DeferDataGeneration = deferDataGeneration,
                Version = version,
                Product = product,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk);
        }

        /// <summary>
        /// AssessmentElement entity
        /// </summary>
        public class AssessmentElement
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
