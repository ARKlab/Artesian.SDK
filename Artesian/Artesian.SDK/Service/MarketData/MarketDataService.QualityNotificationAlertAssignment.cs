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
        /// Creates an assignment binding a Market Data entity to a quality notification alert.
        /// </summary>
        /// <param name="entity">The assignment containing the alert and Market Data identifiers.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created assignment with its server-assigned identifier.</returns>
        public Task<QualityNotificationAlertAssignmentDto.Output> RegisterQualityNotificationAlertAssignmentAsync(QualityNotificationAlertAssignmentDto.Input entity, CancellationToken ctk = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return _client.Exec<QualityNotificationAlertAssignmentDto.Output, QualityNotificationAlertAssignmentDto.Input>(
                HttpMethod.Post, "/dataquality/alertruleassignment", entity, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a quality notification alert assignment by identifier.
        /// </summary>
        /// <param name="id">The assignment identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The assignment, or <see langword="null"/> when it does not exist.</returns>
        public Task<QualityNotificationAlertAssignmentDto.Output> ReadQualityNotificationAlertAssignmentByIdAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertruleassignment".AppendPathSegment(id);
            return _client.Exec<QualityNotificationAlertAssignmentDto.Output>(HttpMethod.Get, url, ctk: ctk);
        }

        /// <summary>
        /// Retrieves a paginated list of quality notification alert assignments.
        /// </summary>
        /// <param name="page">The one-based page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="alertId">Optional quality notification alert filter.</param>
        /// <param name="marketDataId">Optional Market Data filter.</param>
        /// <param name="sort">Optional sort expressions.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated collection of assignments.</returns>
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
        /// Deletes a quality notification alert assignment.
        /// </summary>
        /// <param name="id">The assignment identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        public Task DeleteQualityNotificationAlertAssignmentAsync(int id, CancellationToken ctk = default)
        {
            var url = "/dataquality/alertruleassignment".AppendPathSegment(id);
            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }
    }
}
