// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;

using Flurl;

using NodaTime;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Creates a new assignment binding a Market Data entity to a Data Quality Rule.
        /// The assignment defines which rule validates which market data.
        /// </summary>
        /// <param name="entity">The assignment definition including MarketDataId and DataQualityRuleId.</param>
        /// <param name="initializationLookbackPeriod">Optional ISO 8601 period (e.g. "P30D") defining how far back in time the rule should validate data on initial assignment. Not persisted.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created MarketDataQualityRuleAssignmentDto.Output with server-assigned Id.</returns>
        public Task<MarketDataQualityRuleAssignmentDto.Output> RegisterDataQualityRuleAssignmentAsync(MarketDataQualityRuleAssignmentDto.Input entity, Period? initializationLookbackPeriod = null, CancellationToken ctk = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var url = "/dataquality/dqruleassignment"
                    .SetQueryParam("initializationLookbackPeriod", initializationLookbackPeriod);

            return _client.Exec<MarketDataQualityRuleAssignmentDto.Output, MarketDataQualityRuleAssignmentDto.Input>(HttpMethod.Post, url, entity, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a DQ rule assignment by its unique identifier, including enriched MarketData and Rule data.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The MarketDataQualityRuleAssignmentDto.Output if found; otherwise 404 Not Found.</returns>
        public Task<MarketDataQualityRuleAssignmentDto.Output> ReadDataQualityRuleAssignmentByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqruleassignment".AppendPathSegment(id);

            return _client.Exec<MarketDataQualityRuleAssignmentDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of DQ rule assignments, optionally filtered by MarketData, Rule, or rule name.
        /// </summary>
        /// <param name="marketDataId">Optional filter: returns assignments for the specified Market Data.</param>
        /// <param name="ruleId">Optional filter: returns assignments for the specified Data Quality Rule.</param>
        /// <param name="ruleName">Optional partial match filter on rule name.</param>
        /// <param name="sort">Optional sort expressions (e.g., "Id asc", "RuleName desc").</param>
        /// <param name="page">The page number (1-based, default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated result containing MarketDataQualityRuleAssignmentDto.Output items.</returns>
        public Task<PagedResult<MarketDataQualityRuleAssignmentDto.Output>> ReadDataQualityRuleAssignmentAsync(int page,
                                                                                                                 int pageSize,
                                                                                                                 int? marketDataId = null,
                                                                                                                 int? ruleId = null,
                                                                                                                 string? ruleName = null,
                                                                                                                 string[]? sort = null,
                                                                                                                 CancellationToken ctk = default)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than 0. Page Size:" + pageSize, nameof(pageSize));

            var url = "/dataquality/dqruleassignment"
                    .SetQueryParam("page", page)
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("marketDataId", marketDataId)
                    .SetQueryParam("ruleId", ruleId)
                    .SetQueryParam("ruleName", ruleName);

            if (sort is { Length: > 0 })
                url = url.SetQueryParam("sort", sort);

            return _client.Exec<PagedResult<MarketDataQualityRuleAssignmentDto.Output>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Updates an assignment's initialization lookback, triggering re-evaluation from the new lookback date.
        /// All existing check results for this assignment are deleted and re-computed.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment to update.</param>
        /// <param name="initializationLookbackPeriod">ISO 8601 period (e.g. "P30D") defining the new lookback window.</param>
        /// <param name="etag">The current ETag for optimistic concurrency control.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated MarketDataQualityRuleAssignmentDto.Output.</returns>
        public Task<MarketDataQualityRuleAssignmentDto.Output> UpdateDataQualityRuleAssignmentAsync(int id, Period initializationLookbackPeriod, string etag, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqruleassignment"
                    .AppendPathSegment(id)
                    .SetQueryParam("initializationLookbackPeriod", initializationLookbackPeriod)
                    .SetQueryParam("etag", etag);

            return _client.Exec<MarketDataQualityRuleAssignmentDto.Output>(HttpMethod.Put, url, ctk: ctk);
        }

        /// <summary>
        /// Deletes an assignment, removing the binding between a Market Data entity and a Data Quality Rule.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        public Task DeleteDataQualityRuleAssignmentAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqruleassignment".AppendPathSegment(id);

            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves the raw event feed for a specific rule assignment.
        /// Returns events after the given timestamp (max 8-day lookback).
        /// </summary>
        /// <param name="id">The rule assignment identifier.</param>
        /// <param name="afterTimestamp">Optional lower bound (events after this instant).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of DqCheckChangeEventDto.Output.</returns>
        public Task<DqCheckChangeEventDto.Output[]> ReadDataQualityRuleAssignmentEventsFeedAsync(int id, Instant? afterTimestamp = null, CancellationToken ctk = default)
        {
            var url = "/dataquality/dqruleassignment"
                    .AppendPathSegment(id)
                    .AppendPathSegment("events")
                    .SetQueryParam("afterTimestamp", afterTimestamp);

            return _client.Exec<DqCheckChangeEventDto.Output[]>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
