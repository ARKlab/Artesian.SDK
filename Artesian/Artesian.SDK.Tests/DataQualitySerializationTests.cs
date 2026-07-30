// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.Text.Json;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DataQuality;
using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Service;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Tests for System.Text.Json round-trip serialization/deserialization of Data Quality DTOs.
    /// These tests ensure polymorphic discriminator-based converters work correctly and prevent
    /// silent breaking changes in the serialization behavior.
    /// </summary>
    [TestFixture]
    public class DataQualitySerializationTests
    {
        private JsonSerializerOptions _stjOptions = null!;

        [SetUp]
        public void Setup()
        {
            // Use the same STJ options as the Client
            _stjOptions = Client.CreateDefaultJsonSerializerOptions();
        }

        #region DataQualityRuleConfigDto Polymorphic Tests (RuleType discriminator)

        [Test]
        public void DataQualityRuleConfig_CompletenessAndFreshness_STJ_RoundTrip()
        {
            // Arrange
            var config = new ActualCompletenessAndFreshnessConfigDto
            {
                MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                ScheduleConfig = new ScheduleConfigDto
                {
                    ScheduleDefinition = new CronScheduleDefinitionDto
                    {
                        CronExpression = "0 9 * * *",
                        TimeZone = "UTC"
                    },
                    MaxDelay = Period.FromHours(2)
                },
                RecordValidationConfig = new RecordValidationConfigDto
                {
                    RecordRangeFrom = Period.FromDays(-1),
                    RecordRangeTo = Period.FromDays(0)
                }
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<DataQualityRuleConfigDto>(config, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<ActualCompletenessAndFreshnessConfigDto>());
            Assert.That(deserialized!.Type, Is.EqualTo(RuleType.CompletenessAndFreshness));

            var actualConfig = (ActualCompletenessAndFreshnessConfigDto)deserialized;
            Assert.That(actualConfig.MarketDataType, Is.EqualTo(MarketDataTypeV2.ActualTimeSerie));
            Assert.That(actualConfig.ScheduleConfig.MaxDelay, Is.EqualTo(Period.FromHours(2)));
            Assert.That(actualConfig.RecordValidationConfig.RecordRangeFrom, Is.EqualTo(Period.FromDays(-1)));
        }

        [Test]
        public void DataQualityRuleConfig_Outlier_STJ_RoundTrip()
        {
            // Arrange
            var config = new OutlierConfigDto
            {
                Model = new OutlierAbsoluteBoundConfigDto
                {
                    LowerBound = -10.0,
                    UpperBound = 45.0
                }
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<DataQualityRuleConfigDto>(config, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<OutlierConfigDto>());
            Assert.That(deserialized!.Type, Is.EqualTo(RuleType.Outlier));

            var outlierConfig = (OutlierConfigDto)deserialized;
            Assert.That(outlierConfig.Model, Is.InstanceOf<OutlierAbsoluteBoundConfigDto>());
            var boundConfig = (OutlierAbsoluteBoundConfigDto)outlierConfig.Model;
            Assert.That(boundConfig.LowerBound, Is.EqualTo(-10.0));
            Assert.That(boundConfig.UpperBound, Is.EqualTo(45.0));
        }

        [Test]
        public void DataQualityRuleConfig_InvalidRuleType_STJ_ThrowsException()
        {
            // Arrange - JSON with invalid RuleType value
            const string invalidJson = @"{""Type"":999,""Configuration"":{}}";

            // Act & Assert - System.Text.Json wraps the InvalidOperationException in JsonException
            Assert.Throws<JsonException>(() =>
                System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleConfigDto>(invalidJson, _stjOptions));
        }

        #endregion

        #region CompletenessAndFreshnessConfigDto Polymorphic Tests (MarketDataTypeV2 discriminator)

        [Test]
        public void CompletenessAndFreshnessConfig_Actual_STJ_RoundTrip()
        {
            // Arrange
            var config = new ActualCompletenessAndFreshnessConfigDto
            {
                MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                ScheduleConfig = new ScheduleConfigDto
                {
                    ScheduleDefinition = new CronScheduleDefinitionDto
                    {
                        CronExpression = "0 * * * *",
                        TimeZone = "Europe/Rome"
                    },
                    MaxDelay = Period.FromMinutes(30)
                },
                RecordValidationConfig = new RecordValidationConfigDto
                {
                    RecordRangeFrom = Period.FromHours(-24),
                    RecordRangeTo = Period.FromHours(0)
                }
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<CompletenessAndFreshnessConfigDto>(config, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<CompletenessAndFreshnessConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<ActualCompletenessAndFreshnessConfigDto>());
            Assert.That(deserialized!.MarketDataType, Is.EqualTo(MarketDataTypeV2.ActualTimeSerie));
            Assert.That(json, Does.Contain("ActualTimeSerie"));
        }

        [Test]
        public void CompletenessAndFreshnessConfig_Versioned_STJ_RoundTrip()
        {
            // Arrange
            var config = new VersionedCompletenessAndFreshnessConfigDto
            {
                MarketDataType = MarketDataTypeV2.VersionedTimeSerie,
                ScheduleConfig = new ScheduleConfigDto
                {
                    ScheduleDefinition = new CronScheduleDefinitionDto
                    {
                        CronExpression = "15 * * * *",
                        TimeZone = "UTC"
                    },
                    MaxDelay = Period.FromMinutes(45)
                },
                RecordValidationConfig = new RecordValidationConfigDto
                {
                    RecordRangeFrom = Period.FromDays(0),
                    RecordRangeTo = Period.FromDays(7)
                },
                VersionToleranceFrom = Period.FromHours(-1),
                VersionToleranceTo = Period.FromHours(1),
                VersionPrecision = PeriodPrecision.Hour
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<CompletenessAndFreshnessConfigDto>(config, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<CompletenessAndFreshnessConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<VersionedCompletenessAndFreshnessConfigDto>());
            Assert.That(deserialized!.MarketDataType, Is.EqualTo(MarketDataTypeV2.VersionedTimeSerie));

            var versionedConfig = (VersionedCompletenessAndFreshnessConfigDto)deserialized;
            Assert.That(versionedConfig.VersionToleranceFrom, Is.EqualTo(Period.FromHours(-1)));
            Assert.That(versionedConfig.VersionToleranceTo, Is.EqualTo(Period.FromHours(1)));
            Assert.That(versionedConfig.VersionPrecision, Is.EqualTo(PeriodPrecision.Hour));
            Assert.That(json, Does.Contain("VersionedTimeSerie"));
        }

        [Test]
        public void CompletenessAndFreshnessConfig_InvalidMarketDataType_STJ_ThrowsException()
        {
            // Arrange - JSON with invalid MarketDataType for completeness rule
            const string invalidJson = @"{""MarketDataType"":""BidAsk"",""ScheduleConfig"":{},""RecordValidationConfig"":{}}";

            // Act & Assert - Converter throws InvalidOperationException directly (not wrapped by JsonException)
            var ex = Assert.Throws<InvalidOperationException>(() =>
                System.Text.Json.JsonSerializer.Deserialize<CompletenessAndFreshnessConfigDto>(invalidJson, _stjOptions));

            Assert.That(ex!.Message, Does.Contain("CompletenessAndFreshnessConfigDto"));
            Assert.That(ex.Message, Does.Contain("MarketDataTypeV2"));
        }

        #endregion

        #region OutlierModelConfigDto Polymorphic Tests (OutlierModel discriminator)

        [Test]
        public void OutlierModelConfig_AbsoluteBound_STJ_RoundTrip()
        {
            // Arrange
            var model = new OutlierAbsoluteBoundConfigDto
            {
                LowerBound = -50.5,
                UpperBound = 100.75
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<OutlierModelConfigDto>(model, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<OutlierModelConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<OutlierAbsoluteBoundConfigDto>());
            Assert.That(deserialized!.Model, Is.EqualTo(OutlierModel.AbsoluteBound));

            var boundModel = (OutlierAbsoluteBoundConfigDto)deserialized;
            Assert.That(boundModel.LowerBound, Is.EqualTo(-50.5));
            Assert.That(boundModel.UpperBound, Is.EqualTo(100.75));
            Assert.That(json, Does.Contain("AbsoluteBound"));
        }

        [Test]
        public void OutlierModelConfig_RefCurve_STJ_RoundTrip()
        {
            // Arrange
            var model = new OutlierRefCurveConfigDto
            {
                ReferenceMarketDataId = 100000001,
                TolerancePerc = 0.05  // 5%
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<OutlierModelConfigDto>(model, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<OutlierModelConfigDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<OutlierRefCurveConfigDto>());
            Assert.That(deserialized!.Model, Is.EqualTo(OutlierModel.RefCurve));

            var refCurveModel = (OutlierRefCurveConfigDto)deserialized;
            Assert.That(refCurveModel.ReferenceMarketDataId, Is.EqualTo(100000001));
            Assert.That(refCurveModel.TolerancePerc, Is.EqualTo(0.05));
            Assert.That(json, Does.Contain("RefCurve"));
        }

        [Test]
        public void OutlierModelConfig_InvalidOutlierModel_STJ_ThrowsException()
        {
            // Arrange - JSON with invalid OutlierModel value
            const string invalidJson = @"{""Model"":999,""LowerBound"":-10,""UpperBound"":10}";

            // Act & Assert - System.Text.Json wraps the InvalidOperationException in JsonException
            Assert.Throws<JsonException>(() =>
                System.Text.Json.JsonSerializer.Deserialize<OutlierModelConfigDto>(invalidJson, _stjOptions));
        }

        #endregion

        #region ScheduleDefinitionDto Polymorphic Tests (ScheduleDefinitionType discriminator)

        [Test]
        public void ScheduleDefinition_Cron_STJ_RoundTrip()
        {
            // Arrange
            var scheduleDefinition = new CronScheduleDefinitionDto
            {
                CronExpression = "0 8 * * MON-FRI",
                TimeZone = "America/New_York"
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize<ScheduleDefinitionDto>(scheduleDefinition, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<ScheduleDefinitionDto>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<CronScheduleDefinitionDto>());
            Assert.That(deserialized!.Type, Is.EqualTo(ScheduleDefinitionType.Cron));

            var cronSchedule = (CronScheduleDefinitionDto)deserialized;
            Assert.That(cronSchedule.CronExpression, Is.EqualTo("0 8 * * MON-FRI"));
            Assert.That(cronSchedule.TimeZone, Is.EqualTo("America/New_York"));
            Assert.That(json, Does.Contain("Cron"));
        }

        [Test]
        public void ScheduleDefinition_InvalidType_STJ_ThrowsException()
        {
            // Arrange - JSON with invalid ScheduleDefinitionType
            const string invalidJson = @"{""Type"":999,""CronExpression"":""0 * * * *""}";

            // Act & Assert - System.Text.Json wraps the InvalidOperationException in JsonException
            Assert.Throws<JsonException>(() =>
                System.Text.Json.JsonSerializer.Deserialize<ScheduleDefinitionDto>(invalidJson, _stjOptions));
        }

        #endregion

        #region Full DataQualityRuleDto Integration Tests

        [Test]
        public void DataQualityRuleDto_CompleteActualRule_STJ_RoundTrip()
        {
            // Arrange - Complete rule with all nested polymorphic types
            var rule = new DataQualityRuleDto.Input
            {
                Id = 123,
                Name = "Test Rule",
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new ActualCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.ActualTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "0 9 * * *",
                            TimeZone = "UTC"
                        },
                        MaxDelay = Period.FromHours(2)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.FromDays(-1),
                        RecordRangeTo = Period.FromDays(0),
                        Precision = PeriodPrecision.Day
                    }
                },
                Version = 1,
                ETag = "test-etag"
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize(rule, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleDto.Input>(json, _stjOptions);

            // Assert - Verify all levels of polymorphism
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Name, Is.EqualTo("Test Rule"));
            Assert.That(deserialized.Type, Is.EqualTo(RuleType.CompletenessAndFreshness));
            Assert.That(deserialized.Configuration, Is.InstanceOf<ActualCompletenessAndFreshnessConfigDto>());

            var config = (ActualCompletenessAndFreshnessConfigDto)deserialized.Configuration;
            Assert.That(config.ScheduleConfig.ScheduleDefinition, Is.InstanceOf<CronScheduleDefinitionDto>());

            var cronSchedule = (CronScheduleDefinitionDto)config.ScheduleConfig.ScheduleDefinition;
            Assert.That(cronSchedule.CronExpression, Is.EqualTo("0 9 * * *"));
        }

        [Test]
        public void DataQualityRuleDto_CompleteVersionedRule_STJ_RoundTrip()
        {
            // Arrange - Versioned rule with version tolerance
            var rule = new DataQualityRuleDto.Input
            {
                Id = 456,
                Name = "Versioned Rule",
                Type = RuleType.CompletenessAndFreshness,
                Configuration = new VersionedCompletenessAndFreshnessConfigDto
                {
                    MarketDataType = MarketDataTypeV2.VersionedTimeSerie,
                    ScheduleConfig = new ScheduleConfigDto
                    {
                        ScheduleDefinition = new CronScheduleDefinitionDto
                        {
                            CronExpression = "15 * * * *",
                            TimeZone = "Europe/Rome"
                        },
                        MaxDelay = Period.FromMinutes(30)
                    },
                    RecordValidationConfig = new RecordValidationConfigDto
                    {
                        RecordRangeFrom = Period.FromDays(0),
                        RecordRangeTo = Period.FromDays(7)
                    },
                    VersionToleranceFrom = Period.FromHours(-2),
                    VersionToleranceTo = Period.FromHours(2),
                    VersionPrecision = PeriodPrecision.Hour
                },
                Version = 2
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize(rule, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleDto.Input>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Configuration, Is.InstanceOf<VersionedCompletenessAndFreshnessConfigDto>());

            var config = (VersionedCompletenessAndFreshnessConfigDto)deserialized.Configuration;
            Assert.That(config.VersionToleranceFrom, Is.EqualTo(Period.FromHours(-2)));
            Assert.That(config.VersionToleranceTo, Is.EqualTo(Period.FromHours(2)));
            Assert.That(config.VersionPrecision, Is.EqualTo(PeriodPrecision.Hour));
        }

        [Test]
        public void DataQualityRuleDto_OutlierRule_STJ_RoundTrip()
        {
            // Arrange - Outlier rule
            var rule = new DataQualityRuleDto.Input
            {
                Id = 789,
                Name = "Temperature Outlier Rule",
                Type = RuleType.Outlier,
                Configuration = new OutlierConfigDto
                {
                    Model = new OutlierAbsoluteBoundConfigDto
                    {
                        LowerBound = -20.0,
                        UpperBound = 50.0
                    }
                },
                Version = 1
            };

            // Act - Serialize and deserialize
            var json = System.Text.Json.JsonSerializer.Serialize(rule, _stjOptions);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DataQualityRuleDto.Input>(json, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Type, Is.EqualTo(RuleType.Outlier));
            Assert.That(deserialized.Configuration, Is.InstanceOf<OutlierConfigDto>());

            var config = (OutlierConfigDto)deserialized.Configuration;
            Assert.That(config.Model, Is.InstanceOf<OutlierAbsoluteBoundConfigDto>());
        }

        #endregion
    }
}
