// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Derived Transform Query Validation
        /// </summary>
        /// <param name="request">Request containing TimeSerieData and the Query to be applied to verify the derived transformation</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public Task<DerivedTransformQueryValidationResponse.V1> DerivedTransformQueryValidation(DerivedTransformQueryValidation.V1 request, CancellationToken ctk = default)
        {
            var url = "/utils/derivedTransform/queryValidation";

            return _client.Exec<DerivedTransformQueryValidationResponse.V1, DerivedTransformQueryValidation.V1>(HttpMethod.Post, url, request, ctk: ctk);
        }
    }
}
