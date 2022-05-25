// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.GMEPublicOffer;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    public partial class GMEPublicOfferService
    {
        /// <summary>
        /// Upsert the curve data supplied in <paramref name="data"/>
        /// </summary>
        /// <remarks>
        /// Unified controller for saving curve data
        /// ID, TimeZone and DownloadedAt fields should not be null
        /// - Market Data Assessment: MarketAssessment field should not be null, other fields should be null
        /// - Actual TimeSerie: Rows field should not be null, other fields should be null-
        /// - Versioned TimeSerie: Rows and Version fields should not be null, other fields should be null
        /// </remarks>
        /// <param name="data">
        /// An object that represents GMEPublicOffer
        /// </param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task UpsertDataAsync(GMEPublicOfferUpsertDataDto data, CancellationToken ctk = default)
        {
            //data.Validate();

            var url = "/upsertdata";

            return _client.Exec(HttpMethod.Post, url, data, ctk: ctk);
        }
    }
}
