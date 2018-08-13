﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Market Assessment Query Class
    /// </summary>
    public class MasQuery : Query, IMasQuery<MasQuery>
    {
        private IEnumerable<string> _products;
        private string _routePrefix = "mas";
        private Client _client;

        internal MasQuery(Client client)
        {
            _client = client;
        }

        #region facade methods
        /// <summary>
        /// Set the list of marketdata to be queried
        /// </summary>
        /// <param name="ids">Array of marketdata id's to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int[] ids)
        {
            _ids = ids;
            return this;
        }
        /// <summary>
        /// Set the marketdata to be queried
        /// </summary>
        /// <param name="id">The marketdata id to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery ForMarketData(int id)
        {
            _ids = new int[] { id };
            return this;
        }
        /// <summary>
        /// Specify the timezone of extracted marketdata. Defaults to UTC
        /// </summary>
        /// <param name="tz">Timezone in which to extract eg UTC/CET</param>
        /// <returns>MasQuery</returns>
        public MasQuery InTimezone(string tz)
        {
            _inTimezone(tz);
            return this;
        }
        /// <summary>
        /// Set the date range to be queried
        /// </summary>
        /// <param name="start">Start date of range</param>
        /// <param name="end">End date of range</param>
        /// <returns>MasQuery</returns>
        public MasQuery InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            _inAbsoluteDateRange(start, end);
            return this;
        }
        /// <summary>
        /// Set the relative period range from today to be queried
        /// </summary>
        /// <param name="from">Start period of range</param>
        /// <param name="to">End period of range</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriodRange(Period from, Period to)
        {
            _inRelativePeriodRange(from, to);
            return this;
        }
        /// <summary>
        /// Set the relative period from today to be queried
        /// </summary>
        /// <param name="extractionPeriod">Period to be queried</param>
        /// <returns>MasQuery</returns>
        public MasQuery InRelativePeriod(Period extractionPeriod)
        {
            _inRelativePeriod(extractionPeriod);
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
        /// <returns></returns>
        public MasQuery ForProducts(params string[] products)
        {
            _products = products;
            return this;
        }
        /// <summary>
        /// Execute MasQuery
        /// </summary>
        /// <param name="ctk"></param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public async Task<IEnumerable<AssessmentRow>> ExecuteAsync(CancellationToken ctk = default)
        {
            return await _client.Exec<IEnumerable<AssessmentRow>>(HttpMethod.Get, _buildRequest(), ctk: ctk);
        }

        #region private
        string _buildRequest()
        {
            _validateQuery();

            var url = $"/{_routePrefix}/{_buildExtractionRangeRoute()}"
            .SetQueryParam("id", _ids)
            .SetQueryParam("p", _products)
            .SetQueryParam("tz", _tz);

            return url.ToString();
        }

        protected override void _validateQuery()
        {
            base._validateQuery();

            if (_products == null)
                throw new ApplicationException("Products must be provided for extraction. Use .ForProducts() argument takes a string or string array of products");
        } 
        #endregion
        #endregion
    }
}