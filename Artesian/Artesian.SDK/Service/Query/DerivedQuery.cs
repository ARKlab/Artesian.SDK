// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

using Flurl;

using NodaTime;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Derived Time Serie Query Class
    /// </summary>
    public class DerivedQuery : QueryWithFillAndInterval<DerivedQueryParamaters>, IDerivedQuery<DerivedQuery>
    {
        private readonly Client _client;
        private readonly IPartitionStrategy _partition;

        private const string _routePrefix = "vts"; //what should this be?

        internal DerivedQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForMarketData(int[] ids)
        {
            _forMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForMarketData(int id)
        {
            _forMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForFilterId(int filterId)
        {
            _forFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InTimezone(string tz)
        {
            _inTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            _inAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InRelativePeriodRange(Period from, Period to)
        {
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to extraction
        /// </summary>
        /// <param name="tr">The Time Tramsform id to be applied to the extraction</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithTimeTransform(int tr)
        {
            QueryParamaters.TransformId = tr;
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to extraction
        /// </summary>
        /// <param name="tr">The system defined time transform to be applied to the extraction</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithTimeTransform(SystemTimeTransform tr)
        {
            QueryParamaters.TransformId = (int)tr;
            return this;
        }
        #endregion

        #region derived query methods
        /// <summary>
        /// Set the granularity of the extracted marketdata
        /// </summary>
        /// <param name="granularity">The granulairty in which to extract data. See <see cref="Granularity"/> for types of Granularity</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery InGranularity(Granularity granularity)
        {
            QueryParamaters.Granularity = granularity;
            return this;
        }
        /// <summary>
        /// Set the version selection type to MUV. 
        /// </summary>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForDerived(LocalDateTime? versionLimit = null)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.MUV;
            QueryParamaters.VersionLimit = versionLimit;
            return this;
        }
        /// <summary>
        /// Set a specific analysis date from wich apply the relative interval/period
        /// </summary>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForAnalysisDate(LocalDate analysisDate)
        {
            QueryParamaters.AnalysisDate = analysisDate;

            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithFillNull()
        {
            QueryParamaters.FillerKindType = FillerKindType.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithFillCustomValue(double value)
        {
            QueryParamaters.FillerKindType = FillerKindType.CustomValue;
            QueryParamaters.FillerConfig.FillerTimeSeriesDV = value;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithFillLatestValue(Period period)
        {
            QueryParamaters.FillerKindType = FillerKindType.LatestValidValue;
            QueryParamaters.FillerConfig.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery WithFillNone()
        {
            QueryParamaters.FillerKindType = FillerKindType.NoFill;

            return this;
        }
        /// <summary>
        /// Execute DerivedQuery
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of TimeSerieRow Actual</returns>
        public async Task<IEnumerable<TimeSerieRow.Versioned>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url=> _client.Exec<IEnumerable<TimeSerieRow.Versioned>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList);
            return res.SelectMany(x => x);
        }

        #region private
        /// <summary>
        /// Validate Query override
        /// </summary>
        protected override void _validateQuery()
        {
            base._validateQuery();

            if (QueryParamaters.Granularity == null)
                throw new ArtesianSdkClientException("Extraction granularity must be provided. Use .InGranularity() argument takes a granularity type");

            if (QueryParamaters.VersionSelectionType == null)
                throw new ArtesianSdkClientException("Version selection must be provided. Provide a version to query. eg .ForLastOfDays() arguments take a date range , period or period range");

            if (QueryParamaters.FillerKindType == FillerKindType.CustomValue)
            {
                if (QueryParamaters.FillerConfig.FillerTimeSeriesDV == null)
                {
                    throw new ArtesianSdkClientException("Filler default value must be provided. Provide a value for default value when using custom value filler");
                }
            }

            if (QueryParamaters.FillerKindType == FillerKindType.LatestValidValue)
            {
                if (QueryParamaters.FillerConfig.FillerPeriod.ToString().Contains('-') == true || QueryParamaters.FillerConfig.FillerPeriod == null)
                {
                    throw new ArtesianSdkClientException("Latest valid value filler must contain a non negative Period");
                }
            }

            if (QueryParamaters.ExtractionRangeType == ExtractionRangeType.DateRange && QueryParamaters.AnalysisDate != null)
                throw new ArtesianSdkClientException("Analysis should be related to a Period. Provide a period or remove analysis date.");
        }

        private string _buildVersionRoute(DerivedQueryParamaters queryParamaters)
        {
            string subPath;

            switch (queryParamaters.VersionSelectionType.Value)
            {
                case VersionSelectionType.LastN:
                    subPath = $"Last{queryParamaters.VersionSelectionConfig.LastN}";
                    break;
                case VersionSelectionType.MostRecent:
                    subPath = _buildMostRecentSubRoute(queryParamaters);
                    break;
                case VersionSelectionType.MUV:
                    subPath = $"MUV";
                    break;
                case VersionSelectionType.LastOfDays:
                case VersionSelectionType.LastOfMonths:
                    subPath = _buildLastOfSubRoute(queryParamaters);
                    break;
                case VersionSelectionType.Version:
                    subPath = $"Version/{_toUrlParam(queryParamaters.VersionSelectionConfig.Version)}";
                    break;
                default:
                    throw new NotSupportedException("Unsupported version type");
            }

            return subPath;
        }

        private string _buildMostRecentSubRoute(DerivedQueryParamaters queryParamaters)
        {
            string subPath;

            if (queryParamaters.VersionSelectionConfig.MostRecent.DateStart != null && queryParamaters.VersionSelectionConfig.MostRecent.DateEnd != null)
                subPath = $"MostRecent/{_toUrlParam(queryParamaters.VersionSelectionConfig.MostRecent.DateStart.Value, queryParamaters.VersionSelectionConfig.MostRecent.DateEnd.Value)}";
            else if (queryParamaters.VersionSelectionConfig.MostRecent.Period != null)
                subPath = $"MostRecent/{queryParamaters.VersionSelectionConfig.MostRecent.Period}";
            else if (queryParamaters.VersionSelectionConfig.MostRecent.PeriodFrom != null && queryParamaters.VersionSelectionConfig.MostRecent.PeriodTo != null)
                subPath = $"MostRecent/{queryParamaters.VersionSelectionConfig.MostRecent.PeriodFrom}/{queryParamaters.VersionSelectionConfig.MostRecent.PeriodTo}";
            else
                subPath = $"MostRecent";

            return subPath;
        }

        private string _buildLastOfSubRoute(DerivedQueryParamaters queryParamaters)
        {
            string subPath;

            if (queryParamaters.VersionSelectionConfig.LastOf.DateStart != null && queryParamaters.VersionSelectionConfig.LastOf.DateEnd != null)
                subPath = $"{queryParamaters.VersionSelectionType}/{_toUrlParam(queryParamaters.VersionSelectionConfig.LastOf.DateStart.Value, queryParamaters.VersionSelectionConfig.LastOf.DateEnd.Value)}";
            else if (queryParamaters.VersionSelectionConfig.LastOf.Period != null)
                subPath = $"{queryParamaters.VersionSelectionType}/{queryParamaters.VersionSelectionConfig.LastOf.Period}";
            else if (queryParamaters.VersionSelectionConfig.LastOf.PeriodFrom != null && queryParamaters.VersionSelectionConfig.LastOf.PeriodTo != null)
                subPath = $"{queryParamaters.VersionSelectionType}/{queryParamaters.VersionSelectionConfig.LastOf.PeriodFrom}/{queryParamaters.VersionSelectionConfig.LastOf.PeriodTo}";
            else
                throw new ArtesianSdkClientException("LastOf extraction type not defined");

            return subPath;
        }

        private List<string> _buildRequest()
        {
            _validateQuery();

            var urlList = _partition.Partition(new List<DerivedQueryParamaters> { QueryParamaters })
                    .Select(qp => $"/{_routePrefix}/{_buildVersionRoute(qp)}/{qp.Granularity}/{_buildExtractionRangeRoute(qp)}"
                            .SetQueryParam("id", qp.Ids)
                            .SetQueryParam("filterId", qp.FilterId)
                            .SetQueryParam("tz", qp.TimeZone)
                            .SetQueryParam("tr", qp.TransformId)
                            .SetQueryParam("versionLimit", qp.VersionLimit)
                            .SetQueryParam("fillerK", qp.FillerKindType)
                            .SetQueryParam("fillerDV", qp.FillerConfig.FillerTimeSeriesDV)
                            .SetQueryParam("fillerP", qp.FillerConfig.FillerPeriod)
                            .SetQueryParam("ad",
                                qp.AnalysisDate.HasValue
                                ? qp.AnalysisDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                                : null
                            )
                            .ToString())
                    .ToList();

            return urlList;
        }

        #endregion
        #endregion
    }
}