using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Override.Enum;
using Artesian.SDK.Service;
using Artesian.SDK.Factory;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Tests.Samples
{
    public class DataQualityTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task DataQualityCrud()
        {
            var marketDataService = new MarketDataService(_cfg);

            CancellationToken ctk = default;

            // Register rule
            var rulePayload = new DataQualityRuleDto.Input
            {
                Name = "TestRule" + Guid.NewGuid(),
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new ActualCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 0 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(1)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.Zero,
                        RecordRangeTo = Period.FromHours(1)
                    }
                }
            };

            // Read Rule
            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(rulePayload, ctk);

            var readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.AreEqual(ruleCreated.Id, readDataQualityRule.Id);

            // Update rule
            await marketDataService.UpdateDataQualityRuleAsync(ruleCreated.Id, new DataQualityRuleDto.Input
            {
                Name = "TestRuleUpdated" + Guid.NewGuid(),
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new ActualCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 0 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(1)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.Zero,
                        RecordRangeTo = Period.FromHours(1)
                    }
                }
            }, ctk);

            readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.AreEqual("TestRuleUpdated", readDataQualityRule.Name);

            // Delete rule
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);

            readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.IsNull(readDataQualityRule);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DataQualityAbsoluteBoundOutlierRule()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;

            var input = new MarketDataEntity.Input
            {
                ProviderName = "SpecFlowDataQuality",
                MarketDataName = "Temperature_" + Guid.NewGuid(),
                Type = MarketDataTypeV2.ActualTimeSerie,
                OriginalGranularity = Granularity.Hour,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var marketData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    input.ProviderName,
                    input.MarketDataName));

            if (!await marketData.IsRegistered(ctk))
                await marketData.Register(input, ctk);

            await marketData.Load(ctk);

            var rulePayload = new DataQualityRuleDto.Input
            {
                Name = "Temperature absolute bound outlier check" + Guid.NewGuid(),
                Type = RuleType.Outlier,
                Configuration = new OutlierConfigDto
                {
                    Model = new OutlierAbsoluteBoundConfigDto
                    {
                        LowerBound = -50.0,
                        UpperBound = 100.0
                    }
                },
                ETag = "{opaque string}"
            };

            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(rulePayload, ctk);

            ClassicAssert.IsNotNull(ruleCreated);
            ClassicAssert.AreEqual(rulePayload.Name, ruleCreated.Name);
            ClassicAssert.AreEqual(RuleType.Outlier, ruleCreated.Type);
            ClassicAssert.IsInstanceOf<OutlierConfigDto>(ruleCreated.Configuration);
            var configuration = (OutlierConfigDto)ruleCreated.Configuration;
            ClassicAssert.IsInstanceOf<OutlierAbsoluteBoundConfigDto>(configuration.Model);
            var model = (OutlierAbsoluteBoundConfigDto)configuration.Model;
            ClassicAssert.AreEqual(-50.0, model.LowerBound);
            ClassicAssert.AreEqual(100.0, model.UpperBound);

            var assignmentPayload = new MarketDataQualityRuleAssignmentDto.Input
            {
                MarketDataId = marketData.MarketDataId!.Value,
                DataQualityRuleId = ruleCreated.Id
            };

            var assignmentCreated = await marketDataService.RegisterDataQualityRuleAssignmentAsync(
                assignmentPayload,
                ctk: ctk);

            ClassicAssert.IsNotNull(assignmentCreated);
            ClassicAssert.AreEqual(marketData.MarketDataId.Value, assignmentCreated.MarketDataId);
            ClassicAssert.AreEqual(ruleCreated.Id, assignmentCreated.DataQualityRuleId);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task DataQualityAssignment()
        {
            var marketDataService = new MarketDataService(_cfg);

            CancellationToken ctk = default;

            // Register a market data entity to assign the rule to
            var input = new MarketDataEntity.Input
            {
                ProviderName = "SpecFlowDataQuality",
                MarketDataName = Guid.NewGuid().ToString(),
                Type = MarketDataTypeV2.ActualTimeSerie,
                OriginalGranularity = Granularity.Hour,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    input.ProviderName,
                    input.MarketDataName)
                );

            var isRegd = await mktData.IsRegistered(ctk);

            if (!isRegd)
                await mktData.Register(input, ctk);

            await mktData.Load(ctk);

            // Register rule
            var rulePayload = new DataQualityRuleDto.Input
            {
                Name = "TestRule" + Guid.NewGuid(),
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new ActualCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 0 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(1)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.Zero,
                        RecordRangeTo = Period.FromHours(1)
                    }
                }
            };

            // Read Rule
            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(rulePayload, ctk);

            var readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.AreEqual(readDataQualityRule.Id, ruleCreated.Id);

            // Assign rule to market data
            var assignmentPayload = new MarketDataQualityRuleAssignmentDto.Input
            {
                MarketDataId = mktData.MarketDataId != null ? mktData.MarketDataId.Value : 1,
                DataQualityRuleId = readDataQualityRule.Id
            };

            ClassicAssert.AreEqual(assignmentPayload.DataQualityRuleId, ruleCreated.Id);

            var assignmentCreated = await marketDataService.RegisterDataQualityRuleAssignmentAsync(assignmentPayload, ctk: ctk);

            var dataQualityRuleAssignment = await marketDataService.ReadDataQualityRuleAssignmentByIdAsync(assignmentCreated.Id, ctk);

            ClassicAssert.AreEqual(dataQualityRuleAssignment.Id, assignmentCreated.Id);

            // Delete assignment
            await marketDataService.DeleteDataQualityRuleAssignmentAsync(assignmentCreated.Id, ctk);

            dataQualityRuleAssignment = await marketDataService.ReadDataQualityRuleAssignmentByIdAsync(assignmentCreated.Id, ctk);

            ClassicAssert.IsNull(dataQualityRuleAssignment);

            // Delete rule
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);

            readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.IsNull(readDataQualityRule);

            // Delete MarketData entity
            if (mktData.MarketDataId.HasValue)
                await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task DataQualityQueryCheckResultCheckSummary()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;

            // Step 1: Create a Daily Actual TimeSerie named TsCheckSummaryQuery from Provider DqCheckResult
            var input = new MarketDataEntity.Input
            {
                ProviderName = "DqCheckResult",
                MarketDataName = "TsCheckSummaryQuery_" + Guid.NewGuid(),
                Type = MarketDataTypeV2.ActualTimeSerie,
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    input.ProviderName,
                    input.MarketDataName)
                );

            var isRegd = await mktData.IsRegistered(ctk);

            if (!isRegd)
                await mktData.Register(input, ctk);

            await mktData.Load(ctk);

            // Step 2: Write values with gaps (2025-01-01, 2025-01-03, 2025-01-05)
            var data = ((MarketData)mktData).EditActual();
            data.TryAddData(new LocalDate(2025, 1, 1), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 3), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 5), 10.0);

            await data.Save(Instant.FromUtc(2025, 1, 14, 0, 0), ctk: ctk);

            // Step 3: Create a data quality rule
            var rulePayload = new DataQualityRuleDto.Input
            {
                Name = "TsSummaryRule_" + Guid.NewGuid(),
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new ActualCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 0 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(1)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.Zero,
                        RecordRangeTo = Period.FromDays(1)
                    }
                }
            };

            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(rulePayload, ctk);
            ClassicAssert.IsNotNull(ruleCreated);
            ClassicAssert.AreEqual(ruleCreated.Name, rulePayload.Name);

            // Step 4: Create assignment with initializationLookbackPeriod P13D
            var assignmentPayload = new MarketDataQualityRuleAssignmentDto.Input
            {
                MarketDataId = mktData.MarketDataId!.Value,
                DataQualityRuleId = ruleCreated.Id
            };

            var assignmentCreated = await marketDataService.RegisterDataQualityRuleAssignmentAsync(
                assignmentPayload,
                Period.FromDays(13),
                ctk);

            ClassicAssert.IsNotNull(assignmentCreated);
            ClassicAssert.AreEqual(assignmentCreated.DataQualityRuleId, assignmentPayload.DataQualityRuleId);

            // Step 5: Wait for deferred execution to complete
            // In a real scenario, you would wait for background jobs to process
            await Task.Delay(TimeSpan.FromSeconds(5), ctk);

            // Step 6: Query the check result check summary
            var checkSummaryResult = await marketDataService.GetDataQualityCheckResultCheckSummaryAsync(
                page: 1,
                pageSize: 100,
                assignmentIds: new[] { assignmentCreated.Id },
                ctk: ctk);

            // Step 7: Verify the results
            ClassicAssert.IsNotNull(checkSummaryResult);
            ClassicAssert.IsNotNull(checkSummaryResult.Data);
            var checkSummaries = checkSummaryResult.Data.ToList();
            ClassicAssert.AreEqual(1, checkSummaries.Count);

            var checkSummary = checkSummaries.Single();
            ClassicAssert.IsNotNull(checkSummary.Assignment);
            var checkSummaryAssignment = checkSummary.Assignment!;
            ClassicAssert.AreEqual(assignmentCreated.Id, checkSummaryAssignment.Id);
            ClassicAssert.AreEqual(mktData.MarketDataId.Value, checkSummaryAssignment.MarketDataId);
            ClassicAssert.AreEqual(ruleCreated.Id, checkSummaryAssignment.DataQualityRuleId);
            ClassicAssert.IsNotNull(checkSummaryAssignment.MarketData);
            ClassicAssert.AreEqual(input.ProviderName, checkSummaryAssignment.MarketData!.ProviderName);
            ClassicAssert.AreEqual(input.MarketDataName, checkSummaryAssignment.MarketData.MarketDataName);
            ClassicAssert.IsNotNull(checkSummaryAssignment.DataQualityRule);
            ClassicAssert.AreEqual(rulePayload.Name, checkSummaryAssignment.DataQualityRule!.Name);
            ClassicAssert.AreNotEqual(default(Instant), checkSummary.LastCheckTime);
            ClassicAssert.AreNotEqual(default(Instant), checkSummary.LastUpdated);
            ClassicAssert.AreNotEqual(default(Instant), checkSummary.Created);
            ClassicAssert.LessOrEqual(checkSummary.RangeStart, checkSummary.RangeEnd);
            ClassicAssert.AreEqual(CheckAggregatedStatus.KO, checkSummary.AggregatedStatus);
            ClassicAssert.IsNull(checkSummary.Version);
            ClassicAssert.IsNull(checkSummary.VersionFrom);

            // Step 8: Query the Market Data DQ status summary
            var marketDataDqStatusSummary = await marketDataService.GetMarketDataDqStatusSummaryAsync(
                ruleId: ruleCreated.Id,
                marketDataIds: new[] { mktData.MarketDataId.Value },
                limit: 100,
                ctk: ctk);

            ClassicAssert.IsNotNull(marketDataDqStatusSummary);
            var marketDataDqStatusSummaries = marketDataDqStatusSummary.ToList();
            ClassicAssert.AreEqual(1, marketDataDqStatusSummaries.Count);

            var marketDataSummary = marketDataDqStatusSummaries.Single();
            ClassicAssert.AreEqual(mktData.MarketDataId.Value, marketDataSummary.MarketDataId);
            ClassicAssert.IsNotNull(marketDataSummary.MarketData);
            var returnedMarketData = marketDataSummary.MarketData!;
            ClassicAssert.AreEqual(mktData.MarketDataId.Value, returnedMarketData.MarketDataId);
            ClassicAssert.AreEqual(input.ProviderName, returnedMarketData.ProviderName);
            ClassicAssert.AreEqual(input.MarketDataName, returnedMarketData.MarketDataName);
            ClassicAssert.IsNotNull(marketDataSummary.Assignments);

            var returnedAssignment = marketDataSummary.Assignments!.Single();
            ClassicAssert.AreEqual(assignmentCreated.Id, returnedAssignment.Id);
            ClassicAssert.AreEqual(mktData.MarketDataId.Value, returnedAssignment.MarketDataId);
            ClassicAssert.AreEqual(ruleCreated.Id, returnedAssignment.DataQualityRuleId);
            ClassicAssert.IsNotNull(marketDataSummary.StatusSummary);
            var marketDataStatusSummary = marketDataSummary.StatusSummary!;
            ClassicAssert.IsNotNull(marketDataStatusSummary.LastCheckTime);
            ClassicAssert.AreEqual(CheckAggregatedStatus.KO, marketDataStatusSummary.OverallStatus);
            ClassicAssert.AreEqual(1, marketDataStatusSummary.ActiveRulesCount);
            ClassicAssert.AreEqual(1, marketDataStatusSummary.FailedRulesCount);
            ClassicAssert.IsNotNull(marketDataStatusSummary.From);
            ClassicAssert.IsNotNull(marketDataStatusSummary.To);

            // Step 9: Query the TS check result extract
            var tsExtract = await marketDataService.GetDataQualityCheckResultExtractTsAsync(
                Granularity.Day,
                checkSummary.RangeStart,
                checkSummary.RangeEnd.PlusDays(1),
                "UTC",
                new[] { assignmentCreated.Id },
                ctk);

            ClassicAssert.IsNotNull(tsExtract);
            var tsExtractResults = tsExtract.ToList();
            ClassicAssert.IsNotEmpty(tsExtractResults);
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.AssignmentId == assignmentCreated.Id));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.MarketDataId == mktData.MarketDataId.Value));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.RuleId == ruleCreated.Id));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.ProviderName == input.ProviderName));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.CurveName == input.MarketDataName));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.RuleName == rulePayload.Name));
            ClassicAssert.IsTrue(tsExtractResults.All(x => x.CompetenceStart <= x.CompetenceEnd));
            ClassicAssert.IsTrue(tsExtractResults.Any(x => x.IssueCount > 0));

            // Step 10: Query the DQ rule status summary
            var dqRuleDqStatusSummary = await marketDataService.GetDqRuleDqStatusSummaryAsync(
                marketDataId: mktData.MarketDataId.Value,
                ruleIds: new[] { ruleCreated.Id },
                limit: 100,
                ctk: ctk);

            ClassicAssert.IsNotNull(dqRuleDqStatusSummary);
            var dqRuleDqStatusSummaries = dqRuleDqStatusSummary.ToList();
            ClassicAssert.AreEqual(1, dqRuleDqStatusSummaries.Count);

            var dqRuleSummary = dqRuleDqStatusSummaries.Single();
            ClassicAssert.AreEqual(ruleCreated.Id, dqRuleSummary.RuleId);
            ClassicAssert.IsNotNull(dqRuleSummary.StatusSummary);
            var dqRuleStatusSummary = dqRuleSummary.StatusSummary!;
            ClassicAssert.IsNotNull(dqRuleStatusSummary.LastCheckTime);
            ClassicAssert.AreEqual(CheckAggregatedStatus.KO, dqRuleStatusSummary.OverallStatus);
            ClassicAssert.AreEqual(1, dqRuleStatusSummary.ActiveRulesCount);
            ClassicAssert.AreEqual(1, dqRuleStatusSummary.FailedRulesCount);
            ClassicAssert.AreEqual(marketDataStatusSummary.From, dqRuleStatusSummary.From);
            ClassicAssert.AreEqual(marketDataStatusSummary.To, dqRuleStatusSummary.To);

            // Cleanup
            await marketDataService.DeleteDataQualityRuleAssignmentAsync(assignmentCreated.Id, ctk);
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);

            if (mktData.MarketDataId.HasValue)
                await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task DataQualityQueryCheckResultExtractVts()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;
            var currentTime = SystemClock.Instance.GetCurrentInstant();
            var currentUtc = currentTime.InUtc().LocalDateTime;
            var version = new LocalDateTime(
                currentUtc.Year,
                currentUtc.Month,
                currentUtc.Day,
                0,
                0);
            var rangeStart = version.Date.PlusDays(-13);
            var rangeEnd = version.Date;

            var input = new MarketDataEntity.Input
            {
                ProviderName = "DqCheckResultVts",
                MarketDataName = "VtsExtractQuery_" + Guid.NewGuid(),
                Type = MarketDataTypeV2.VersionedTimeSerie,
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(input.ProviderName, input.MarketDataName));

            if (!await mktData.IsRegistered(ctk))
                await mktData.Register(input, ctk);

            await mktData.Load(ctk);

            var data = mktData.EditVersioned(version);
            data.TryAddData(rangeStart, 10.0);
            data.TryAddData(rangeStart.PlusDays(2), 10.0);
            data.TryAddData(rangeStart.PlusDays(4), 10.0);
            await data.Save(currentTime, ctk: ctk);

            var rulePayload = new DataQualityRuleDto.Input
            {
                Name = "VtsExtractRule_" + Guid.NewGuid(),
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new VersionedCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.VersionedTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 0 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(1)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.Zero,
                        RecordRangeTo = Period.FromDays(1)
                    },
                    VersionToleranceFrom = Period.FromHours(-1),
                    VersionToleranceTo = Period.FromHours(1),
                    VersionPrecision = PeriodPrecision.Hour
                }
            };

            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(rulePayload, ctk);
            var assignmentCreated = await marketDataService.RegisterDataQualityRuleAssignmentAsync(
                new MarketDataQualityRuleAssignmentDto.Input
                {
                    MarketDataId = mktData.MarketDataId!.Value,
                    DataQualityRuleId = ruleCreated.Id
                },
                Period.FromDays(13),
                ctk);

            await Task.Delay(TimeSpan.FromSeconds(5), ctk);

            var checkSummaryResult = await marketDataService.GetDataQualityCheckResultCheckSummaryAsync(
                page: 1,
                pageSize: 100,
                assignmentIds: new[] { assignmentCreated.Id },
                ctk: ctk);

            ClassicAssert.IsNotNull(checkSummaryResult.Data);
            var versionedCheckSummary = checkSummaryResult.Data.First(x => x.Version.HasValue);
            var checkedVersion = versionedCheckSummary.Version!.Value;

            var vtsExtract = await marketDataService.GetDataQualityCheckResultExtractVtsAsync(
                checkedVersion,
                Granularity.Day,
                versionedCheckSummary.RangeStart,
                versionedCheckSummary.RangeEnd.PlusDays(1),
                "UTC",
                new[] { assignmentCreated.Id },
                ctk);

            ClassicAssert.IsNotNull(vtsExtract);
            var vtsExtractResults = vtsExtract.ToList();
            ClassicAssert.IsNotEmpty(vtsExtractResults);
            //ClassicAssert.IsTrue(vtsExtractResults.All(x => x.AssignmentId == assignmentCreated.Id));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.MarketDataId == mktData.MarketDataId.Value));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.RuleId == ruleCreated.Id));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.ProviderName == input.ProviderName));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.CurveName == input.MarketDataName));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.RuleName == rulePayload.Name));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.Version == checkedVersion.ToDateTimeUnspecified()));
            ClassicAssert.IsTrue(vtsExtractResults.All(x => x.CompetenceStart <= x.CompetenceEnd));
            ClassicAssert.IsTrue(vtsExtractResults.Any(x => x.IssueCount > 0));

            await marketDataService.DeleteDataQualityRuleAssignmentAsync(assignmentCreated.Id, ctk);
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);
            await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task QualityNotificationAlertCrud()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;

            var alertPayload = new QualityNotificationAlertDto.Input
            {
                Name = "Quality notification alert " + Guid.NewGuid(),
                TriggerConfig = new OnEventTriggerConfigDto(),
                MailNotifications = new System.Collections.Generic.List<MailNotificationDto>
                {
                    new MailNotificationDto
                    {
                        Recipients = new[] { "test@example.com" }
                    }
                }
            };

            var alertCreated = await marketDataService.RegisterQualityNotificationAlertAsync(alertPayload, ctk);
            ClassicAssert.IsNotNull(alertCreated);
            ClassicAssert.AreEqual(alertPayload.Name, alertCreated.Name);
            ClassicAssert.AreEqual(AlertType.OnEvent, alertCreated.TriggerConfig.Type);

            var alertRead = await marketDataService.ReadQualityNotificationAlertByIdAsync(alertCreated.Id, ctk);
            ClassicAssert.IsNotNull(alertRead);
            ClassicAssert.AreEqual(alertCreated.Id, alertRead.Id);

            var alertUpdated = new QualityNotificationAlertDto.Input
            {
                Name = alertCreated.Name + " updated",
                TriggerConfig = new OnEventTriggerConfigDto(),
                MailNotifications = alertPayload.MailNotifications,
                ETag = alertCreated.ETag,
                Version = alertCreated.Version
            };

            var updatedAlert = await marketDataService.UpdateQualityNotificationAlertAsync(alertCreated.Id, alertUpdated, ctk);
            ClassicAssert.AreEqual(alertUpdated.Name, updatedAlert.Name);

            await marketDataService.DeleteQualityNotificationAlertAsync(alertCreated.Id, ctk);

            var deletedAlert = await marketDataService.ReadQualityNotificationAlertByIdAsync(alertCreated.Id, ctk);
            ClassicAssert.IsNull(deletedAlert);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task QualityNotificationAlertAssignmentCrud()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;

            var alert = await marketDataService.RegisterQualityNotificationAlertAsync(new QualityNotificationAlertDto.Input
            {
                Name = "Quality notification assignment " + Guid.NewGuid(),
                TriggerConfig = new OnEventTriggerConfigDto(),
                MailNotifications = new System.Collections.Generic.List<MailNotificationDto>
                {
                    new MailNotificationDto { Recipients = new[] { "test@example.com" } }
                }
            }, ctk);

            // Create a Daily Actual TimeSerie named TsCheckSummaryQuery from Provider DqNotificationAlert
            var input = new MarketDataEntity.Input
            {
                ProviderName = "DqNotificationAlert",
                MarketDataName = "TsCheckSummaryQuery_" + Guid.NewGuid(),
                Type = MarketDataTypeV2.ActualTimeSerie,
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    input.ProviderName,
                    input.MarketDataName)
                );

            var isRegd = await mktData.IsRegistered(ctk);

            if (!isRegd)
                await mktData.Register(input, ctk);

            await mktData.Load(ctk);

            // Write values with gaps (2025-01-01, 2025-01-03, 2025-01-05)
            var data = ((MarketData)mktData).EditActual();
            data.TryAddData(new LocalDate(2025, 1, 1), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 3), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 5), 10.0);

            await data.Save(Instant.FromUtc(2025, 1, 14, 0, 0), ctk: ctk);
            var assignment = await marketDataService.RegisterQualityNotificationAlertAssignmentAsync(
                    new QualityNotificationAlertAssignmentDto.Input
                    {
                        AlertId = alert.Id,
                        MarketDataId = mktData.MarketDataId!.Value
                    },
                    ctk);

            ClassicAssert.IsNotNull(assignment);
            ClassicAssert.AreEqual(alert.Id, assignment.AlertId);
            ClassicAssert.AreEqual(mktData.MarketDataId!.Value, assignment.MarketDataId);

            var readAssignment = await marketDataService.ReadQualityNotificationAlertAssignmentByIdAsync(assignment.Id, ctk);
            ClassicAssert.AreEqual(assignment.Id, readAssignment.Id);

            var assignments = await marketDataService.ReadQualityNotificationAlertAssignmentsAsync(
                page: 1,
                pageSize: 10,
                alertId: alert.Id,
                marketDataId: mktData.MarketDataId!.Value,
                ctk: ctk);
            ClassicAssert.IsNotNull(assignments);

            await marketDataService.DeleteQualityNotificationAlertAssignmentAsync(assignment.Id, ctk);
            ClassicAssert.IsNull(await marketDataService.ReadQualityNotificationAlertAssignmentByIdAsync(assignment.Id, ctk));

            await marketDataService.DeleteQualityNotificationAlertAsync(alert.Id, ctk);

            // Delete MarketData entity
            if (mktData.MarketDataId.HasValue)
                await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task MarketDataOverrideCrud()
        {
            var marketDataService = new MarketDataService(_cfg);
            CancellationToken ctk = default;

            var input = new MarketDataEntity.Input
            {
                ProviderName = "MarketDataOverrideSample",
                MarketDataName = "Override_" + Guid.NewGuid(),
                Type = MarketDataTypeV2.ActualTimeSerie,
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "UTC",
                AggregationRule = AggregationRule.Undefined,
            };

            var marketData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(input.ProviderName, input.MarketDataName));

            if (!await marketData.IsRegistered(ctk))
                await marketData.Register(input, ctk);

            await marketData.Load(ctk);

            var data = ((MarketData)marketData).EditActual();
            data.TryAddData(new LocalDate(2025, 1, 1), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 3), 10.0);
            data.TryAddData(new LocalDate(2025, 1, 5), 10.0);

            await data.Save(Instant.FromUtc(2025, 1, 14, 0, 0), ctk: ctk);

            data.ClearData();
            data.TryAddData(new LocalDate(2025, 1, 1), 11.5);
            data.TryAddData(new LocalDate(2025, 1, 2), 12.5);

            await data.SaveOverride(
                SystemClock.Instance.GetCurrentInstant(),
                deferDataGeneration: false,
                replaceExisting: true,
                comment: "SDK Market Data override sample",
                ctk: ctk);

            var overrideMetadata = await marketDataService.ReadOverrideMetadataAsync(
                marketData.MarketDataId!.Value,
                OverrideKind.Override,
                page: 1,
                pageSize: 10,
                ctk: ctk);

            ClassicAssert.IsNotNull(overrideMetadata);
            ClassicAssert.IsNotEmpty(overrideMetadata.Data);
            var createdOverride = overrideMetadata.Data.First();
            ClassicAssert.IsTrue(createdOverride.Id.HasValue);
            ClassicAssert.AreEqual(OverrideKind.Override, createdOverride.Kind);
            var overrideId = createdOverride.Id!.Value;

            data.ClearData();
            data.TryAddData(new LocalDate(2025, 1, 4), 13.5);
            data.TryAddData(new LocalDate(2025, 1, 5), 14.5);

            await data.SaveFallback(
                SystemClock.Instance.GetCurrentInstant(),
                deferDataGeneration: false,
                replaceExisting: true,
                comment: "SDK Market Data fallback sample",
                ctk: ctk);

            var fallbackMetadata = await marketDataService.ReadOverrideMetadataAsync(
                marketData.MarketDataId.Value,
                OverrideKind.Fallback,
                page: 1,
                pageSize: 10,
                ctk: ctk);

            ClassicAssert.IsNotNull(fallbackMetadata);
            ClassicAssert.IsNotEmpty(fallbackMetadata.Data);
            var createdFallback = fallbackMetadata.Data.First();
            ClassicAssert.IsTrue(createdFallback.Id.HasValue);
            ClassicAssert.AreEqual(OverrideKind.Fallback, createdFallback.Kind);
            var fallbackId = createdFallback.Id!.Value;

            await marketDataService.DeleteOverrideDataAsync(overrideId, ctk);
            await marketDataService.DeleteOverrideDataAsync(fallbackId, ctk);

            var deletedMetadata = await marketDataService.ReadOverrideMetadataAsync(
                marketData.MarketDataId!.Value,
                OverrideKind.Override,
                page: 1,
                pageSize: 10,
                ctk: ctk);

            ClassicAssert.IsFalse(
                deletedMetadata.Data.Any(x => x.Id == overrideId),
                "The override metadata should not be returned after deletion.");

            var deletedFallbackMetadata = await marketDataService.ReadOverrideMetadataAsync(
                marketData.MarketDataId.Value,
                OverrideKind.Fallback,
                page: 1,
                pageSize: 10,
                ctk: ctk);

            ClassicAssert.IsFalse(
                deletedFallbackMetadata.Data.Any(x => x.Id == fallbackId),
                "The fallback metadata should not be returned after deletion.");

            await marketDataService.DeleteMarketDataAsync(marketData.MarketDataId.Value, ctk);
        }
    }
}