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
        /// Creates a new alert rule definition for data quality monitoring.
        /// The alert rule defines the trigger mode (on-event or scheduled)
        /// and the notification channels (email, Teams, Slack, webhook, principal).
        /// Market Data assignments are managed separately via the alertruleassignment endpoint.
        /// </summary>
        /// <param name="entity">The alert rule definition including trigger config and notification channels.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="QualityNotificationAlertDto.Output"/> with server-assigned Id.</returns>
        public Task<QualityNotificationAlertDto.Output> RegisterQualityNotificationAlertAsync(QualityNotificationAlertDto.Input entity, CancellationToken ctk = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return _client.Exec<QualityNotificationAlertDto.Output, QualityNotificationAlertDto.Input>(
                HttpMethod.Post, "/dataquality/alertrule", entity, ctk: ctk);
        }

        /// <summary>
        /// Retrieves an alert rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="QualityNotificationAlertDto.Output"/> if found; otherwise 404 Not Found.</returns>
        public Task<QualityNotificationAlertDto.Output> ReadQualityNotificationAlertByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule".AppendPathSegment(id);
            return _client.Exec<QualityNotificationAlertDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of alert rules, optionally filtered by name, assigned market data, rule IDs; and sorted.
        /// </summary>
        /// <param name="name">Optional filter by alert rule name (partial match).</param>
        /// <param name="marketDataId">Optional filter: returns alerts assigned to this MarketData.</param>
        /// <param name="ruleIds">Optional filter by specific rule IDs (via assignment chain).</param>
        /// <param name="sort">Optional sort expressions (e.g., "Name asc").</param>
        /// <param name="page">The page number (1-based, default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated result containing <see cref="QualityNotificationAlertDto.Output"/> items.</returns>
        public Task<PagedResult<QualityNotificationAlertDto.Output>> ReadQualityNotificationAlertsAsync(
            int page,
            int pageSize,
            string? name = null,
            int? marketDataId = null,
            int[]? ruleIds = null,
            string[]? sort = null,
            CancellationToken ctk = default)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than 0. PageSize:" + pageSize, nameof(pageSize));

            var url = "/dataquality/alertrule"
                .SetQueryParam("name", name)
                .SetQueryParam("marketDataId", marketDataId)
                .SetQueryParam("page", page)
                .SetQueryParam("pageSize", pageSize);

            if (ruleIds is { Length: > 0 })
                url = url.SetQueryParam("ruleIds", ruleIds);
            if (sort is { Length: > 0 })
                url = url.SetQueryParam("sort", sort);

            return _client.Exec<PagedResult<QualityNotificationAlertDto.Output>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Updates an existing alert rule. Modifies the trigger configuration or notification channels.
        /// Market Data assignments are managed separately via the alertruleassignment endpoint.
        /// Uses optimistic concurrency via the ETag property.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule to update.</param>
        /// <param name="entity">The updated alert rule definition.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated <see cref="QualityNotificationAlertDto.Output"/>.</returns>
        public Task<QualityNotificationAlertDto.Output> UpdateQualityNotificationAlertAsync(int id, QualityNotificationAlertDto.Input entity, CancellationToken ctk = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var url = "/dataquality/alertrule".AppendPathSegment(id);
            return _client.Exec<QualityNotificationAlertDto.Output, QualityNotificationAlertDto.Input>(
                HttpMethod.Put, url, entity, ctk: ctk);
        }

        /// <summary>
        /// Deletes an alert rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        public Task DeleteQualityNotificationAlertAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule".AppendPathSegment(id);
            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves the materialized events for a specific alert schedule occurrence.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="scheduleTime">The schedule occurrence timestamp.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqCheckChangeEventDto.Output"/>.</returns>
        public Task<AlertScheduleEventsDto.Output> ReadAlertScheduleEventsAsync(int alertId, Instant scheduleTime, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule"
                .AppendPathSegment(alertId)
                .AppendPathSegment("schedule")
                .AppendPathSegment(scheduleTime)
                .AppendPathSegment("events");

            return _client.Exec<AlertScheduleEventsDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Lists the most recent schedule occurrence timestamps for an alert.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="lastN">Number of most recent occurrences to return (default 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="Instant"/> timestamps ordered descending.</returns>
        public Task<Instant[]> ReadAlertScheduleListAsync(int alertId, int lastN = 10, CancellationToken ctk = default)
        {
            if (lastN < 1)
                throw new ArgumentException("LastN must be greater than 0. LastN:" + lastN, nameof(lastN));

            var url = "/dataquality/alertrule"
                .AppendPathSegment(alertId)
                .AppendPathSegment("schedule")
                .SetQueryParam("lastN", lastN);

            return _client.Exec<Instant[]>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves the materialized events from the latest schedule occurrence for an alert.
        /// Returns an empty array if no schedule has been materialized yet.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqCheckChangeEventDto.Output"/> from the latest schedule.</returns>
        public Task<AlertScheduleEventsDto.Output> ReadAlertScheduleLastEventsAsync(int alertId, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule"
                .AppendPathSegment(alertId)
                .AppendPathSegment("schedule")
                .AppendPathSegment("latest")
                .AppendPathSegment("events");

            return _client.Exec<AlertScheduleEventsDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
