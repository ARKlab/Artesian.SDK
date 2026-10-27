# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]
### Added
- Add paged Data Quality alert schedule events and optional ACL-aware Market Data details.
- Add MAS and BidAsk data quality check result extraction APIs.
- Support MAS and BidAsk completeness and freshness rule configurations.
- Support MAS and BidAsk absolute-bound outlier rule configurations.
- Support optional Market Data details in TS and VTS data quality extraction.

### Changed
- BREAKING: Insert parameters before `CancellationToken` in `ReadDataQualityRuleAssignmentByIdAsync`, `ReadDataQualityRuleAssignmentsAsync`, `ReadDataQualityRuleAssignmentEventsFeedAsync`, `ReadAlertScheduleEventsAsync`, `ReadAlertScheduleLastEventsAsync`, `ReadQualityNotificationAlertAssignmentByIdAsync`, `ReadQualityNotificationAlertAssignmentsAsync`, `GetDataQualityCheckResultExtractVtsAsync`, `GetDataQualityCheckResultExtractTsAsync`, `GetDataQualityCheckResultCheckSummaryAsync`, and `GetMarketDataDqStatusSummaryAsync`; positional cancellation-token calls must be updated and compiled consumers must rebuild.
- BREAKING: Change `Events` in alert schedule event responses from an array to `PagedResult<>`; callers must access items through `Events.Data`.
- BREAKING: Reject Data Quality `pageSize` or `limit` above 100 and `assignmentIds` arrays containing more than 100 IDs client-side with `ArgumentOutOfRangeException` (previously invalid values threw `ArgumentException`).
- BREAKING: Require both `LowerBound` and `UpperBound` on `OutlierAbsoluteBoundConfigDto`; initializers that set only one bound no longer compile.
- Default `MarketDataType` to the matching type on MAS and BidAsk outlier and completeness configurations.
- BREAKING: Default `includeMarketData` to `false` for Data Quality reads; Market Data names and providers are omitted unless requested.
- BREAKING: Require Data Quality ACL grants on `/dataquality/dqrule/all` and `/dataquality/alertrule/all`; grants on `/featureflag/dqbeta` no longer apply.
- Throw `ArgumentOutOfRangeException` consistently for invalid `page` and `pageSize` in Data Quality ACL-protected reads.

### Security
- Update MessagePack to 3.1.12 to address [GHSA-qhrr-8q5h-9q3h](https://github.com/advisories/GHSA-qhrr-8q5h-9q3h).