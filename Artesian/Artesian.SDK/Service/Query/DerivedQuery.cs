﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

using Flurl;

using NodaTime;

using System;
using System.Collections.Generic;
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

        private const string _routePrefix = "ts"; //what should this be?

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
        /// Set the number of versions to retrieve in the extraction
        /// </summary>
        /// <param name="lastN">The number of previous versions to extract</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastNVersions(int lastN)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.LastN;
            QueryParamaters.VersionSelectionConfig.LastN = lastN;
            return this;
        }
        /// <summary>
        /// Set the version selection type to MUV. 
        /// </summary>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForMUV(LocalDateTime? versionLimit = null)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.MUV;
            QueryParamaters.VersionLimit = versionLimit;
            return this;
        }
        /// <summary>
        /// Set MostRecent version selection
        /// </summary>
        /// <returns></returns>
        public DerivedQuery ForMostRecent()
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.MostRecent;

            return this;
        }
        /// <summary>
        /// Set Most Recent date range version selection
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DerivedQuery ForMostRecent(LocalDate start, LocalDate end)
        {
            return ForMostRecent(start.AtMidnight(), end.AtMidnight());
        }
        /// <summary>
        /// Set Most Recent date range version selection
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DerivedQuery ForMostRecent(LocalDateTime start, LocalDateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End datetime " + end + " must be greater than start datetime " + start);

            QueryParamaters.VersionSelectionType = VersionSelectionType.MostRecent;
            QueryParamaters.VersionSelectionConfig.MostRecent.DateStart = start;
            QueryParamaters.VersionSelectionConfig.MostRecent.DateEnd = end;

            return this;
        }
        /// <summary>
        /// Set Most Recent period range version selection
        /// </summary>
        /// <param name="from">Start period of version range</param>
        /// <param name="to">End period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForMostRecent(Period from, Period to)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.MostRecent;
            QueryParamaters.VersionSelectionConfig.MostRecent.PeriodFrom = from;
            QueryParamaters.VersionSelectionConfig.MostRecent.PeriodTo = to;

            return this;
        }
        /// <summary>
        /// Set Most Recent period version selection
        /// </summary>
        /// <param name="mostRecentPeriod">Period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForMostRecent(Period mostRecentPeriod)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.MostRecent;
            QueryParamaters.VersionSelectionConfig.MostRecent.Period = mostRecentPeriod;

            return this;
        }
        /// <summary>
        /// Set Last Of Days date range version selection
        /// </summary>
        /// <param name="start">Start date of version range</param>
        /// <param name="end">End date of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfDays(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfDays;
            QueryParamaters.VersionSelectionConfig.LastOf.DateStart = start;
            QueryParamaters.VersionSelectionConfig.LastOf.DateEnd = end;

            return this;
        }
        /// <summary>
        /// Set Last Of Days period range version selection
        /// </summary>
        /// <param name="from">Start period of version range</param>
        /// <param name="to">End period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfDays(Period from, Period to)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfDays;
            QueryParamaters.VersionSelectionConfig.LastOf.PeriodFrom = from;
            QueryParamaters.VersionSelectionConfig.LastOf.PeriodTo = to;

            return this;
        }
        /// <summary>
        /// Set Last Of Days period version selection
        /// </summary>
        /// <param name="lastOfPeriod">Period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfDays(Period lastOfPeriod)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfDays;
            QueryParamaters.VersionSelectionConfig.LastOf.Period = lastOfPeriod;

            return this;
        }
        /// <summary>
        /// Set Last of Months date range version selection
        /// </summary>
        /// <param name="start">Start date of version range</param>
        /// <param name="end">End date of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfMonths(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfMonths;
            QueryParamaters.VersionSelectionConfig.LastOf.DateStart = start;
            QueryParamaters.VersionSelectionConfig.LastOf.DateEnd = end;

            return this;
        }
        /// <summary>
        /// Set Last of Months period version selection
        /// </summary>
        /// <param name="lastOfPeriod">Period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfMonths(Period lastOfPeriod)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfMonths;
            QueryParamaters.VersionSelectionConfig.LastOf.Period = lastOfPeriod;

            return this;
        }
        /// <summary>
        /// Set Last of Months period range version selection
        /// </summary>
        /// <param name="from">Start period of version range</param>
        /// <param name="to">End period of version range</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForLastOfMonths(Period from, Period to)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.LastOfMonths;
            QueryParamaters.VersionSelectionConfig.LastOf.PeriodFrom = from;
            QueryParamaters.VersionSelectionConfig.LastOf.PeriodTo = to;

            return this;
        }
        /// <summary>
        /// Set specific version selection
        /// </summary>
        /// <param name="version">Date time of the version to be extracted</param>
        /// <returns>DerivedQuery</returns>
        public DerivedQuery ForVersion(LocalDateTime version)
        {
            QueryParamaters.VersionSelectionType = VersionSelectionType.Version;
            QueryParamaters.VersionSelectionConfig.Version = version;

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
        public async Task<IEnumerable<TimeSerieRow.Actual>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url=> _client.Exec<IEnumerable<TimeSerieRow.Actual>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList);
            return res.SelectMany(x => x);
        }

        #region private
        private List<string> _buildRequest()
        {
            _validateQuery();

            var urlList = _partition.Partition(new List<DerivedQueryParamaters> { QueryParamaters })
                .Select(qp => $"/{_routePrefix}/{qp.Granularity}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("tz", qp.TimeZone)
                        .SetQueryParam("tr", qp.TransformId)
                        .SetQueryParam("fillerK", qp.FillerKindType)
                        .SetQueryParam("fillerDV", qp.FillerConfig.FillerTimeSeriesDV)
                        .SetQueryParam("fillerP", qp.FillerConfig.FillerPeriod)
                        .ToString())
                .ToList();

            return urlList;
        }

        /// <summary>
        /// Validate Query override
        /// </summary>
        protected sealed override void _validateQuery()
        {
            base._validateQuery();

            if (QueryParamaters.Granularity == null)
                throw new ArtesianSdkClientException("Extraction granularity must be provided. Use .InGranularity() argument takes a granularity type");

            if (QueryParamaters.FillerKindType == FillerKindType.LatestValidValue)
            {
                if (QueryParamaters.FillerConfig.FillerPeriod.ToString().Contains('-') == true || QueryParamaters.FillerConfig.FillerPeriod == null)
                {
                    throw new ArtesianSdkClientException("Latest valid value filler must contain a non negative Period");
                }
            }
        }

      
        #endregion
        #endregion
    }
}