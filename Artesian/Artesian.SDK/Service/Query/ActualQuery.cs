// Copyright (c) ARK LTD. All rights reserved.
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
    /// Actual Time Serie Query Class
    /// </summary>
    public class ActualQuery : QueryWithFillAndInterval<ActualQueryParamaters>, IActualQuery<ActualQuery>
    {
        private Client _client;
        private IPartitionStrategy _partition;

        private string _routePrefix = "ts";

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
            _forMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery ForMarketData(int id)
        {
            _forMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery ForFilterId(int filterId)
        {
            _forFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InTimezone(string tz)
        {
            _inTimezone(tz);
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
            _inAbsoluteDateRange(start, end);
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
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
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
            _queryParamaters.TransformId = tr;
            return this;
        }
        /// <summary>
        /// Set the time transform to be applied to extraction
        /// </summary>
        /// <param name="tr">The system defined time transform to be applied to the extraction</param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithTimeTransform(SystemTimeTransform tr)
        {
            _queryParamaters.TransformId = (int)tr;
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
            _queryParamaters.Granularity = granularity;
            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillNull()
        {
            _queryParamaters.FillerKindType = FillerKindType.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fillerContinue"></param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillCustomValue(double value, bool fillerContinue = false)
        {
            _queryParamaters.FillerKindType = FillerKindType.CustomValue;
            _queryParamaters.FillerContinue = fillerContinue;
            _queryParamaters.FillerConfig.FillerTimeSeriesDV = value;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <param name="fillerContinue"></param>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillLatestValue(Period period, bool fillerContinue = false)
        {
            _queryParamaters.FillerKindType = FillerKindType.LatestValidValue;
            _queryParamaters.FillerContinue = fillerContinue;
            _queryParamaters.FillerConfig.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>ActualQuery</returns>
        public ActualQuery WithFillNone()
        {
            _queryParamaters.FillerKindType = FillerKindType.NoFill;

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
            _validateQuery();

            var urlList = _partition.Partition(new List<ActualQueryParamaters> { _queryParamaters })
                .Select(qp => $"/{_routePrefix}/{qp.Granularity}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("tz", qp.TimeZone)
                        .SetQueryParam("tr", qp.TransformId)
                        .SetQueryParam("fillerK", qp.FillerKindType)
                        .SetQueryParam("fillerC", qp.FillerContinue)
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

            if (_queryParamaters.Granularity == null)
                throw new ApplicationException("Extraction granularity must be provided. Use .InGranularity() argument takes a granularity type");

            if (_queryParamaters.FillerKindType == FillerKindType.LatestValidValue)
            {
                if (_queryParamaters.FillerConfig.FillerPeriod.ToString().Contains('-') == true || _queryParamaters.FillerConfig.FillerPeriod == null)
                {
                    throw new ApplicationException("Latest valid value filler must contain a non negative Period");
                }
            }
        }

      
        #endregion
        #endregion
    }
}