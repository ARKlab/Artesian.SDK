// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Delete the curve data supplied in <paramref name="data"/>
        /// </summary>
        /// <remarks>
        /// Unified controller for deleting curve data
        /// ID, TimeZone and Range fields should not be null
        /// - Product: MarketDataAssessment, BidAsk and Auction should not be null. For Actual and Versioned should be null
        /// - Actual TimeSerie: Version fiels should be null
        /// - Versioned TimeSerie: Version fields should not be null
        /// </remarks>
        /// <param name="data">
        /// An object that represents Auction, BidAsk, MarketDataAssessment, ActualTimeSerie or VersionedTimeSerie
        /// </param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task DeleteCurveDataAsync(DeleteCurveData data, CancellationToken ctk = default)
        {
            data.Validate();

            var url = "/marketdata/deletedata";

            return _client.Exec(HttpMethod.Post, url, data, ctk: ctk);
        }
    }
}
