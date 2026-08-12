using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;

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
        /// Creates a new assignment binding a Market Data entity to an alert rule.
        /// The assignment defines which market data is monitored by which alert rule.
        /// </summary>
        /// <param name="entity">The assignment definition including AlertId and MarketDataId.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="QualityNotificationAlertAssignmentDto.Output"/> with server-assigned Id.</returns>
        public Task<QualityNotificationAlertAssignmentDto.Output> RegisterQualityNotificationAlertAssignmentAsync(QualityNotificationAlertAssignmentDto.Input entity, CancellationToken ctk = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return _client.Exec<QualityNotificationAlertAssignmentDto.Output, QualityNotificationAlertAssignmentDto.Input>(
                HttpMethod.Post, "/dataquality/alertruleassignment", entity, ctk: ctk);
        }

        /// <summary>
        /// Retrieves an alert rule / Market Data assignment by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="QualityNotificationAlertAssignmentDto.Output"/> if found; otherwise 404 Not Found.</returns>
        public Task<QualityNotificationAlertAssignmentDto.Output> ReadQualityNotificationAlertAssignmentByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertruleassignment".AppendPathSegment(id);
            return _client.Exec<QualityNotificationAlertAssignmentDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of alert assignments, optionally filtered by alert rule or Market Data identifier.
        /// </summary>
        /// <param name="alertId">Optional filter: returns assignments for the specified alert rule.</param>
        /// <param name="marketDataId">Optional filter: returns assignments for the specified Market Data.</param>
        /// <param name="sort">Optional sort expressions (e.g., "Id asc", "AlertId desc").</param>
        /// <param name="page">The page number (1-based, default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated result containing <see cref="QualityNotificationAlertAssignmentDto.Output"/> items.</returns>
        public Task<PagedResult<QualityNotificationAlertAssignmentDto.Output>> ReadQualityNotificationAlertAssignmentsAsync(
            int page,
            int pageSize,
            int? alertId = null,
            int? marketDataId = null,
            string[]? sort = null,
            CancellationToken ctk = default)
        {
            if (page < 1)
                throw new ArgumentException("Page must be greater than 0. Page:" + page, nameof(page));
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than 0. PageSize:" + pageSize, nameof(pageSize));

            var url = "/dataquality/alertruleassignment"
                .SetQueryParam("alertId", alertId)
                .SetQueryParam("marketDataId", marketDataId)
                .SetQueryParam("page", page)
                .SetQueryParam("pageSize", pageSize);

            if (sort is { Length: > 0 })
                url = url.SetQueryParam("sort", sort);

            return _client.Exec<PagedResult<QualityNotificationAlertAssignmentDto.Output>>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Deletes an assignment, removing the binding between a Market Data entity and an alert rule.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        public Task DeleteQualityNotificationAlertAssignmentAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertruleassignment".AppendPathSegment(id);
            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }
    }
}
