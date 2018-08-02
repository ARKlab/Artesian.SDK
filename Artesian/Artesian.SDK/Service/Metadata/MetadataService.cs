﻿// Copyright (c) ARK LTD. All rights reserved.
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

namespace Artesian.SDK.Service
{
    public class MetadataService : IMetadataService
    {
        private IArtesianServiceConfig _cfg;
        private static Client _client;

        public MetadataService(IArtesianServiceConfig cfg)
        {
            _cfg = cfg;
            _client = new Client(cfg, cfg.BaseAddress.ToString().AppendPathSegment(ArtesianConstants.MetadataVersion)
            );
        }

        public Task<TimeTransform> ReadTimeTransformBaseAsync(int timeTransformId, CancellationToken ctk = default)
        {
            if (timeTransformId < 1)
                throw new ArgumentException("Transform id is invalid : " + timeTransformId);

            return _client.Exec<TimeTransform>(HttpMethod.Get, $@"/timeTransform/entity/{timeTransformId}", ctk: ctk);
        }

        public Task<PagedResult<TimeTransform>> ReadTimeTransformsAsync(int page, int pageSize, bool userDefined, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/timeTransform/entity"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page)
                    .SetQueryParam("userDefined", userDefined)
                    ;

            return _client.Exec<PagedResult<TimeTransform>>(HttpMethod.Get, url.ToString(), ctk: ctk);
        }

        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(MarketDataIdentifier id, CancellationToken ctk = default)
        {
            id.Validate();
            var url = "/marketdata/entity"
                    .SetQueryParam("provider", id.Provider)
                    .SetQueryParam("curveName", id.Name)
                    ;
            return _client.Exec<MarketDataEntity.Output>(HttpMethod.Get, url.ToString(), ctk: ctk);
        }

        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(int id, CancellationToken ctk = default)
        {
            if (id < 1)
                throw new ArgumentException("Id invalid :" + id);

            var url = "/marketdata/entity/".AppendPathSegment(id.ToString());
            return _client.Exec<MarketDataEntity.Output>(HttpMethod.Get, url.ToString(), ctk: ctk);
        }

        public Task<PagedResult<CurveRange>> ReadCurveRangeAsync(int id, int page, int pageSize, string product = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, CancellationToken ctk = default)
        {

            var url = "/marketdata/entity/".AppendPathSegment(id.ToString()).AppendPathSegment("curves")
                     .SetQueryParam("versionFrom", versionFrom)
                     .SetQueryParam("versionTo", versionTo)
                     .SetQueryParam("product", product)
                     .SetQueryParam("page", page)
                     .SetQueryParam("pageSize", pageSize)
                     ;

            return _client.Exec<PagedResult<CurveRange>>(HttpMethod.Get, url, ctk: ctk);
        }

        public Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, CancellationToken ctk = default)
        {
            filter.Validate();

            var url = "/marketdata/searchfacet"
                    .SetQueryParam("pageSize", filter.PageSize)
                    .SetQueryParam("page", filter.Page)
                    .SetQueryParam("searchText", filter.SearchText)
                    .SetQueryParam("filters", filter.Filters?.SelectMany(s => s.Value.Select(x => $@"{s.Key}:{x}")))
                    .SetQueryParam("sorts", filter.Sorts)
                    ;

            return _client.Exec<ArtesianSearchResults>(HttpMethod.Get, url.ToString(), ctk: ctk);
        }
    }
}
