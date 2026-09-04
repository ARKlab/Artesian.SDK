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
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="CheckResultExtract.Vts"/> with Version populated.</returns>
        public Task<IEnumerable<CheckResultExtract.Vts>> GetDataQualityCheckResultExtractVtsAsync(
            LocalDateTime version,
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);

            var url = $"/dataquality/checkresult/extract/vts/Version/{version:yyyy-MM-ddTHH:mm:ss}/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("timeZone", timeZone);

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
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="CheckResultExtract.Ts"/>.</returns>
        public Task<IEnumerable<CheckResultExtract.Ts>> GetDataQualityCheckResultExtractTsAsync(
            Granularity granularity,
            LocalDate start,
            LocalDate end,
            string timeZone,
            int[]? assignmentIds = null,
            CancellationToken ctk = default)
        {
            Guard.IsNotNullOrWhiteSpace(timeZone);

            var url = $"/dataquality/checkresult/extract/ts/{granularity}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}"
                .SetQueryParam("timeZone", timeZone);

            if (assignmentIds != null && assignmentIds.Length > 0)
            {
                url = url.SetQueryParam("assignmentIds", assignmentIds);
            }

            return _client.Exec<IEnumerable<CheckResultExtract.Ts>>(HttpMethod.Get, url, ctk: ctk);
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
        /// <param name="pageSize">Items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
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
            CancellationToken ctk = default)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than 0. PageSize:" + pageSize, nameof(pageSize));

            var url = "/dataquality/checkresult/checksummary"
                .SetQueryParam("page", page)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("skipEmptyRanges", skipEmptyRanges);

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
        /// <param name="limit">Maximum number of results to return (1..1000, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="MarketDataDqStatusSummaryDto"/> items.</returns>
        public Task<IEnumerable<MarketDataDqStatusSummaryDto>> GetMarketDataDqStatusSummaryAsync(
            int? ruleId = null,
            int[]? marketDataIds = null,
            CheckAggregatedStatus? dqStatus = null,
            int limit = 10,
            CancellationToken ctk = default)
        {
            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000. Limit:" + limit, nameof(limit));

            var url = "/dataquality/checkresult/marketdata/dataqualitystatussummary"
                .SetQueryParam("limit", limit);

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
        /// <param name="limit">Maximum number of results to return (1..1000, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqRuleDqStatusSummaryDto"/> items.</returns>
        public Task<IEnumerable<DqRuleDqStatusSummaryDto>> GetDqRuleDqStatusSummaryAsync(
            int? marketDataId = null,
            int[]? ruleIds = null,
            CheckAggregatedStatus? dqStatus = null,
            int limit = 10,
            CancellationToken ctk = default)
        {
            if (limit < 1 || limit > 1000)
                throw new ArgumentException("Limit must be between 1 and 1000. Limit:" + limit, nameof(limit));

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
    }
}
