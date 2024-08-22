// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Artesian.SDK.Service
{
    [Obsolete("Use IMarketDataService", true)]
    public partial class MetadataService : IMetadataService
    {
        private readonly IArtesianServiceConfig _cfg;
        private readonly ArtesianPolicyConfig _policy;
        private static Client _client;
        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public MetadataService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public MetadataService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.MetadataVersion, _policy);
        }

        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(MarketDataIdentifier id, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(int id, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<CurveRange>> ReadCurveRangeAsync(int id, int page, int pageSize, string product = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketDataEntity.Output> RegisterMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<MarketDataEntity.Output> UpdateMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMarketDataAsync(int id, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<MarketDataEntity.Output>> PerformOperationsAsync(Operations operations, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<TimeTransform> ReadTimeTransformBaseAsync(int timeTransformId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TimeTransform>> ReadTimeTransformsAsync(int page, int pageSize, bool userDefined, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<TimeTransform> RegisterTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<TimeTransform> UpdateTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTimeTransformSimpleShiftAsync(int timeTransformId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<CustomFilter> CreateFilter(CustomFilter filter, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<CustomFilter> UpdateFilter(int filterId, CustomFilter filter, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<CustomFilter> ReadFilter(int filterId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<CustomFilter> RemoveFilter(int filterId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<CustomFilter>> ReadFilters(int page, int pageSize, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuthorizationPath.Output>> ReadRolesByPath(PathString path, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<AclPath>> GetRoles(int page, int pageSize, string[] principalIds, LocalDateTime? asOf = null, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task UpsertRoles(AuthorizationPath.Input upsert, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task AddRoles(AuthorizationPath.Input add, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRoles(AuthorizationPath.Input remove, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<AuthGroup> CreateAuthGroup(AuthGroup group, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<AuthGroup> UpdateAuthGroup(int groupID, AuthGroup group, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAuthGroup(int groupID, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<AuthGroup> ReadAuthGroup(int groupID, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<AuthGroup>> ReadAuthGroups(int page, int pageSize, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Principals>> ReadUserPrincipals(string user, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task UpsertCurveDataAsync(UpsertCurveData data, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKey.Output> CreateApiKeyAsync(ApiKey.Input apiKeyRecord, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKey.Output> ReadApiKeyByKeyAsync(string key, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKey.Output> ReadApiKeyByIdAsync(int id, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<ApiKey.Output>> ReadApiKeysAsync(int page, int pageSize, string userId, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteApiKeyAsync(int id, CancellationToken ctk = default)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore 1591
}
