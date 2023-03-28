// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Create a new Authorization Group
        /// </summary>
        /// <param name="group">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Auth Group entity</returns>
        public Task<AuthGroup> CreateAuthGroup(AuthGroup group, CancellationToken ctk = default)
        {
            var url = "/group";

            return _client.Exec<AuthGroup, AuthGroup>(HttpMethod.Post, url, group, ctk);
        }
        /// <summary>
        /// Update an Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="group">The entity to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Auth Group entity</returns>
        public Task<AuthGroup> UpdateAuthGroup(int groupID, AuthGroup group, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec<AuthGroup, AuthGroup>(HttpMethod.Put, url, group, ctk);
        }
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task RemoveAuthGroup(int groupID, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec(HttpMethod.Delete, url, ctk);
        }
        /// <summary>
        /// Read Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>AuthGroup entity</returns>
        public Task<AuthGroup> ReadAuthGroup(int groupID, CancellationToken ctk = default)
        {
            var url = "/group".AppendPathSegment(groupID);

            return _client.Exec<AuthGroup>(HttpMethod.Get, url, ctk);
        }
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of Auth Group entity</returns>
        public Task<PagedResult<AuthGroup>> ReadAuthGroups(int page, int pageSize, CancellationToken ctk = default)
        {
            var url = "/group"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page);

            return _client.Exec<PagedResult<AuthGroup>>(HttpMethod.Get, url, ctk);
        }
        /// <summary>
        /// Get a list of Principals of the selected user
        /// </summary>
        /// <param name="user">The user name</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>List of Principals entity</returns>
        public Task<List<Principals>> ReadUserPrincipals(string user, CancellationToken ctk = default)
        {
            var url = "/user/principals"
                        .SetQueryParam("user", $"{user}");

            return _client.Exec<List<Principals>>(HttpMethod.Get, url, ctk);
        }
    }
}
