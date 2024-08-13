// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using System;
using Flurl;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Create new ApiKey
        /// </summary>
        /// <param name="apiKeyRecord">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        public Task<ApiKey.Output> CreateApiKeyAsync(ApiKey.Input apiKeyRecord, CancellationToken ctk = default)
        {
            apiKeyRecord.Validate();

            var url = "/apikey/entity";

            return _client.Exec<ApiKey.Output, ApiKey.Input>(HttpMethod.Post, url, apiKeyRecord, ctk: ctk);
        }
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="key">The Key</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        public Task<ApiKey.Output> ReadApiKeyByKeyAsync(string key, CancellationToken ctk = default)
        {
            var url = "/apikey/entity"
                    .SetQueryParam("key", key)
                    ;

            return _client.Exec<ApiKey.Output>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        public Task<ApiKey.Output> ReadApiKeyByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/apikey/entity".AppendPathSegment(id);

            return _client.Exec<ApiKey.Output>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Retrieve the apikeys paged
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="userId">The userid we want to filter for</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of ApiKey Output entity</returns>
        public Task<PagedResult<ApiKey.Output>> ReadApiKeysAsync(int page, int pageSize, string userId, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/apikey/entity"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page)
                    .SetQueryParam("userId", userId);

            return _client.Exec<PagedResult<ApiKey.Output>>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Delete the ApiKey
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task DeleteApiKeyAsync(int id, CancellationToken ctk = default)
        {
            var url = "/apikey/entity".AppendPathSegment(id);

            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }
    }
}
