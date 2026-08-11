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
        /// </summary>
        /// <param name="entity">The alert rule definition including trigger configuration and notification channels.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created alert rule with its server-assigned identifier.</returns>
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
        /// <returns>The alert rule if found; otherwise <see langword="null"/>.</returns>
        public Task<QualityNotificationAlertDto.Output> ReadQualityNotificationAlertByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule".AppendPathSegment(id);
            return _client.Exec<QualityNotificationAlertDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of alert rules with optional filters and sorting.
        /// </summary>
        /// <param name="page">The page number, starting at 1.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="name">Optional partial name filter.</param>
        /// <param name="marketDataId">Optional Market Data assignment filter.</param>
        /// <param name="ruleIds">Optional alert rule identifier filter.</param>
        /// <param name="sort">Optional sort expressions, such as <c>Name asc</c>.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated collection of alert rules.</returns>
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
        /// Updates an existing alert rule using optimistic concurrency through its ETag.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule.</param>
        /// <param name="entity">The updated alert rule definition.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated alert rule.</returns>
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
        /// <param name="id">The unique identifier of the alert rule.</param>
        /// <param name="ctk">Cancellation token.</param>
        public Task DeleteQualityNotificationAlertAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertrule".AppendPathSegment(id);
            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves the materialized events for a specific alert schedule occurrence.
        /// </summary>
        /// <param name="alertId">The alert rule identifier.</param>
        /// <param name="scheduleTime">The schedule occurrence timestamp.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The schedule occurrence and its materialized data quality events.</returns>
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
        /// Lists the most recent schedule occurrence timestamps for an alert rule.
        /// </summary>
        /// <param name="alertId">The alert rule identifier.</param>
        /// <param name="lastN">The number of recent occurrences to return.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>Recent schedule timestamps ordered descending.</returns>
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
        /// Retrieves the materialized events from the latest schedule occurrence.
        /// </summary>
        /// <param name="alertId">The alert rule identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The latest schedule occurrence and its events, or an empty event set when none exists.</returns>
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
