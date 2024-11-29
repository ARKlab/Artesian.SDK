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
    ///  Bid Ask Query Class
    /// </summary>
    public class BidAskQuery : QueryWithFillAndInterval<BidAskQueryParamaters>, IBidAskQuery<BidAskQuery>
    {        
        private const string _routePrefix = "ba";
        private readonly Client _client;
        private readonly IPartitionStrategy _partition;

        internal BidAskQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery ForMarketData(int[] ids)
        {
            base.ForMarketData(ids);
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery ForMarketData(int id)
        {
            base.ForMarketData(new int[] { id });
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery ForFilterId(int filterId)
        {
            base.ForFilterId(filterId);
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery InTimezone(string tz)
        {
            base.InTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            base.InAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery InRelativePeriodRange(Period from, Period to)
        {
            base.InRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery InRelativePeriod(Period extractionPeriod)
        {
            base.InRelativePeriod(extractionPeriod);
            return this;
        }
        /// <summary>
        /// Set the relative interval to be queried
        /// </summary>
        /// <param name="relativeInterval">The relative interval to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery InRelativeInterval(RelativeInterval relativeInterval)
        {
            _inRelativeInterval(relativeInterval);
            return this;
        }
        #endregion

        #region bid ask methods
        /// <summary>
        /// Set list of market products to be queried
        /// </summary>
        /// <param name="products">List of products to be queried</param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery ForProducts(params string[] products)
        {
            QueryParamaters.Products = products;
            return this;
        }
        /// <summary>
        /// Set the Filler strategy to Null
        /// </summary>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery WithFillNull()
        {
            QueryParamaters.FillerKindType = FillerKindType.Null;
            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Custom Value
        /// </summary>
        /// <param name="bidAskValue"></param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery WithFillCustomValue(BidAskValue bidAskValue)
        {
            QueryParamaters.FillerKindType = FillerKindType.CustomValue;
            QueryParamaters.FillerConfig.FillerBidAskDV = bidAskValue;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Latest Value
        /// </summary>
        /// <param name="period"></param>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery WithFillLatestValue(Period period)
        {
            QueryParamaters.FillerKindType = FillerKindType.LatestValidValue;
            QueryParamaters.FillerConfig.FillerPeriod = period;

            return this;
        }
        /// <summary>
        /// Set the Filler Strategy to Fill None
        /// </summary>
        /// <returns>BidAskQuery</returns>
        public BidAskQuery WithFillNone()
        {
            QueryParamaters.FillerKindType = FillerKindType.NoFill;

            return this;
        }
        /// <summary>
        /// Execute BidAskQuery
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of BidAskRow</returns>
        public async Task<IEnumerable<BidAskRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url => _client.Exec<IEnumerable<BidAskRow>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList);

            return res.SelectMany(x => x);
        }

        #region private
        private List<string> _buildRequest()
        {
            ValidateQuery();

            var urlList = _partition.Partition(new List<BidAskQueryParamaters> { QueryParamaters })
                .Select(qp => $"/{_routePrefix}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("p", qp.Products)
                        .SetQueryParam("tz", qp.TimeZone)
                        .SetQueryParam("fillerK",qp.FillerKindType)
                        .SetQueryParam("fillerDVbbp",qp.FillerConfig.FillerBidAskDV.BestBidPrice)
                        .SetQueryParam("fillerDVbap", qp.FillerConfig.FillerBidAskDV.BestAskPrice)
                        .SetQueryParam("fillerDVbbq", qp.FillerConfig.FillerBidAskDV.BestBidQuantity)
                        .SetQueryParam("fillerDVbaq", qp.FillerConfig.FillerBidAskDV.BestAskQuantity)
                        .SetQueryParam("fillerDVlp", qp.FillerConfig.FillerBidAskDV.LastPrice)
                        .SetQueryParam("fillerDVlq", qp.FillerConfig.FillerBidAskDV.LastQuantity)
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