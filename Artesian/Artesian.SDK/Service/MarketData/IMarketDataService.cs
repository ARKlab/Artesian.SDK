// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 

using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Override.Enum;
using Artesian.SDK.Dto.UoM;

using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    ///  MarketData service interface
    /// </summary>
    public interface IMarketDataService
    {
        #region MarketData
        /// <summary>
        /// Get MarketData by provider and curve name with MarketDataIdentifier
        /// </summary>
        /// <param name="id">MarketDataIdentifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(MarketDataIdentifier id, CancellationToken ctk = default);
        /// <summary>
        /// Get MarketData by provider and curve name with optional related data.
        /// </summary>
        /// <param name="id">MarketDataIdentifier</param>
        /// <param name="includeCurveSummary">When true, includes curve summary (ranges) in the response</param>
        /// <param name="includeTimeTransform">When true, includes time transform in the response</param>
        /// <param name="includeDataQuality">When true, includes data quality status summary in the response</param>
        /// <param name="skipOverrides">When false, composes original and override metadata. Requires the /featureflag/overridebeta permission during beta. Default true.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity OutputEnriched</returns>
        Task<MarketDataEntity.OutputEnriched> ReadMarketDataRegistryAsync(MarketDataIdentifier id, bool includeCurveSummary = false, bool includeTimeTransform = false, bool includeDataQuality = false, bool skipOverrides = true, CancellationToken ctk = default);
        /// <summary>
        /// Read MarketData by curve id
        /// </summary>
        /// <param name="id">An Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Read MarketData by curve id with optional related data.
        /// </summary>
        /// <param name="id">An Int</param>
        /// <param name="includeCurveSummary">When true, includes curve summary (ranges) in the response</param>
        /// <param name="includeTimeTransform">When true, includes time transform in the response</param>
        /// <param name="includeDataQuality">When true, includes data quality status summary in the response</param>
        /// <param name="skipOverrides">When false, composes original and override metadata. Requires the /featureflag/overridebeta permission during beta. Default true.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity OutputEnriched</returns>
        Task<MarketDataEntity.OutputEnriched> ReadMarketDataRegistryAsync(int id, bool includeCurveSummary = false, bool includeTimeTransform = false, bool includeDataQuality = false, bool skipOverrides = true, CancellationToken ctk = default);
        /// <summary>
        /// Get the MarketData versions by id
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="page">Int</param>
        /// <param name="pageSize">Int</param>
        /// <param name="product">String</param>
        /// <param name="versionFrom">LocalDateTime</param>
        /// <param name="versionTo">LocalDateTime</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of CurveRange entity</returns>
        Task<PagedResult<CurveRange>> ReadCurveRangeAsync(int id, int page, int pageSize, string? product = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, CancellationToken ctk = default);
        /// <summary>
        /// Register the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        Task<MarketDataEntity.Output> RegisterMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default);
        /// <summary>
        /// Save the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        Task<MarketDataEntity.Output> UpdateMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default);
        /// <summary>
        /// Delete the specific MarketData entity by id
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteMarketDataAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Update Derived Configuration for marketData with id supplied in <paramref name="marketDataId"/> and Rebuild
        /// </summary>
        /// <param name="marketDataId">Id of the marketData</param>
        /// <param name="derivedCfg">The Derived Configuration to be updated</param>
        /// <param name="force">Force the update of configuration also if another rebuild process is running (Defualt=false)</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns>MarketData Entity Output</returns>
        Task<MarketDataEntity.Output> UpdateDerivedConfigurationAsync(int marketDataId, DerivedCfgBase derivedCfg, bool force = false, CancellationToken ctk = default);
        #endregion

        #region DataQualityRule
        /// <summary>
        /// Creates a new Data Quality Rule with the specified configuration.
        /// The rule defines validation logic (completeness/freshness or outlier detection) that can be assigned to Market Data entities.
        /// </summary>
        /// <param name="entity">The rule definition including name, type, and configuration.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="DataQualityRuleDto.Output"/> with server-assigned Id and metadata.</returns>
        Task<DataQualityRuleDto.Output> RegisterDataQualityRuleAsync(DataQualityRuleDto.Input entity, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves a Data Quality Rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rule.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="DataQualityRuleDto.Output"/> if found; otherwise 404 Not Found.</returns>
        Task<DataQualityRuleDto.Output> ReadDataQualityRuleByIdAsync(int id, CancellationToken ctk = default);
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
        Task<PagedResult<DataQualityRuleDto.Output>> ReadDataQualityRuleAsync(int page, int pageSize, RuleType? type = null, int? marketDataId = null, string? name = null, int[]? ruleIds = null, string[]? sort = null, CancellationToken ctk = default);
        /// <summary>
        /// Updates an existing Data Quality Rule. The rule's configuration, name, and type can be modified.
        /// Uses optimistic concurrency via the ETag property.
        /// </summary>
        /// <param name="id">The unique identifier of the rule to update.</param>
        /// <param name="entity">The updated rule definition.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated <see cref="DataQualityRuleDto.Output"/> with new metadata.</returns>
        Task<DataQualityRuleDto.Output> UpdateDataQualityRuleAsync(int id, DataQualityRuleDto.Input entity, CancellationToken ctk = default);
        /// <summary>
        /// Deletes a Data Quality Rule by its unique identifier.
        /// Existing assignments referencing this rule should be removed first.
        /// </summary>
        /// <param name="id">The unique identifier of the rule to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        Task DeleteDataQualityRuleAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Creates a new assignment binding a Market Data entity to a Data Quality Rule.
        /// The assignment defines which rule validates which market data.
        /// </summary>
        /// <param name="entity">The assignment definition including MarketDataId and DataQualityRuleId.</param>
        /// <param name="initializationLookbackPeriod">Optional ISO 8601 period (e.g. "P30D") defining how far back in time the rule should validate data on initial assignment. Not persisted.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="MarketDataQualityRuleAssignmentDto.Output"/> with server-assigned Id.</returns>
        Task<MarketDataQualityRuleAssignmentDto.Output> RegisterDataQualityRuleAssignmentAsync(MarketDataQualityRuleAssignmentDto.Input entity, Period? initializationLookbackPeriod = null, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves a DQ rule assignment by its unique identifier, including enriched MarketData and Rule data.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="MarketDataQualityRuleAssignmentDto.Output"/> if found; otherwise 404 Not Found.</returns>
        Task<MarketDataQualityRuleAssignmentDto.Output> ReadDataQualityRuleAssignmentByIdAsync(int id, CancellationToken ctk = default);
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
        Task<PagedResult<MarketDataQualityRuleAssignmentDto.Output>> ReadDataQualityRuleAssignmentAsync(int page, int pageSize, int? marketDataId = null, int? ruleId = null, string? ruleName = null, string[]? sort = null, CancellationToken ctk = default);
        /// <summary>
        /// Updates an assignment's initialization lookback, triggering re-evaluation from the new lookback date.
        /// All existing check results for this assignment are deleted and re-computed.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment to update.</param>
        /// <param name="initializationLookbackPeriod">ISO 8601 period (e.g. "P30D") defining the new lookback window.</param>
        /// <param name="etag">The current ETag for optimistic concurrency control.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated MarketDataQualityRuleAssignmentDto.Output.</returns>
        Task<MarketDataQualityRuleAssignmentDto.Output> UpdateDataQualityRuleAssignmentAsync(int id, Period initializationLookbackPeriod, string etag, CancellationToken ctk = default);
        /// <summary>
        /// Deletes an assignment, removing the binding between a Market Data entity and a Data Quality Rule.
        /// </summary>
        /// <param name="id">The unique identifier of the assignment to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>204 No Content on successful deletion.</returns>
        Task DeleteDataQualityRuleAssignmentAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves the raw event feed for a specific rule assignment.
        /// Returns events after the given timestamp (max 8-day lookback).
        /// </summary>
        /// <param name="id">The rule assignment identifier.</param>
        /// <param name="afterTimestamp">Optional lower bound (events after this instant). Clamped to 8 days ago.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqCheckChangeEventDto.Output"/>.</returns>
        Task<DqCheckChangeEventDto.Output[]> ReadDataQualityRuleAssignmentEventsFeedAsync(int id, Instant? afterTimestamp = null, CancellationToken ctk = default);

        /// <summary>
        /// Creates a new alert rule definition for data quality monitoring.
        /// The alert rule defines the trigger mode (on-event or scheduled)
        /// and the notification channels (email, Teams, Slack, webhook, principal).
        /// Market Data assignments are managed separately via the alertruleassignment endpoint.
        /// </summary>
        /// <param name="entity">The alert rule definition including trigger config and notification channels.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created <see cref="QualityNotificationAlertDto.Output"/> with server-assigned Id.</returns>
        Task<QualityNotificationAlertDto.Output> RegisterQualityNotificationAlertAsync(QualityNotificationAlertDto.Input entity, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves an alert rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The <see cref="QualityNotificationAlertDto.Output"/> if found; otherwise 404 Not Found.</returns>
        Task<QualityNotificationAlertDto.Output> ReadQualityNotificationAlertByIdAsync(int id, CancellationToken ctk = default);
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
        Task<PagedResult<QualityNotificationAlertDto.Output>> ReadQualityNotificationAlertsAsync(int page, int pageSize, string? name = null, int? marketDataId = null, int[]? ruleIds = null, string[]? sort = null, CancellationToken ctk = default);
        /// <summary>
        /// Updates an existing alert rule. Modifies the trigger configuration or notification channels.
        /// Market Data assignments are managed separately via the alertruleassignment endpoint.
        /// Uses optimistic concurrency via the ETag property.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule to update.</param>
        /// <param name="entity">The updated alert definition.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The updated <see cref="QualityNotificationAlertDto.Output"/>.</returns>
        Task<QualityNotificationAlertDto.Output> UpdateQualityNotificationAlertAsync(int id, QualityNotificationAlertDto.Input entity, CancellationToken ctk = default);
        /// <summary>
        /// Deletes an alert rule by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the alert rule to delete.</param>
        /// <param name="ctk">Cancellation token.</param>
        Task DeleteQualityNotificationAlertAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves the materialized events for a specific alert schedule occurrence.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="scheduleTime">The schedule occurrence timestamp.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqCheckChangeEventDto.Output"/>.</returns>
        Task<AlertScheduleEventsDto.Output> ReadAlertScheduleEventsAsync(int alertId, Instant scheduleTime, CancellationToken ctk = default);
        /// <summary>
        /// Lists the most recent schedule occurrence timestamps for an alert.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="lastN">Number of most recent occurrences to return (default 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>Recent schedule timestamps in descending order.</returns>
        Task<Instant[]> ReadAlertScheduleListAsync(int alertId, int lastN = 10, CancellationToken ctk = default);
        /// <summary>
        /// Retrieves the materialized events from the latest schedule occurrence for an alert.
        /// Returns an empty array if no schedule has been materialized yet.
        /// </summary>
        /// <param name="alertId">The alert identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An array of <see cref="DqCheckChangeEventDto.Output"/> from the latest schedule.</returns>
        Task<AlertScheduleEventsDto.Output> ReadAlertScheduleLastEventsAsync(int alertId, CancellationToken ctk = default);

        /// <summary>
        /// Creates an assignment binding a Market Data entity to a quality notification alert.
        /// </summary>
        /// <param name="entity">The assignment containing the alert and Market Data identifiers.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created assignment.</returns>
        Task<QualityNotificationAlertAssignmentDto.Output> RegisterQualityNotificationAlertAssignmentAsync(QualityNotificationAlertAssignmentDto.Input entity, CancellationToken ctk = default);

        /// <summary>
        /// Retrieves a quality notification alert assignment by identifier.
        /// </summary>
        /// <param name="id">The assignment identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The assignment, or <see langword="null"/> when it does not exist.</returns>
        Task<QualityNotificationAlertAssignmentDto.Output> ReadQualityNotificationAlertAssignmentByIdAsync(int id, CancellationToken ctk = default);

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
        Task<PagedResult<QualityNotificationAlertAssignmentDto.Output>> ReadQualityNotificationAlertAssignmentsAsync(int page, int pageSize, int? alertId = null, int? marketDataId = null, string[]? sort = null, CancellationToken ctk = default);

        /// <summary>
        /// Deletes a quality notification alert assignment.
        /// </summary>
        /// <param name="id">The assignment identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        Task DeleteQualityNotificationAlertAssignmentAsync(int id, CancellationToken ctk = default);

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
        /// <returns>An enumerable of <see cref="CheckResultExtract.Vts"/> with Version populated.</returns>
        Task<IEnumerable<CheckResultExtract.Vts>> GetDataQualityCheckResultExtractVtsAsync(LocalDateTime version, Granularity granularity, LocalDate start, LocalDate end, string timeZone, int[]? assignmentIds = null, CancellationToken ctk = default);

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
        /// <returns>An enumerable of <see cref="CheckResultExtract.Ts"/>.</returns>
        Task<IEnumerable<CheckResultExtract.Ts>> GetDataQualityCheckResultExtractTsAsync(Granularity granularity, LocalDate start, LocalDate end, string timeZone, int[]? assignmentIds = null, CancellationToken ctk = default);

        /// <summary>
        /// Retrieves a paged summary of data quality check results (CurveRange-like view per assignment).
        /// Includes range metadata, product info, and assignment details.
        /// </summary>
        /// <param name="page">Page number (1-based, default: 1).</param>
        /// <param name="pageSize">Items per page (default: 10).</param>
        /// <param name="marketDataIds">Optional filter by MarketData IDs.</param>
        /// <param name="ruleIds">Optional filter by Rule IDs.</param>
        /// <param name="assignmentIds">Optional filter by Assignment IDs.</param>
        /// <param name="dqStatus">Optional filter by aggregated DQ status (OK/KO).</param>
        /// <param name="from">Optional range start filter.</param>
        /// <param name="to">Optional range end filter.</param>
        /// <param name="versionFrom">Optional version range start.</param>
        /// <param name="versionTo">Optional version range end.</param>
        /// <param name="products">Optional filter by products.</param>
        /// <param name="skipEmptyRanges">When true, return only summaries with non-empty range data (default: false).</param>
        /// <param name="sort">Optional sort expressions.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated result containing <see cref="CheckResultCheckSummaryDto"/> items.</returns>
        Task<PagedResult<CheckResultCheckSummaryDto>> GetDataQualityCheckResultCheckSummaryAsync(int page, int pageSize, int[]? marketDataIds = null, int[]? ruleIds = null, int[]? assignmentIds = null, CheckAggregatedStatus? dqStatus = null, Instant? from = null, Instant? to = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, string[]? products = null, bool skipEmptyRanges = false, string[]? sort = null, CancellationToken ctk = default);

        /// <summary>
        /// Retrieves market data entities with their DQ status summary for a given rule.
        /// Results are sorted by LastCheckTime descending.
        /// </summary>
        /// <param name="ruleId">Optional Rule ID filter.</param>
        /// <param name="marketDataIds">Optional filter by MarketData IDs.</param>
        /// <param name="dqStatus">Optional aggregated DQ status filter (OK/KO). When KO, returns only Market Data whose overall status is KO.</param>
        /// <param name="limit">Maximum number of results to return (1..1000, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An enumerable of <see cref="MarketDataDqStatusSummaryDto"/> items.</returns>
        Task<IEnumerable<MarketDataDqStatusSummaryDto>> GetMarketDataDqStatusSummaryAsync(int? ruleId = null, int[]? marketDataIds = null, CheckAggregatedStatus? dqStatus = null, int limit = 10, CancellationToken ctk = default);

        /// <summary>
        /// Retrieves DQ rules with their status summary, optionally filtered by a specific market data entity.
        /// Results are sorted by LastCheckTime descending.
        /// </summary>
        /// <param name="marketDataId">Optional filter by a specific MarketData ID.</param>
        /// <param name="ruleIds">Optional filter by specific Rule IDs.</param>
        /// <param name="dqStatus">Optional aggregated DQ status filter (OK/KO). When KO, returns only rules whose overall status is KO.</param>
        /// <param name="limit">Maximum number of results to return (1..1000, default: 10).</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>An enumerable of <see cref="DqRuleDqStatusSummaryDto"/> items.</returns>
        Task<IEnumerable<DqRuleDqStatusSummaryDto>> GetDqRuleDqStatusSummaryAsync(int? marketDataId = null, int[]? ruleIds = null, CheckAggregatedStatus? dqStatus = null, int limit = 10, CancellationToken ctk = default);
        #endregion

        #region SearchFacet
        /// <summary>
        /// Search the marketdata metadata
        /// </summary>
        /// <param name="filter">ArtesianSearchFilter containing the search params</param>
        /// <param name="doNotLoadAdditionalInfo">Skip loading up-to-date curve range and transform</param>
        /// <param name="includeCurveSummary">When true, includes curve summary (ranges) in the response</param>
        /// <param name="includeTimeTransform">When true, includes time transform in the response</param>
        /// <param name="includeDataQuality">When true, includes data quality status summary in the response</param>
        /// <param name="skipOverrides">When false, composes original and override metadata. Requires the /featureflag/overridebeta permission during beta. Default true.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ArtesianSearchResults entity</returns>
        Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, bool doNotLoadAdditionalInfo = false, bool includeCurveSummary = false, bool includeTimeTransform = false, bool includeDataQuality = false, bool skipOverrides = true, CancellationToken ctk = default);
        #endregion

        #region Operations
        /// <summary>
        /// A sequence of operation will be applied to the MarketData identified by ids
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="operations"></param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        Task<IList<MarketDataEntity.Output>> PerformOperationsAsync(Operations operations, CancellationToken ctk = default);
        #endregion

        #region TimeTransform
        /// <summary>
        /// Retrieve the TimeTransform entity from the database
        /// </summary>
        /// <param name="timeTransformId">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Time Transform Entity</returns>
        Task<TimeTransform> ReadTimeTransformBaseAsync(int timeTransformId, CancellationToken ctk = default);
        /// <summary>
        /// Read the TimeTransform entity from the database paged
        /// </summary>
        /// <param name="page">Int</param>
        /// <param name="pageSize">Int</param>
        /// <param name="userDefined">Bool</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged Result of Time Transform Entity</returns>
        Task<PagedResult<TimeTransform>> ReadTimeTransformsAsync(int page, int pageSize, bool userDefined, CancellationToken ctk = default);
        /// <summary>
        /// Register a new TimeTransform
        /// </summary>
        /// <param name="timeTransform">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Time Transform Entity</returns>
        Task<TimeTransform> RegisterTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default);
        /// <summary>
        /// Update the TimeTransform
        /// </summary>
        /// <param name="timeTransform">The entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Time Transform Entity</returns>
        Task<TimeTransform> UpdateTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default);
        /// <summary>
        /// Delete the TimeTransform
        /// </summary>
        /// <param name="timeTransformId">The entity id we are going to delete</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteTimeTransformSimpleShiftAsync(int timeTransformId, CancellationToken ctk = default);
        #endregion

        #region Filters
        /// <summary>
        /// Create a new Filter
        /// </summary>
        /// <param name="filter">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Custom Filter Entity</returns>
        Task<CustomFilter> CreateFilter(CustomFilter filter, CancellationToken ctk = default);
        /// <summary>
        /// Update specific Filter
        /// </summary>
        /// <param name="filterId">The entity id</param>
        /// <param name="filter">The entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Custom Filter Entity</returns>
        Task<CustomFilter> UpdateFilter(int filterId, CustomFilter filter, CancellationToken ctk = default);
        /// <summary>
        /// Read specific filter
        /// </summary>
        /// <param name="filterId">The entity id to get</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Custom Filter Entity</returns>
        Task<CustomFilter> ReadFilter(int filterId, CancellationToken ctk = default);
        /// <summary>
        /// Remove specific Filter
        /// </summary>
        /// <param name="filterId">The entity id to be removed</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Custom Filter Entity</returns>
        Task<CustomFilter> RemoveFilter(int filterId, CancellationToken ctk = default);
        /// <summary>
        /// Read all filters
        /// </summary>
        /// <param name="page">Int</param>
        /// <param name="pageSize">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged Result of Custom Filter Entity</returns>
        Task<PagedResult<CustomFilter>> ReadFilters(int page, int pageSize, CancellationToken ctk = default);
        #endregion

        #region Acl
        /// <summary>
        /// Retrieve the ACL Path Roles by path
        /// </summary>
        /// <param name="path">The path (starting with "/" char. Ex. "/marketdata/system/" identifies folder "marketdata" with a subfolder "system", roles are assigned to "system" subfolder. Ex. "/marketdata/genoacurve" identifies folder "marketdata" with entity "genoacurve", roles are assigned to "genoacurve" entity.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of AuthorizationPath Output entity</returns>
        Task<IEnumerable<AuthorizationPath.Output>> ReadRolesByPath(PathString path, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ACL Path Roles paged
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="principalIds">The principal ids I want to inspect, encoded.( ex. u:user@example.com for users and clients,g:1001 for groups)</param>
        /// <param name="asOf">LocalDateTime we want to inspect</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>AclPath entity</returns>
        Task<PagedResult<AclPath>> GetRoles(int page, int pageSize, string[] principalIds, LocalDateTime? asOf = null, CancellationToken ctk = default);
        /// <summary>
        /// Upsert the ACL Path Roles
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="upsert">The entity we want to upsert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task UpsertRoles(AuthorizationPath.Input upsert, CancellationToken ctk = default);
        /// <summary>
        /// Add a role to the ACL Path
        /// </summary>
        /// <param name="add">The entity we want to add. At the path add.Path we add the add.Roles</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task AddRoles(AuthorizationPath.Input add, CancellationToken ctk = default);
        /// <summary>
        /// Remove a role from the ACL Path
        /// </summary>
        /// <param name="remove">The entity we want to remove. At the path remove.Path we remove the remove.Roles</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task RemoveRoles(AuthorizationPath.Input remove, CancellationToken ctk = default);
        #endregion

        #region Admin
        /// <summary>
        /// Create a new Authorization Group
        /// </summary>
        /// <param name="group">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Auth Group entity</returns>
        Task<AuthGroup> CreateAuthGroup(AuthGroup group, CancellationToken ctk = default);
        /// <summary>
        /// Update an Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="group">The entity to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Auth Group entity</returns>
        Task<AuthGroup> UpdateAuthGroup(int groupID, AuthGroup group, CancellationToken ctk = default);
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task RemoveAuthGroup(int groupID, CancellationToken ctk = default);
        /// <summary>
        /// Read Authorization Group
        /// </summary>
        /// <param name="groupID">The entity Identifier</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>AuthGroup entity</returns>
        Task<AuthGroup> ReadAuthGroup(int groupID, CancellationToken ctk = default);
        /// <summary>
        /// Remove an Authorization Group
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged Result of Auth Group entity</returns>
        Task<PagedResult<AuthGroup>> ReadAuthGroups(int page, int pageSize, CancellationToken ctk = default);
        /// <summary>
        /// Get a list of Principals of the selected user
        /// </summary>
        /// <param name="user">The user name</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>List of Principals entity</returns>
        Task<IList<Principals>> ReadUserPrincipals(string user, CancellationToken ctk = default);
        #endregion

        #region UpsertCurve
        /// <summary>
        /// Upsert the curve data supplied in <paramref name="data"/>
        /// </summary>
        /// <remarks>
        /// Unified controller for saving curve data
        /// ID, TimeZone and DownloadedAt fields should not be null
        /// - Market Data Assessment: MarketAssessment field should not be null, other fields should be null
        /// - Actual TimeSerie: Rows field should not be null, other fields should be null-
        /// - Versioned TimeSerie: Rows and Version fields should not be null, other fields should be null
        /// </remarks>
        /// <param name="data">
        /// An object that represents MarketDataAssessment, ActualTimeSerie or VersionedTimeSerie
        /// </param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task UpsertCurveDataAsync(UpsertCurveData data, CancellationToken ctk = default);
        #endregion

        #region DeleteCurve
        /// <summary>
        /// Delete the curve data supplied in <paramref name="data"/>
        /// </summary>
        /// <remarks>
        /// Unified controller for deleting curve data
        /// ID, TimeZone and Range fields should not be null
        /// - Product: MarketDataAssessment, BidAsk and Auction should not be null. For Actual and Versioned should be null
        /// - Actual TimeSerie: Version fiels should be null
        /// - Versioned TimeSerie: Version fields should not be null
        /// </remarks>
        /// <param name="data">
        /// An object that represents Auction, BidAsk, MarketDataAssessment, ActualTimeSerie or VersionedTimeSerie
        /// </param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteCurveDataAsync(DeleteCurveData data, CancellationToken ctk = default);
        #endregion

        #region MarketDataOverride
        /// <summary>
        /// Upserts an override or fallback correction for a portion of a Market Data.
        /// </summary>
        /// <param name="data">The curve data and override configuration.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>The created or updated override metadata entries.</returns>
        Task<IReadOnlyList<OverrideMetadataEntry>> UpsertCurveDataOverrideAsync(UpsertCurveDataOverride data, CancellationToken ctk = default);

        /// <summary>
        /// Deletes the data and metadata associated with an override or fallback correction.
        /// </summary>
        /// <param name="id">The override metadata identifier.</param>
        /// <param name="ctk">Cancellation token.</param>
        Task DeleteOverrideDataAsync(Guid id, CancellationToken ctk = default);

        /// <summary>
        /// Retrieves paged override and fallback metadata for a Market Data.
        /// </summary>
        /// <param name="marketDataId">The Market Data identifier.</param>
        /// <param name="kind">Optional correction-kind filter.</param>
        /// <param name="page">The one-based page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="ctk">Cancellation token.</param>
        /// <returns>A paginated collection of override metadata entries.</returns>
        Task<PagedResult<OverrideMetadataEntry>> ReadOverrideMetadataAsync(int marketDataId, OverrideKind? kind = null, int page = 1, int pageSize = 10, CancellationToken ctk = default);
        #endregion

        #region ApiKey
        /// <summary>
        /// Create new ApiKey
        /// </summary>
        /// <param name="apiKeyRecord">The entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        Task<ApiKey.Output> CreateApiKeyAsync(ApiKey.Input apiKeyRecord, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="key">The Key</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        Task<ApiKey.Output> ReadApiKeyByKeyAsync(string key, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the ApiKey entity
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ApiKey Output entity</returns>
        Task<ApiKey.Output> ReadApiKeyByIdAsync(int id, CancellationToken ctk = default);
        /// <summary>
        /// Retrieve the apikeys paged
        /// </summary>
        /// <param name="page">The requested page</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="userId">The userid we want to filter for</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of ApiKey Output entity</returns>
        Task<PagedResult<ApiKey.Output>> ReadApiKeysAsync(int page, int pageSize, string userId, CancellationToken ctk = default);
        /// <summary>
        /// Delete the ApiKey
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteApiKeyAsync(int id, CancellationToken ctk = default);
        #endregion

        #region UnitOfMeasure
        /// <summary>
        /// Check the conversion between the input units of measure and the target unit of measure
        /// </summary>
        /// <param name="inputUnitsOfMeasure">Input units of measure</param>
        /// <param name="targetUnitOfMeasure">Target unit of measure</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>The CheckConversionResult class.
        ///             TargetUnitOfMeasure = the target unit of measure
        ///             ConvertibleInputUnitsOfMeasure = list of convertible input units of measure
        ///             NotConvertibleInputUnitsOfMeasure = list of not convertible input units of measure</returns>
        Task<CheckConversionResult> CheckConversionAsync(string[] inputUnitsOfMeasure, string targetUnitOfMeasure, CancellationToken ctk = default);
        #endregion

        #region Utils
        /// <summary>
        /// Derived Transform Query Validation
        /// </summary>
        /// <param name="request">Query to be validated from Derived Transform</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        Task<DerivedTransformQueryValidationResponse.V1> DerivedTransformQueryValidation(DerivedTransformQueryValidation.V1 request, CancellationToken ctk = default);
        #endregion
    }
}
