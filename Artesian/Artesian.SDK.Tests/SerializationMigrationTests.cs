// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
#pragma warning disable MA0002 // Dictionary ContainsKey is fine without comparer in tests
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.UoM;
using Artesian.SDK.Service;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Serialization tests to ensure compatibility during migration from Newtonsoft.Json to System.Text.Json
    /// These tests validate that System.Text.Json can:
    /// 1. Deserialize JSON created by Newtonsoft.Json (backward compatibility)
    /// 2. Serialize objects to JSON that is equivalent to Newtonsoft.Json output (forward compatibility)
    /// 
    /// JSON literal strings in these tests were generated once using Newtonsoft.Json with the original
    /// master settings and are used as the expected baseline for compatibility testing.
    /// </summary>
    [TestFixture]
    public class SerializationMigrationTests
    {
        private JsonSerializerOptions _stjOptions = null!;

        [SetUp]
        public void Setup()
        {
            // Use the same STJ options as the Client
            _stjOptions = Client.CreateDefaultJsonSerializerOptions();
        }

        /// <summary>
        /// Helper to compare two JSON strings using JsonNode.DeepEquals
        /// </summary>
        private void AssertJsonEquals(string expected, string actual, string message = "JSON should be equivalent")
        {
            var expectedNode = JsonNode.Parse(expected, new JsonNodeOptions { PropertyNameCaseInsensitive = false });
            var actualNode = JsonNode.Parse(actual, new JsonNodeOptions { PropertyNameCaseInsensitive = false });
            
            Assert.That(JsonNode.DeepEquals(expectedNode, actualNode), Is.True, 
                $"{message}\nExpected: {expected}\nActual: {actual}");
        }

        #region MarketData Entity Tags Dictionary Tests

        [Test]
        public void MarketDataEntity_WithTags_STJ_CanDeserialize_NewtonsoftJson()
        {
            // Arrange - Create object and serialize with Newtonsoft
            var entity = new MarketDataEntity.Input()
            {
                MarketDataId = 100000001,
                ProviderName = "TestProvider",
                MarketDataName = "TestCurve",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.ActualTimeSerie,
                Tags = new Dictionary<string, List<string>>
                {
                    { "Region", new List<string> { "Europe", "EMEA" } },
                    { "Product", new List<string> { "Power", "Electricity" } },
                    { "Market", new List<string> { "DayAhead" } }
                }
            };
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);

            // Act - Deserialize with STJ
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<MarketDataEntity.Input>(newtonsoftJson, _stjOptions);

            // Assert - STJ successfully deserialized Newtonsoft JSON
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.MarketDataId, Is.EqualTo(100000001));
            Assert.That(deserialized.ProviderName, Is.EqualTo("TestProvider"));
            Assert.That(deserialized.Tags, Is.Not.Null);
            Assert.That(deserialized.Tags!.Count, Is.EqualTo(3));
            Assert.That(deserialized.Tags["Region"], Does.Contain("Europe"));
            Assert.That(deserialized.Tags["Product"], Does.Contain("Power"));
        }

        [Test]
        public void MarketDataEntity_WithTags_STJ_Serializes_CompatibleJson()
        {
            // Arrange
            var entity = new MarketDataEntity.Input()
            {
                MarketDataId = 100000001,
                ProviderName = "TestProvider",
                MarketDataName = "TestCurve",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.ActualTimeSerie,
                Tags = new Dictionary<string, List<string>>
                {
                    { "Region", new List<string> { "Europe", "EMEA" } },
                    { "Product", new List<string> { "Power", "Electricity" } },
                    { "Market", new List<string> { "DayAhead" } }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Both produce equivalent JSON
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should produce JSON equivalent to Newtonsoft");
        }

        [Test]
        public void MarketDataEntity_WithNullTags_STJ_SkipsInSerialization()
        {
            // Arrange
            var entity = new MarketDataEntity.Input()
            {
                MarketDataId = 100000001,
                ProviderName = "TestProvider",
                MarketDataName = "TestCurve",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.ActualTimeSerie,
                Tags = null
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Null Tags should not be present in JSON for both serializers
            Assert.That(newtonsoftJson, Does.Not.Contain("Tags"));
            Assert.That(stjJson, Does.Not.Contain("Tags"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should handle null properties same as Newtonsoft");
        }

        [Test]
        public void MarketDataEntity_Tags_STJ_PreservesDictionaryKeyCasing()
        {
            // Arrange - Use mixed case keys to ensure casing is preserved
            var entity = new MarketDataEntity.Input()
            {
                MarketDataId = 100000001,
                ProviderName = "TestProvider",
                MarketDataName = "TestCurve",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.ActualTimeSerie,
                Tags = new Dictionary<string, List<string>>
                {
                    { "RegionCode", new List<string> { "EU" } },
                    { "PRODUCT_TYPE", new List<string> { "POWER" } },
                    { "marketSegment", new List<string> { "wholesale" } }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Dictionary keys should preserve original casing in both
            Assert.That(stjJson, Does.Contain("RegionCode"));
            Assert.That(stjJson, Does.Contain("PRODUCT_TYPE"));
            Assert.That(stjJson, Does.Contain("marketSegment"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should preserve dictionary key casing like Newtonsoft");

            // Verify STJ deserialization preserves keys
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<MarketDataEntity.Input>(stjJson, _stjOptions);
            Assert.That(deserialized!.Tags!.ContainsKey("RegionCode"), Is.True);
            Assert.That(deserialized.Tags.ContainsKey("PRODUCT_TYPE"), Is.True);
            Assert.That(deserialized.Tags.ContainsKey("marketSegment"), Is.True);
        }

        #endregion

        #region TimeTransform Polymorphic Tests

        [Test]
        public void TimeTransform_SimpleShift_STJ_CanDeserialize_NewtonsoftJson()
        {
            // Arrange - Create and serialize with Newtonsoft
            var transform = new TimeTransformSimpleShift
            {
                ID = 1,
                Name = "SimpleShift1",
                ETag = Guid.NewGuid(),
                DefinedBy = TransformDefinitionType.User,
                Period = Granularity.Day
            };
            var newtonsoftJson = JsonConvert.SerializeObject(transform, _newtonsoftSettings);

            // Act - Deserialize with STJ
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<TimeTransform>(newtonsoftJson, _stjOptions);

            // Assert - STJ correctly deserialized polymorphic type
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<TimeTransformSimpleShift>());
            var shift = (TimeTransformSimpleShift)deserialized!;
            Assert.That(shift.Type, Is.EqualTo(TransformType.SimpleShift));
            Assert.That(shift.Period, Is.EqualTo(Granularity.Day));
            Assert.That(shift.Name, Is.EqualTo("SimpleShift1"));
        }

        [Test]
        public void TimeTransform_SimpleShift_STJ_Serializes_CompatibleJson()
        {
            // Arrange
            var transform = new TimeTransformSimpleShift
            {
                ID = 1,
                Name = "SimpleShift1",
                ETag = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                DefinedBy = TransformDefinitionType.User,
                Period = Granularity.Day
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(transform, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(transform, _stjOptions);

            // Assert - Both produce equivalent JSON including Type discriminator
            Assert.That(stjJson, Does.Contain("\"Type\""));
            Assert.That(stjJson, Does.Contain("SimpleShift"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize polymorphic types like Newtonsoft");
        }

        [Test]
        public void TimeTransform_BaseClass_STJ_Deserializes_ToCorrectType()
        {
            // Arrange - Serialize with Newtonsoft
            var transform = new TimeTransformSimpleShift
            {
                ID = 2,
                Name = "TestShift",
                ETag = Guid.NewGuid(),
                DefinedBy = TransformDefinitionType.System,
                Period = Granularity.Hour
            };
            var newtonsoftJson = JsonConvert.SerializeObject(transform, _newtonsoftSettings);

            // Act - Deserialize as base type with STJ
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<TimeTransform>(newtonsoftJson, _stjOptions);

            // Assert - STJ polymorphic deserialization works
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<TimeTransformSimpleShift>());
            var shift = (TimeTransformSimpleShift)deserialized!;
            Assert.That(shift.Type, Is.EqualTo(TransformType.SimpleShift));
            Assert.That(shift.Name, Is.EqualTo("TestShift"));
        }

        #endregion

        #region DerivedCfg Polymorphic Tests

        [Test]
        public void DerivedCfg_MUV_STJ_CanDeserialize_NewtonsoftJson()
        {
            // Arrange - Serialize with Newtonsoft
            var cfg = new DerivedCfgMuv
            {
                Version = 1
            };
            var newtonsoftJson = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);

            // Act - Deserialize with STJ
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DerivedCfgBase>(newtonsoftJson, _stjOptions);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgMuv>());
            Assert.That(((DerivedCfgMuv)deserialized!).Version, Is.EqualTo(1));
            Assert.That(deserialized.DerivedAlgorithm, Is.EqualTo(Dto.Enums.DerivedAlgorithm.MUV));
        }

        [Test]
        public void DerivedCfg_MUV_STJ_Serializes_CompatibleJson()
        {
            // Arrange
            var cfg = new DerivedCfgMuv
            {
                Version = 1
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("MUV"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize DerivedCfg like Newtonsoft");
        }

        [Test]
        public void DerivedCfg_Sum_STJ_Serializes_Correctly()
        {
            // Arrange
            var cfg = new DerivedCfgSum
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("Sum"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize DerivedCfgSum like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DerivedCfgBase>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgSum>());
            var sum = (DerivedCfgSum)deserialized!;
            Assert.That(sum.OrderedReferencedMarketDataIds, Has.Length.EqualTo(2));
        }

        [Test]
        public void DerivedCfg_Coalesce_STJ_Serializes_Correctly()
        {
            // Arrange
            var cfg = new DerivedCfgCoalesce
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("Coalesce"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize DerivedCfgCoalesce like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DerivedCfgBase>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgCoalesce>());
        }

        #endregion

        #region UpsertData Dictionary Properties Tests

        [Test]
        public void UpsertCurveData_Rows_STJ_CanDeserialize_NewtonsoftJson()
        {
            // Arrange - Serialize with Newtonsoft
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                Rows = new Dictionary<LocalDateTime, double?>
                {
                    { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 },
                    { new LocalDateTime(2024, 1, 2, 0, 0), 101.2 },
                    { new LocalDateTime(2024, 1, 3, 0, 0), null }
                }
            };
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Act - Deserialize with STJ
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(newtonsoftJson, _stjOptions);

            // Assert - STJ can deserialize dictionary with LocalDateTime keys
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Rows, Is.Not.Null);
            Assert.That(deserialized.Rows!.Count, Is.EqualTo(3));
            Assert.That(deserialized.Rows[new LocalDateTime(2024, 1, 1, 0, 0)], Is.EqualTo(100.5));
            Assert.That(deserialized.Rows[new LocalDateTime(2024, 1, 3, 0, 0)], Is.Null);
        }

        [Test]
        public void UpsertCurveData_Rows_STJ_Serializes_CompatibleJson()
        {
            // Arrange
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                Rows = new Dictionary<LocalDateTime, double?>
                {
                    { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 },
                    { new LocalDateTime(2024, 1, 2, 0, 0), 101.2 }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert - Dictionary with complex keys serialized the same way
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize Rows dictionary like Newtonsoft");
        }

        [Test]
        public void UpsertCurveData_MarketAssessment_STJ_Serializes_Correctly()
        {
            // Arrange
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>
                {
                    {
                        new LocalDateTime(2024, 1, 1, 0, 0),
                        new Dictionary<string, MarketAssessmentValue>
                        {
                            { "Product1", new MarketAssessmentValue { Settlement = 100.0, Open = 99.0, Close = 101.0, High = 102.0, Low = 98.0 } },
                            { "Product2", new MarketAssessmentValue { Settlement = 200.0, Open = 199.0, Close = 201.0, High = 202.0, Low = 198.0 } }
                        }
                    }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize MarketAssessment like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.MarketAssessment, Is.Not.Null);
            Assert.That(deserialized.MarketAssessment!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_AuctionRows_STJ_Serializes_Correctly()
        {
            // Arrange
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                AuctionRows = new Dictionary<LocalDateTime, AuctionBids>
                {
                    {
                        new LocalDateTime(2024, 1, 1, 0, 0),
                        new AuctionBids
                        {
                            BidTimestamp = new LocalDateTime(2024, 1, 1, 0, 0),
                            Bid = new AuctionBidValue[] 
                            {
                                new AuctionBidValue(50.0, 100),
                                new AuctionBidValue(51.0, 200)
                            },
                            Offer = new AuctionBidValue[]
                            {
                                new AuctionBidValue(52.0, 150)
                            }
                        }
                    }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize AuctionRows like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.AuctionRows, Is.Not.Null);
            Assert.That(deserialized.AuctionRows!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_BidAsk_STJ_Serializes_Correctly()
        {
            // Arrange
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                BidAsk = new Dictionary<LocalDateTime, IDictionary<string, BidAskValue>>
                {
                    {
                        new LocalDateTime(2024, 1, 1, 0, 0),
                        new Dictionary<string, BidAskValue>
                        {
                            { "Product1", new BidAskValue { BestBidPrice = 99.0, BestAskPrice = 101.0, BestBidQuantity = 100, BestAskQuantity = 150 } },
                            { "Product2", new BidAskValue { BestBidPrice = 199.0, BestAskPrice = 201.0, BestBidQuantity = 200, BestAskQuantity = 250 } }
                        }
                    }
                }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should serialize BidAsk like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.BidAsk, Is.Not.Null);
            Assert.That(deserialized.BidAsk!.Count, Is.EqualTo(1));
        }

        #endregion

        #region Null Handling Tests

        [Test]
        public void STJ_Serialization_SkipsNullProperties()
        {
            // Arrange
            var entity = new MarketDataEntity.Input()
            {
                MarketDataId = 100000001,
                ProviderName = "TestProvider",
                MarketDataName = "TestCurve",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.ActualTimeSerie,
                Tags = null,
                ProviderDescription = null,
                TransformID = null
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Null properties should not be present in both
            Assert.That(stjJson, Does.Not.Contain("Tags"));
            Assert.That(stjJson, Does.Not.Contain("ProviderDescription"));
            Assert.That(stjJson, Does.Not.Contain("TransformID"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should skip null properties like Newtonsoft");
        }

        [Test]
        public void UpsertCurveData_STJ_NullDictionaries_SkippedInSerialization()
        {
            // Arrange
            var upsertData = new UpsertCurveData
            {
                ID = new MarketDataIdentifier("Provider", "Curve"),
                Timezone = "CET",
                DownloadedAt = Instant.FromUtc(2024, 1, 1, 12, 0),
                Rows = new Dictionary<LocalDateTime, double?>
                {
                    { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 }
                },
                MarketAssessment = null,
                AuctionRows = null,
                BidAsk = null
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert - Null dictionaries should be skipped in both
            Assert.That(stjJson, Does.Contain("Rows"));
            Assert.That(stjJson, Does.Not.Contain("MarketAssessment"));
            Assert.That(stjJson, Does.Not.Contain("AuctionRows"));
            Assert.That(stjJson, Does.Not.Contain("BidAsk"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should skip null dictionaries like Newtonsoft");
        }

        #endregion

        #region Dictionary Key Format Tests

        [Test]
        public void Dictionary_WithComplexKey_STJ_SerializesCompatibly()
        {
            // Arrange - LocalDateTime as dictionary key
            var rows = new Dictionary<LocalDateTime, double?>
            {
                { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 },
                { new LocalDateTime(2024, 1, 2, 12, 30), 101.2 }
            };

            // Act - Serialize with both
            var newtonsoftJson = JsonConvert.SerializeObject(rows, _newtonsoftSettings);
            var stjJson = System.Text.Json.JsonSerializer.Serialize(rows, _stjOptions);

            // Assert - Should use Key/Value format in both
            Assert.That(stjJson, Does.Contain("Key"));
            Assert.That(stjJson, Does.Contain("Value"));
            AssertJsonEquals(newtonsoftJson, stjJson, "STJ should use same Key/Value format as Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<Dictionary<LocalDateTime, double?>>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Count, Is.EqualTo(2));
            Assert.That(deserialized[new LocalDateTime(2024, 1, 1, 0, 0)], Is.EqualTo(100.5));
        }

        #endregion
    }
}
