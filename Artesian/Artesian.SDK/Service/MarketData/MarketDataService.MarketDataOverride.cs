// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.Override.Enum;

using Flurl;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Writes (upserts) an override or fallback for a portion of a Market Data. Reuses the standard
        /// upsert-data payload extended with override metadata; the values are stored separately and described by an <see cref="OverrideMetadataEntry"/>.
        /// </summary>
        /// <param name="data">The curve data payload plus override configuration (kind, id, replace, comment).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created/updated <see cref="OverrideMetadataEntry"/> entries.</returns>
        public Task<IReadOnlyList<OverrideMetadataEntry>> UpsertCurveDataOverrideAsync(UpsertCurveDataOverride data, CancellationToken ctk = default)
        {
            data.Validate();

            var url = "/marketdata/override/upsertdata";

            return _marketDataOverrideClient.Exec<IReadOnlyList<OverrideMetadataEntry>, UpsertCurveDataOverride>(HttpMethod.Post, url, data, ctk: ctk);
        }

        /// <summary>
        /// Deletes the data and metadata associated with a specific override/fallback, identified by its metadata id.
        /// </summary>
        /// <param name="id">The override/fallback metadata id to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        public Task DeleteOverrideDataAsync(Guid id, CancellationToken ctk = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Override metadata id must be valorized", nameof(id));

            var url = "/marketdata/override/".AppendPathSegment(id).AppendPathSegment("deletedata");

            return _marketDataOverrideClient.Exec(HttpMethod.Post, url, ctk: ctk);
        }

        /// <summary>
        /// Lists the override/fallback metadata of a specific Market Data, paged and optionally filtered by kind.
        /// </summary>
        /// <param name="marketDataId">The Market Data whose override/fallback metadata is requested.</param>
        /// <param name="kind">Optional filter by override/fallback kind.</param>
        /// <param name="page">The page number (1-based, default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated list of <see cref="OverrideMetadataEntry"/>.</returns>
        public Task<PagedResult<OverrideMetadataEntry>> ReadOverrideMetadataAsync(int marketDataId, OverrideKind? kind = null, int page = 1, int pageSize = 10, CancellationToken ctk = default)
        {
            if (marketDataId < 1)
                throw new ArgumentException("Market Data id must be greater than zero", nameof(marketDataId));
            if (page < 1)
                throw new ArgumentException("Page must be greater than zero", nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than zero", nameof(pageSize));

            var url = "/marketdata/override/".AppendPathSegment(marketDataId).AppendPathSegment("metadata")
                .SetQueryParam("kind", kind)
                .SetQueryParam("page", page)
                .SetQueryParam("pageSize", pageSize);

            return _marketDataOverrideClient.Exec<PagedResult<OverrideMetadataEntry>>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
