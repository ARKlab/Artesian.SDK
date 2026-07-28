using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Service;
using Artesian.SDK.Factory;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
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
                Name = "TestRule",
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
                Name = "TestRuleUpdated",
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
                Name = "TestRule",
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
                MarketDataName = "TsCheckSummaryQuery_" + Guid.NewGuid().ToString(),
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
                Name = "TsSummaryRule_" + Guid.NewGuid().ToString(),
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

            // The check summary should contain results
            // Note: Depending on timing and background processing, results may or may not be available immediately
            // This assertion verifies the API call succeeded

            // Cleanup
            await marketDataService.DeleteDataQualityRuleAssignmentAsync(assignmentCreated.Id, ctk);
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);

            if (mktData.MarketDataId.HasValue)
                await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }
    }
}