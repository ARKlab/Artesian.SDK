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
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Metadata service
    /// </summary>
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Retrieve the ACL Path Roles by path
        /// </summary>
        /// <param name="path">The path (starting with "/" char. Ex. "/marketdata/system/" identifies folder "marketdata" with a subfolder "system", roles are assigned to "system" subfolder. Ex. "/marketdata/genoacurve" identifies folder "marketdata" with entity "genoacurve", roles are assigned to "genoacurve" entity.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of AuthorizationPath Output entity</returns>
        public Task<IEnumerable<AuthorizationPath.Output>> ReadRolesByPath(PathString path, CancellationToken ctk = default)
        {
            var url = "/acl/me".AppendPathSegment(path);

            return _client.Exec<IEnumerable<AuthorizationPath.Output>>(HttpMethod.Get, url, ctk);
        }

        /// <summary>
        /// Retrieve the ACL Path Roles paged
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="principalIds">The principal ids I want to inspect, encoded.( ex. u:user@example.com for users and clients,g:1001 for groups)</param>
        /// <param name="asOf">LocalDateTime we want to inspect</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>AclPath entity</returns>
        public Task<PagedResult<AclPath>> GetRoles(int page, int pageSize, string[] principalIds, LocalDateTime? asOf = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/acl"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page)
                    .SetQueryParam("principalIds", principalIds)
                    .SetQueryParam("asOf", asOf)
                    ;

            return _client.Exec<PagedResult<AclPath>>(HttpMethod.Get, url, ctk);
        }

        /// <summary>
        /// Upsert the ACL Path Roles
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="upsert">The entity we want to upsert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task UpsertRoles(AuthorizationPath.Input upsert, CancellationToken ctk = default)
        {
            var url = "/acl";

            return _client.Exec<AuthorizationPath.Input>(HttpMethod.Post, url, upsert, ctk);
        }

        /// <summary>
        /// Add a role to the ACL Path
        /// </summary>
        /// <param name="add">The entity we want to add. At the path add.Path we add the add.Roles</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task AddRoles(AuthorizationPath.Input add, CancellationToken ctk = default)
        {
            var url = "/acl/roles";

            return _client.Exec<AuthorizationPath.Input>(HttpMethod.Post, url, add, ctk);
        }

        /// <summary>
        /// Remove a role from the ACL Path
        /// </summary>
        /// <param name="remove">The entity we want to remove. At the path remove.Path we remove the remove.Roles</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task RemoveRoles(AuthorizationPath.Input remove, CancellationToken ctk = default)
        {
            var url = "/acl/roles";

            return _client.Exec<AuthorizationPath.Input>(HttpMethod.Delete, url, remove, ctk);
        }
    }
}
