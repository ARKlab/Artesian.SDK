// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;

using Flurl;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Creates a new Data Quality Rule with the specified configuration.
        /// The rule defines validation logic (completeness/freshness or outlier detection) that can be assigned to Market Data entities.
        /// </summary>
        /// <param name="entity">The rule definition including name, type, and configuration.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="DataQualityRuleDto.Output"/> with server-assigned Id and metadata.</returns>
        public Task<DataQualityRuleDto.Output> RegisterDataQualityRuleAsync(DataQualityRuleDto.Input entity, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqrule";

            return _client.Exec<DataQualityRuleDto.Output, DataQualityRuleDto.Input>(HttpMethod.Post, url, entity, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a Data Quality Rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rule.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="DataQualityRuleDto.Output"/> if found; otherwise 404 Not Found.</returns>
        public Task<DataQualityRuleDto.Output> ReadDataQualityRuleByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqrule".AppendPathSegment(id);

            return _client.Exec<DataQualityRuleDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of Data Quality Rules, optionally filtered by rule type, name, and sorted.
        /// </summary>
        /// <param name="type">Optional filter by rule type (CompletenessAndFreshness or Outlier).</param>
        /// <param name="marketDataId">Optional filter: returns rules assigned to this MarketData.</param>
        /// <param name="name">Optional partial match filter on rule name.</param>
        /// <param name="ruleIds">Optional filter by specific rule IDs.</param>
        /// <param name="sort">Optional sort expressions (e.g., "Name asc").</param>
        /// <param name="page">The page number (1-based, default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated result containing <see cref="DataQualityRuleDto.Output"/> items.</returns>
        public Task<PagedResult<DataQualityRuleDto.Output>> ReadDataQualityRuleAsync(int page, 
                                                                         int pageSize, 
                                                                         RuleType? type = null,
                                                                         int? marketDataId = null, 
                                                                         string? name = null, 
                                                                         int[]? ruleIds = null, 
                                                                         string[]? sort = null, 
                                                                         CancellationToken ctk = default)
        {
            ruleIds ??= Array.Empty<int>();
            sort ??= Array.Empty<string>();

            if (page < 1)
                throw new ArgumentException("Page must to be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must to be greater than 0. Page Size:" + pageSize, nameof(pageSize));

            var url = "/dataquality/dqrule"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page)
                    .SetQueryParam("type", type)
                    .SetQueryParam("marketDataId", marketDataId)
                    .SetQueryParam("name", name)
                    .SetQueryParam("ruleIds", ruleIds)
                    .SetQueryParam("sort", sort)
                    ;

            return _client.Exec<PagedResult<DataQualityRuleDto.Output>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Updates an existing Data Quality Rule. The rule's configuration, name, and type can be modified.
        /// Uses optimistic concurrency via the ETag property.
        /// </summary>
        /// <param name="id">The unique identifier of the rule to update.</param>
        /// <param name="entity">The updated rule definition.</param>
        /// <param name="ctk">Cancellation token.</param>
        public Task<DataQualityRuleDto.Output> UpdateDataQualityRuleAsync(int id, DataQualityRuleDto.Input entity, CancellationToken ctk = default)
        {
            entity?.Validate();

            var url = "/dataquality/dqrule".AppendPathSegment(id);

            return _client.Exec<DataQualityRuleDto.Output, DataQualityRuleDto.Input>(HttpMethod.Put, url, entity, ctk: ctk);
        }

        /// <summary>
        /// Deletes a Data Quality Rule by its unique identifier.
        /// Existing assignments referencing this rule should be removed first.
        /// </summary>
        /// <param name="id">The unique identifier of the rule to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        public Task DeleteDataQualityRuleAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqrule".AppendPathSegment(id);

            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }
    }
}
