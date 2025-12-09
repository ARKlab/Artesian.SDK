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
            // Arrange - JSON literal generated once with Newtonsoft.Json using original master settings
            const string newtonsoftJson = @"{""MarketDataId"":100000001,""ProviderName"":""TestProvider"",""MarketDataName"":""TestCurve"",""OriginalGranularity"":""Day"",""Type"":""ActualTimeSerie"",""OriginalTimezone"":""CET"",""AggregationRule"":""Undefined"",""Tags"":[{""Key"":""Region"",""Value"":[""Europe"",""EMEA""]},{""Key"":""Product"",""Value"":[""Power"",""Electricity""]},{""Key"":""Market"",""Value"":[""DayAhead""]}],""Path"":""/marketdata/system/TestProvider/TestCurve"",""DerivedCfg"":{""DerivedAlgorithm"":""MUV"",""Version"":1},""UnitOfMeasure"":{}}";

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""MarketDataId"":100000001,""ProviderName"":""TestProvider"",""MarketDataName"":""TestCurve"",""OriginalGranularity"":""Day"",""Type"":""ActualTimeSerie"",""OriginalTimezone"":""CET"",""AggregationRule"":""Undefined"",""Tags"":[{""Key"":""Region"",""Value"":[""Europe"",""EMEA""]},{""Key"":""Product"",""Value"":[""Power"",""Electricity""]},{""Key"":""Market"",""Value"":[""DayAhead""]}],""Path"":""/marketdata/system/TestProvider/TestCurve"",""DerivedCfg"":{""DerivedAlgorithm"":""MUV"",""Version"":1},""UnitOfMeasure"":{}}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - STJ produces equivalent JSON to Newtonsoft
            AssertJsonEquals(expectedJson, stjJson, "STJ should produce JSON equivalent to Newtonsoft");
        }

        [Test]
        public void MarketDataEntity_WithNullTags_STJ_SkipsInSerialization()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""MarketDataId"":100000001,""ProviderName"":""TestProvider"",""MarketDataName"":""TestCurve"",""OriginalGranularity"":""Day"",""Type"":""ActualTimeSerie"",""OriginalTimezone"":""CET"",""AggregationRule"":""Undefined"",""Path"":""/marketdata/system/TestProvider/TestCurve"",""DerivedCfg"":{""DerivedAlgorithm"":""MUV"",""Version"":1},""UnitOfMeasure"":{}}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Null Tags should not be present in JSON
            Assert.That(stjJson, Does.Not.Contain("Tags"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should handle null properties same as Newtonsoft");
        }

        [Test]
        public void MarketDataEntity_Tags_STJ_PreservesDictionaryKeyCasing()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""MarketDataId"":100000001,""ProviderName"":""TestProvider"",""MarketDataName"":""TestCurve"",""OriginalGranularity"":""Day"",""Type"":""ActualTimeSerie"",""OriginalTimezone"":""CET"",""AggregationRule"":""Undefined"",""Tags"":[{""Key"":""RegionCode"",""Value"":[""EU""]},{""Key"":""PRODUCT_TYPE"",""Value"":[""POWER""]},{""Key"":""marketSegment"",""Value"":[""wholesale""]}],""Path"":""/marketdata/system/TestProvider/TestCurve"",""DerivedCfg"":{""DerivedAlgorithm"":""MUV"",""Version"":1},""UnitOfMeasure"":{}}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Dictionary keys should preserve original casing
            Assert.That(stjJson, Does.Contain("RegionCode"));
            Assert.That(stjJson, Does.Contain("PRODUCT_TYPE"));
            Assert.That(stjJson, Does.Contain("marketSegment"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should preserve dictionary key casing like Newtonsoft");

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
            // Arrange - JSON literal generated once with Newtonsoft.Json
            const string newtonsoftJson = @"{""Period"":""Day"",""Type"":""SimpleShift"",""ID"":1,""Name"":""SimpleShift1"",""ETag"":""12345678-1234-1234-1234-123456789012"",""DefinedBy"":""User""}";

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""Period"":""Day"",""Type"":""SimpleShift"",""ID"":1,""Name"":""SimpleShift1"",""ETag"":""12345678-1234-1234-1234-123456789012"",""DefinedBy"":""User""}";
            
            var transform = new TimeTransformSimpleShift
            {
                ID = 1,
                Name = "SimpleShift1",
                ETag = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                DefinedBy = TransformDefinitionType.User,
                Period = Granularity.Day
            };

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(transform, _stjOptions);

            // Assert - Both produce equivalent JSON including Type discriminator
            Assert.That(stjJson, Does.Contain("\"Type\""));
            Assert.That(stjJson, Does.Contain("SimpleShift"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize polymorphic types like Newtonsoft");
        }

        [Test]
        public void TimeTransform_BaseClass_STJ_Deserializes_ToCorrectType()
        {
            // Arrange - JSON literal from Newtonsoft.Json
            const string newtonsoftJson = @"{""Period"":""Hour"",""Type"":""SimpleShift"",""ID"":2,""Name"":""TestShift"",""ETag"":""87654321-4321-4321-4321-210987654321"",""DefinedBy"":""System""}";

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
            // Arrange - JSON literal from Newtonsoft.Json
            const string newtonsoftJson = @"{""DerivedAlgorithm"":""MUV"",""Version"":1}";

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""DerivedAlgorithm"":""MUV"",""Version"":1}";
            
            var cfg = new DerivedCfgMuv
            {
                Version = 1
            };

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("MUV"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize DerivedCfg like Newtonsoft");
        }

        [Test]
        public void DerivedCfg_Sum_STJ_Serializes_Correctly()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""DerivedAlgorithm"":""Sum"",""OrderedReferencedMarketDataIds"":[100000001,100000002],""Version"":0}";
            
            var cfg = new DerivedCfgSum
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("Sum"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize DerivedCfgSum like Newtonsoft");

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""DerivedAlgorithm"":""Coalesce"",""OrderedReferencedMarketDataIds"":[100000001,100000002],""Version"":0}";
            
            var cfg = new DerivedCfgCoalesce
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(cfg, _stjOptions);

            // Assert
            Assert.That(stjJson, Does.Contain("DerivedAlgorithm"));
            Assert.That(stjJson, Does.Contain("Coalesce"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize DerivedCfgCoalesce like Newtonsoft");

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
            // Arrange - JSON literal from Newtonsoft.Json
            const string newtonsoftJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""Rows"":[{""Key"":""2024-01-01T00:00:00"",""Value"":100.5},{""Key"":""2024-01-02T00:00:00"",""Value"":101.2},{""Key"":""2024-01-03T00:00:00"",""Value"":null}],""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false}";

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""Rows"":[{""Key"":""2024-01-01T00:00:00"",""Value"":100.5},{""Key"":""2024-01-02T00:00:00"",""Value"":101.2}],""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert - Dictionary with complex keys serialized the same way
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize Rows dictionary like Newtonsoft");
        }

        [Test]
        public void UpsertCurveData_MarketAssessment_STJ_Serializes_Correctly()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""MarketAssessment"":[{""Key"":""2024-01-01T00:00:00"",""Value"":[{""Key"":""Product1"",""Value"":{""Settlement"":100.0,""Open"":99.0,""Close"":101.0,""High"":102.0,""Low"":98.0}},{""Key"":""Product2"",""Value"":{""Settlement"":200.0,""Open"":199.0,""Close"":201.0,""High"":202.0,""Low"":198.0}}]}],""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize MarketAssessment like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.MarketAssessment, Is.Not.Null);
            Assert.That(deserialized.MarketAssessment!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_AuctionRows_STJ_Serializes_Correctly()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false,""AuctionRows"":[{""Key"":""2024-01-01T00:00:00"",""Value"":{""BidTimestamp"":""2024-01-01T00:00:00"",""Bid"":[{""Price"":50.0,""Quantity"":100.0},{""Price"":51.0,""Quantity"":200.0}],""Offer"":[{""Price"":52.0,""Quantity"":150.0}]}}]}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize AuctionRows like Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<UpsertCurveData>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.AuctionRows, Is.Not.Null);
            Assert.That(deserialized.AuctionRows!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_BidAsk_STJ_Serializes_Correctly()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false,""BidAsk"":[{""Key"":""2024-01-01T00:00:00"",""Value"":[{""Key"":""Product1"",""Value"":{""BestBidPrice"":99.0,""BestAskPrice"":101.0,""BestBidQuantity"":100.0,""BestAskQuantity"":150.0}},{""Key"":""Product2"",""Value"":{""BestBidPrice"":199.0,""BestAskPrice"":201.0,""BestBidQuantity"":200.0,""BestAskQuantity"":250.0}}]}]}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert
            AssertJsonEquals(expectedJson, stjJson, "STJ should serialize BidAsk like Newtonsoft");

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
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""MarketDataId"":100000001,""ProviderName"":""TestProvider"",""MarketDataName"":""TestCurve"",""OriginalGranularity"":""Day"",""Type"":""ActualTimeSerie"",""OriginalTimezone"":""CET"",""AggregationRule"":""Undefined"",""Path"":""/marketdata/system/TestProvider/TestCurve"",""DerivedCfg"":{""DerivedAlgorithm"":""MUV"",""Version"":1},""UnitOfMeasure"":{}}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(entity, _stjOptions);

            // Assert - Null properties should not be present
            Assert.That(stjJson, Does.Not.Contain("Tags"));
            Assert.That(stjJson, Does.Not.Contain("ProviderDescription"));
            Assert.That(stjJson, Does.Not.Contain("TransformID"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should skip null properties like Newtonsoft");
        }

        [Test]
        public void UpsertCurveData_STJ_NullDictionaries_SkippedInSerialization()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"{""ID"":{""Provider"":""Provider"",""Name"":""Curve""},""Timezone"":""CET"",""DownloadedAt"":""2024-01-01T12:00:00Z"",""Rows"":[{""Key"":""2024-01-01T00:00:00"",""Value"":100.5}],""DeferCommandExecution"":true,""DeferDataGeneration"":true,""KeepNulls"":false}";
            
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

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(upsertData, _stjOptions);

            // Assert - Null dictionaries should be skipped
            Assert.That(stjJson, Does.Contain("Rows"));
            Assert.That(stjJson, Does.Not.Contain("MarketAssessment"));
            Assert.That(stjJson, Does.Not.Contain("AuctionRows"));
            Assert.That(stjJson, Does.Not.Contain("BidAsk"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should skip null dictionaries like Newtonsoft");
        }

        #endregion

        #region Dictionary Key Format Tests

        [Test]
        public void Dictionary_WithComplexKey_STJ_SerializesCompatibly()
        {
            // Arrange - Expected JSON from Newtonsoft.Json (literal)
            const string expectedJson = @"[{""Key"":""2024-01-01T00:00:00"",""Value"":100.5},{""Key"":""2024-01-02T12:30:00"",""Value"":101.2}]";
            
            var rows = new Dictionary<LocalDateTime, double?>
            {
                { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 },
                { new LocalDateTime(2024, 1, 2, 12, 30), 101.2 }
            };

            // Act - Serialize with STJ
            var stjJson = System.Text.Json.JsonSerializer.Serialize(rows, _stjOptions);

            // Assert - Should use Key/Value format in both
            Assert.That(stjJson, Does.Contain("Key"));
            Assert.That(stjJson, Does.Contain("Value"));
            AssertJsonEquals(expectedJson, stjJson, "STJ should use same Key/Value format as Newtonsoft");

            // Verify STJ deserialization
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<Dictionary<LocalDateTime, double?>>(stjJson, _stjOptions);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Count, Is.EqualTo(2));
            Assert.That(deserialized[new LocalDateTime(2024, 1, 1, 0, 0)], Is.EqualTo(100.5));
        }

        #endregion
    }
}
