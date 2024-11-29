// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DerivedCfg;
using Artesian.SDK.Dto.DerivedCfg.Enums;

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
    /// Actual Time Serie Query Class
    /// </summary>
    public class ActualQuery : QueryWithFillAndInterval<ActualQueryParamaters>, IActualQuery<ActualQuery>
    {
        private readonly Client _client;
        private readonly IPartitionStrategy _partition;

        private const string _routePrefix = "ts";

        internal ActualQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery ForMarketData(int[] ids)
        {
            base.ForMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery ForMarketData(int id)
        {
            base.ForMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery ForFilterId(int filterId)
        {
            base.ForFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InTimezone(string tz)
        {
            base.InTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            base.InAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InRelativePeriodRange(Period from, Period to)
        {
            base.InRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InRelativePeriod(Period extractionPeriod)
        {
            base.InRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to extraction
        /// </summary>
        /// <param name="tr">The Time Tramsform id to be applied to the extraction</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithTimeTransform(int tr)
        {
            QueryParamaters.TransformId = tr;
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to extraction
        /// </summary>
        /// <param name="tr">The system defined time transform to be applied to the extraction</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithTimeTransform(SystemTimeTransform tr)
        {
            QueryParamaters.TransformId = (int)tr;
            return this;
        }
        #endregion

        #region actual query methods
        /// <summary>
        /// Set the granularity of the extracted marketdata
        /// </summary>
        /// <param name="granularity">The granulairty in which to extract data. See <see cref="Granularity"/> for types of Granularity</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InGranularity(Granularity granularity)
        {
            QueryParamaters.Granularity = granularity;
            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillNull()
        {
            QueryParamaters.FillerKindType = FillerKindType.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillCustomValue(double value)
        {
            QueryParamaters.FillerKindType = FillerKindType.CustomValue;
            QueryParamaters.FillerConfig.FillerTimeSeriesDV = value;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillLatestValue(Period period)
        {
            QueryParamaters.FillerKindType = FillerKindType.LatestValidValue;
            QueryParamaters.FillerConfig.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillNone()
        {
            QueryParamaters.FillerKindType = FillerKindType.NoFill;

            return this;
        }
        /// <summary>
        /// Execute ActualQuery
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
            ValidateQuery();

            var urlList = _partition.Partition(new List<ActualQueryParamaters> { QueryParamaters })
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
        protected sealed override void ValidateQuery()
        {
            base.ValidateQuery();

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