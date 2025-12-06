// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

using Flurl;

using NodaTime;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Market Assessment Query Class
    /// </summary>
    public sealed class MasQuery : QueryWithFillAndInterval<MasQueryParamaters>, IMasQuery<MasQuery>
    {        
        private readonly string _routePrefix = "mas";
        private readonly Client _client;
        private readonly IPartitionStrategy _partition;

        internal MasQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>MasQuery</returns>
        public new MasQuery ForMarketData(int[] ids)
        {
            base.ForMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int id)
        {
            base.ForMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>MasQuery</returns>
        public new MasQuery ForFilterId(int filterId)
        {
            base.ForFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>MasQuery</returns>
        public new MasQuery InTimezone(string tz)
        {
            base.InTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>MasQuery</returns>
        public new MasQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            base.InAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>MasQuery</returns>
        public new MasQuery InRelativePeriodRange(Period from, Period to)
        {
            base.InRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>MasQuery</returns>
        public new MasQuery InRelativePeriod(Period extractionPeriod)
        {
            base.InRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        #endregion

        #region market assessment methods
        /// <summary>
        /// Set list of market products to be queried
        /// </summary>
        /// <param name="products">List of products to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForProducts(params string[] products)
        {
            QueryParamaters.Products = products;
            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillNull()
        {
            QueryParamaters.FillerKindType = FillerKindType.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="marketAssessmentValue"></param>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillCustomValue(MarketAssessmentValue marketAssessmentValue)
        {
            QueryParamaters.FillerKindType = FillerKindType.CustomValue;
            QueryParamaters.FillerConfig.FillerMasDV = marketAssessmentValue;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillLatestValue(Period period)
        {
            QueryParamaters.FillerKindType = FillerKindType.LatestValidValue;
            QueryParamaters.FillerConfig.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>MasQuery</returns>
        public MasQuery WithFillNone()
        {
            QueryParamaters.FillerKindType = FillerKindType.NoFill;

            return this;
        }
        /// <summary>
        /// Execute MasQuery
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of AssessmentRow</returns>
        public async Task<IEnumerable<AssessmentRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url => _client.Exec<IEnumerable<AssessmentRow>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList).ConfigureAwait(false);

            return res.Where(x => x != null).SelectMany(x => x);
        }

        #region private
        private List<string> _buildRequest()
        {
            ValidateQuery();

            var urlList = _partition.Partition(new List<MasQueryParamaters> { QueryParamaters })
                .Select(qp => $"/{_routePrefix}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("p", qp.Products)
                        .SetQueryParam("tz", qp.TimeZone)
                        .SetQueryParam("fillerK",qp.FillerKindType)
                        .SetQueryParam("fillerDVs",qp.FillerConfig.FillerMasDV.Settlement)
                        .SetQueryParam("fillerDVo", qp.FillerConfig.FillerMasDV.Open)
                        .SetQueryParam("fillerDVc", qp.FillerConfig.FillerMasDV.Close)
                        .SetQueryParam("fillerDVh", qp.FillerConfig.FillerMasDV.High)
                        .SetQueryParam("fillerDVl", qp.FillerConfig.FillerMasDV.Low)
                        .SetQueryParam("fillerDVvp", qp.FillerConfig.FillerMasDV.VolumePaid)
                        .SetQueryParam("fillerDVvg", qp.FillerConfig.FillerMasDV.VolumeGiven)
                        .SetQueryParam("fillerDVvt", qp.FillerConfig.FillerMasDV.Volume)
                        .SetQueryParam("fillerP" ,qp.FillerConfig.FillerPeriod)
                        .ToString())
                .ToList();

            return urlList;
        }
        /// <summary>
        /// Validate Query override
        /// </summary>
        protected override void ValidateQuery()
        {
            base.ValidateQuery();

            if (QueryParamaters.Products == null)
                throw new ArtesianSdkClientException("Products must be provided for extraction. Use .ForProducts() argument takes a string or string array of products");

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