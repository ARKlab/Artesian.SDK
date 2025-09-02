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
    /// Auction Time Serie Query Class
    /// </summary>
    public sealed class AuctionQuery : QueryWithRange<AuctionQueryParamaters>, IAuctionQuery<AuctionQuery>
    {
        private readonly Client _client;
        private readonly IPartitionStrategy _partition;

        private const string _routePrefix = "auction";

        internal AuctionQuery(Client client, IPartitionStrategy partiton)
        {
            _client = client;
            _partition = partiton;
        }

        #region facade methods

        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery ForMarketData(int[] ids)
        {
            base.ForMarketData(ids);
            return this;
        }

        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>AuctionQuery</returns>
        public AuctionQuery ForMarketData(int id)
        {
            base.ForMarketData(new int[] { id });
            return this;
        }

        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery ForFilterId(int filterId)
        {
            base.ForFilterId(filterId);
            return this;
        }

        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery InTimezone(string tz)
        {
            base.InTimezone(tz);
            return this;
        }

        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            base.InAbsoluteDateRange(start, end);
            return this;
        }

        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery InRelativePeriodRange(Period from, Period to)
        {
            base.InRelativePeriodRange(from, to);
            return this;
        }

        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>AuctionQuery</returns>
        public new AuctionQuery InRelativePeriod(Period extractionPeriod)
        {
            base.InRelativePeriod(extractionPeriod);
            return this;
        }

        /// <summary>
        /// Execute Auction
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of TimeSerieRow Auction</returns>
        public async Task<IEnumerable<AuctionRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            List<string> urls = _buildRequest();

            var taskList = urls.Select(url => _client.Exec<IEnumerable<AuctionRow>>(HttpMethod.Get, url, ctk: ctk));

            var res = await Task.WhenAll(taskList).ConfigureAwait(false);
            return res.SelectMany(x => x);
        }

        #endregion facade methods

        #region auction query methods

        #region private

        private List<string> _buildRequest()
        {
            ValidateQuery();

            var urlList = _partition.Partition(new List<AuctionQueryParamaters> { QueryParamaters })
                .Select(qp => $"/{_routePrefix}/{_buildExtractionRangeRoute(qp)}"
                        .SetQueryParam("id", qp.Ids)
                        .SetQueryParam("filterId", qp.FilterId)
                        .SetQueryParam("tz", qp.TimeZone)
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
        }

        /// <summary>
        /// Build extraction range
        /// </summary>
        /// <returns>string</returns>
        private string _buildExtractionRangeRoute(QueryWithRangeParamaters queryParamaters)
        {
            string subPath;
            switch (queryParamaters.ExtractionRangeType)
            {
                case ExtractionRangeType.DateRange:
                    subPath = $"{QueryWithRange<AuctionQueryParamaters>.ToUrlParam(queryParamaters.ExtractionRangeSelectionConfig.DateStart, queryParamaters.ExtractionRangeSelectionConfig.DateEnd)}";
                    break;

                case ExtractionRangeType.PeriodRange:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.PeriodFrom}/{queryParamaters.ExtractionRangeSelectionConfig.PeriodTo}";
                    break;

                case ExtractionRangeType.Period:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.Period}";
                    break;

                default:
                    throw new NotSupportedException("ExtractionRangeType");
            }

            return subPath;
        }

        #endregion private

        #endregion auction query methods
    }
}