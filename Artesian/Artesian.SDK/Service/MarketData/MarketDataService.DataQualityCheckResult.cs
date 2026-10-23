// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;

using Flurl;

using NodaTime;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        private const int _maxDataQualityAclCandidates = 100;

        /// <summary>
        /// Extracts data quality check results for versioned time series (VTS).
        /// Returns compact, abbreviated DTOs designed for high-volume extraction.
        /// </summary>
        /// <param name="version">Version timestamp (format yyyy-MM-ddTHH:mm:ss).</param>
        /// <param name="granularity">Time granularity (Day, Hour, etc.).</param>
        /// <param name="start">Range start (format yyyy-MM-dd).</param>
        /// <param name="end">Range end (format yyyy-MM-dd, not inclusive).</param>
        /// <param name="timeZone">IANA timezone identifier.</param>
        /// <param name="assignmentIds">Filter by assignment IDs.</param>
        /// <param name="includeMarketData">When true, includes provider and curve names when accessible.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="CheckResultExtract.Vts"/> with Version populated.</returns>
        public Task<IEnumerable<CheckResultExtract.Vts>> GetDataQualityCheckResultExtractVtsAsync(
            LocalDateTime version,
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);
            _validateAssignmentIds(assignmentIds);

            var url = $"/dataquality/checkresult/extract/vts/Version/{version:yyyy-MM-ddTHH:mm:ss}/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("timeZone", timeZone)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (assignmentIds != null && assignmentIds.Length > 0)
            {
                url = url.SetQueryParam("assignmentIds", assignmentIds);
            }

            return _client.Exec<IEnumerable<CheckResultExtract.Vts>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Extracts data quality check results for (non-versioned) time series (TS).
        /// Returns compact DTOs without version information.
        /// </summary>
        /// <param name="granularity">Time granularity (Day, Hour, etc.).</param>
        /// <param name="start">Range start (format yyyy-MM-dd).</param>
        /// <param name="end">Range end (format yyyy-MM-dd, not inclusive).</param>
        /// <param name="timeZone">IANA timezone identifier.</param>
        /// <param name="assignmentIds">Filter by assignment IDs.</param>
        /// <param name="includeMarketData">When true, includes provider and curve names when accessible.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="CheckResultExtract.Ts"/>.</returns>
        public Task<IEnumerable<CheckResultExtract.Ts>> GetDataQualityCheckResultExtractTsAsync(
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);
            _validateAssignmentIds(assignmentIds);

            var url = $"/dataquality/checkresult/extract/ts/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("timeZone", timeZone)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (assignmentIds != null && assignmentIds.Length > 0)
            {
                url = url.SetQueryParam("assignmentIds", assignmentIds);
            }

            return _client.Exec<IEnumerable<CheckResultExtract.Ts>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Extracts data quality check results for Market Assessment series.
        /// </summary>
        /// <param name="granularity">Time granularity.</param>
        /// <param name="start">Range start, inclusive.</param>
        /// <param name="end">Range end, not inclusive.</param>
        /// <param name="timeZone">IANA timezone identifier.</param>
        /// <param name="assignmentIds">Optional assignment ID filter.</param>
        /// <param name="products">Optional Market Assessment product filter.</param>
        /// <param name="includeMarketData">When true, includes provider and curve names when accessible.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An enumerable of <see cref="CheckResultExtract.Mas"/>.</returns>
        public Task<IEnumerable<CheckResultExtract.Mas>> GetDataQualityCheckResultExtractMasAsync(
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            string[]? products = null,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);
            _validateAssignmentIds(assignmentIds);

            var url = $"/dataquality/checkresult/extract/mas/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("tz", timeZone)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (assignmentIds != null && assignmentIds.Length > 0)
            {
                url = url.SetQueryParam("id", assignmentIds);
            }

            if (products != null && products.Length > 0)
            {
                url = url.SetQueryParam("p", products);
            }

            return _client.Exec<IEnumerable<CheckResultExtract.Mas>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Extracts data quality check results for BidAsk series.
        /// </summary>
        /// <param name="granularity">Time granularity.</param>
        /// <param name="start">Range start, inclusive.</param>
        /// <param name="end">Range end, not inclusive.</param>
        /// <param name="timeZone">IANA timezone identifier.</param>
        /// <param name="assignmentIds">Optional assignment ID filter.</param>
        /// <param name="products">Optional BidAsk product filter.</param>
        /// <param name="includeMarketData">When true, includes provider and curve names when accessible.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An enumerable of <see cref="CheckResultExtract.BidAsk"/>.</returns>
        public Task<IEnumerable<CheckResultExtract.BidAsk>> GetDataQualityCheckResultExtractBidAskAsync(
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            string[]? products = null,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);
            _validateAssignmentIds(assignmentIds);

            var url = $"/dataquality/checkresult/extract/ba/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("tz", timeZone)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (assignmentIds != null && assignmentIds.Length > 0)
            {
                url = url.SetQueryParam("id", assignmentIds);
            }

            if (products != null && products.Length > 0)
            {
                url = url.SetQueryParam("p", products);
            }

            return _client.Exec<IEnumerable<CheckResultExtract.BidAsk>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paged summary of data quality check results (CurveRange-like view per assignment).
        /// Includes range metadata, product info, and assignment details.
        /// </summary>
        /// <param name="marketDataIds">Filter by MarketData IDs.</param>
        /// <param name="ruleIds">Filter by Rule IDs.</param>
        /// <param name="assignmentIds">Filter by Assignment IDs.</param>
        /// <param name="dqStatus">Filter by aggregated DQ status (OK/KO).</param>
        /// <param name="from">Range start filter.</param>
        /// <param name="to">Range end filter.</param>
        /// <param name="versionFrom">Version range start.</param>
        /// <param name="versionTo">Version range end.</param>
        /// <param name="products">Filter by products.</param>
        /// <param name="skipEmptyRanges">When true, return only summaries with non-empty range data (default: false).</param>
        /// <param name="sort">Optional sort expressions.</param>
        /// <param name="page">Page number (1-based, default: 1).</param>
        /// <param name="pageSize">Items per page (1-100, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <param name="includeMarketData">When true, includes Market Data details the caller can read.</param>
        /// <returns>A paginated result containing <see cref="CheckResultCheckSummaryDto"/> items.</returns>
        public Task<PagedResult<CheckResultCheckSummaryDto>> GetDataQualityCheckResultCheckSummaryAsync(
            int page,
            int pageSize,
            int[]? marketDataIds = null,
            int[]? ruleIds = null,
            int[]? assignmentIds = null,
            CheckAggregatedStatus? dqStatus = null,
            Instant? from = null,
            Instant? to = null,
            LocalDateTime? versionFrom = null,
            LocalDateTime? versionTo = null,
            string[]? products = null,
            bool skipEmptyRanges = false,
            string[]? sort = null,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1 || pageSize > _maxDataQualityAclCandidates)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, $"PageSize must be between 1 and {_maxDataQualityAclCandidates}.");

            var url = "/dataquality/checkresult/checksummary"
                .SetQueryParam("page", page)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("skipEmptyRanges", skipEmptyRanges)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (marketDataIds != null && marketDataIds.Length > 0)
                url = url.SetQueryParam("marketDataIds", marketDataIds);

            if (ruleIds != null && ruleIds.Length > 0)
                url = url.SetQueryParam("ruleIds", ruleIds);

            if (assignmentIds != null && assignmentIds.Length > 0)
                url = url.SetQueryParam("assignmentIds", assignmentIds);

            if (dqStatus.HasValue)
                url = url.SetQueryParam("dqStatus", dqStatus.Value);

            if (from.HasValue)
                url = url.SetQueryParam("from", from.Value);

            if (to.HasValue)
                url = url.SetQueryParam("to", to.Value);

            if (versionFrom.HasValue)
                url = url.SetQueryParam("versionFrom", versionFrom.Value);

            if (versionTo.HasValue)
                url = url.SetQueryParam("versionTo", versionTo.Value);

            if (products != null && products.Length > 0)
                url = url.SetQueryParam("products", products);

            if (sort != null && sort.Length > 0)
                url = url.SetQueryParam("sort", sort);

            return _client.Exec<PagedResult<CheckResultCheckSummaryDto>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves market data entities with their DQ status summary for a given rule.
        /// Results are sorted by LastCheckTime descending.
        /// </summary>
        /// <param name="ruleId">Optional Rule ID filter.</param>
        /// <param name="marketDataIds">Optional filter by MarketData IDs.</param>
        /// <param name="dqStatus">Optional aggregated DQ status filter (OK/KO). When KO, returns only Market Data whose overall status is KO.</param>
        /// <param name="limit">Maximum number of results to return (1..100, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <param name="includeMarketData">When true, includes Market Data details the caller can read.</param>
        /// <returns>An array of <see cref="MarketDataDqStatusSummaryDto"/> items.</returns>
        public Task<IEnumerable<MarketDataDqStatusSummaryDto>> GetMarketDataDqStatusSummaryAsync(
            int? ruleId = null,
            int[]? marketDataIds = null,
            CheckAggregatedStatus? dqStatus = null,
            int limit = 10,
            bool includeMarketData = false,
            CancellationToken ctk = default)
        {
            if (limit < 1 || limit > _maxDataQualityAclCandidates)
                throw new ArgumentOutOfRangeException(nameof(limit), limit, $"Limit must be between 1 and {_maxDataQualityAclCandidates}.");

            var url = "/dataquality/checkresult/marketdata/dataqualitystatussummary"
                .SetQueryParam("limit", limit)
                .SetQueryParam("includeMarketData", includeMarketData);

            if (ruleId.HasValue)
                url = url.SetQueryParam("ruleId", ruleId.Value);

            if (marketDataIds != null && marketDataIds.Length > 0)
                url = url.SetQueryParam("marketDataIds", marketDataIds);

            if (dqStatus.HasValue)
                url = url.SetQueryParam("dqStatus", dqStatus.Value);

            return _client.Exec<IEnumerable<MarketDataDqStatusSummaryDto>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves DQ rules with their status summary, optionally filtered by a specific market data entity.
        /// Results are sorted by LastCheckTime descending.
        /// </summary>
        /// <param name="marketDataId">Optional filter by a specific MarketData ID.</param>
        /// <param name="ruleIds">Optional filter by specific Rule IDs.</param>
        /// <param name="dqStatus">Optional aggregated DQ status filter (OK/KO). When KO, returns only rules whose overall status is KO.</param>
        /// <param name="limit">Maximum number of results to return (1..100, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqRuleDqStatusSummaryDto"/> items.</returns>
        public Task<IEnumerable<DqRuleDqStatusSummaryDto>> GetDqRuleDqStatusSummaryAsync(
            int? marketDataId = null,
            int[]? ruleIds = null,
            CheckAggregatedStatus? dqStatus = null,
            int limit = 10,
            CancellationToken ctk = default)
        {
            if (limit < 1 || limit > _maxDataQualityAclCandidates)
                throw new ArgumentOutOfRangeException(nameof(limit), limit, $"Limit must be between 1 and {_maxDataQualityAclCandidates}.");

            var url = "/dataquality/checkresult/dqrule/dataqualitystatussummary"
                .SetQueryParam("limit", limit);

            if (marketDataId.HasValue)
                url = url.SetQueryParam("marketDataId", marketDataId.Value);

            if (ruleIds != null && ruleIds.Length > 0)
                url = url.SetQueryParam("ruleIds", ruleIds);

            if (dqStatus.HasValue)
                url = url.SetQueryParam("dqStatus", dqStatus.Value);

            return _client.Exec<IEnumerable<DqRuleDqStatusSummaryDto>>(HttpMethod.Get, url, ctk: ctk);
        }

        private static void _validateAssignmentIds(int[]? assignmentIds)
        {
            if (assignmentIds is { Length: > _maxDataQualityAclCandidates })
                throw new ArgumentOutOfRangeException(nameof(assignmentIds), assignmentIds.Length, $"No more than {_maxDataQualityAclCandidates} assignment IDs are allowed.");
        }
    }
}
