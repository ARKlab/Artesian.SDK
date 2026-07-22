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
            var _rulePayload = new DataQualityRuleDto.Input
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
            var ruleCreated = await marketDataService.RegisterDataQualityRuleAsync(_rulePayload, ctk);

            var readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.AreEqual(readDataQualityRule.Id, ruleCreated.Id);

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

            ClassicAssert.AreEqual(readDataQualityRule.Name, "TestRuleUpdated");

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
                Type = MarketDataType.ActualTimeSerie,
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

            var dataQualityRuleAssignement = await marketDataService.ReadDataQualityRuleAssignmentByIdAsync(assignmentCreated.Id, ctk);

            ClassicAssert.AreEqual(dataQualityRuleAssignement.Id, assignmentCreated.Id);

            // Delete assignement
            await marketDataService.DeleteDataQualityRuleAssignmentAsync(assignmentCreated.Id, ctk);

            dataQualityRuleAssignement = await marketDataService.ReadDataQualityRuleAssignmentByIdAsync(assignmentCreated.Id, ctk);

            ClassicAssert.IsNull(dataQualityRuleAssignement);

            // Delete rule
            await marketDataService.DeleteDataQualityRuleAsync(ruleCreated.Id, ctk);

            readDataQualityRule = await marketDataService.ReadDataQualityRuleByIdAsync(ruleCreated.Id, ctk);

            ClassicAssert.IsNull(readDataQualityRule);

            // Delete MarketData entity
            if (mktData.MarketDataId.HasValue)
                await marketDataService.DeleteMarketDataAsync(mktData.MarketDataId.Value, ctk);
        }
    }
}