// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// A sequence of operations will be applied to the metadata identified by ids
        /// </summary>
        /// <param name="operations">Operations</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<IList<MarketDataEntity.Output>> PerformOperationsAsync(Operations operations, CancellationToken ctk = default)
        {
            operations.Validate();

            var url = "/marketdata/operations";

            return _client.Exec<IList<MarketDataEntity.Output>, Operations>(HttpMethod.Post, url, operations, ctk: ctk);
        }
    }
}
