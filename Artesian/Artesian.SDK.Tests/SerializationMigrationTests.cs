// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
#pragma warning disable MA0002 // Dictionary ContainsKey is fine without comparer in tests
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.UoM;
using Artesian.SDK.Service;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Serialization tests to ensure compatibility during migration from Newtonsoft.Json to System.Text.Json
    /// These tests capture the expected JSON format using the current Newtonsoft.Json serializer,
    /// then validate that the new System.Text.Json serializer produces equivalent results
    /// </summary>
    [TestFixture]
    public class SerializationMigrationTests
    {
        private JsonSerializerSettings _newtonsoftSettings = null!;

        [SetUp]
        public void Setup()
        {
            // Configure Newtonsoft.Json settings matching the current Client.cs setup
            _newtonsoftSettings = new JsonSerializerSettings();
            _newtonsoftSettings = _newtonsoftSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            // Manually add dictionary converter since ConfigureForDictionary is internal
            _newtonsoftSettings.Converters.Add(new Service.DictionaryJsonConverter());
            _newtonsoftSettings.Formatting = Formatting.None; // Use compact format for easier comparison
            _newtonsoftSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            _newtonsoftSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            _newtonsoftSettings.TypeNameHandling = TypeNameHandling.None;
            _newtonsoftSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            _newtonsoftSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        #region MarketData Entity Tags Dictionary Tests

        [Test]
        public void MarketDataEntity_WithTags_Serializes_Correctly()
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

            // Act - Serialize with Newtonsoft
            var newtonsoftJson = JsonConvert.SerializeObject(entity, _newtonsoftSettings);

            // Assert - Verify JSON contains Tags as expected
            Assert.That(newtonsoftJson, Does.Contain("Tags"));
            Assert.That(newtonsoftJson, Does.Contain("Region"));
            Assert.That(newtonsoftJson, Does.Contain("Europe"));

            // Deserialize to verify round-trip
            var deserialized = JsonConvert.DeserializeObject<MarketDataEntity.Input>(newtonsoftJson, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Tags, Is.Not.Null);
            Assert.That(deserialized.Tags!.Count, Is.EqualTo(3));
            Assert.That(deserialized.Tags["Region"], Does.Contain("Europe"));
            Assert.That(deserialized.Tags["Product"], Does.Contain("Power"));
        }

        [Test]
        public void MarketDataEntity_WithNullTags_SkipsInSerialization()
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

            // Act
            var json = JsonConvert.SerializeObject(entity, _newtonsoftSettings);

            // Assert - Null Tags should not be present in JSON
            Assert.That(json, Does.Not.Contain("Tags"));
        }

        [Test]
        public void MarketDataEntity_Tags_DictionaryKeyCasing_Preserved()
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

            // Act
            var json = JsonConvert.SerializeObject(entity, _newtonsoftSettings);

            // Assert - Dictionary keys should preserve original casing
            Assert.That(json, Does.Contain("RegionCode"));
            Assert.That(json, Does.Contain("PRODUCT_TYPE"));
            Assert.That(json, Does.Contain("marketSegment"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<MarketDataEntity.Input>(json, _newtonsoftSettings);
            Assert.That(deserialized!.Tags!.ContainsKey("RegionCode"), Is.True);
            Assert.That(deserialized.Tags.ContainsKey("PRODUCT_TYPE"), Is.True);
            Assert.That(deserialized.Tags.ContainsKey("marketSegment"), Is.True);
        }

        #endregion

        #region TimeTransform Polymorphic Tests

        [Test]
        public void TimeTransform_SimpleShift_Serializes_WithType()
        {
            // Arrange
            var transform = new TimeTransformSimpleShift
            {
                ID = 1,
                Name = "SimpleShift1",
                ETag = Guid.NewGuid(),
                DefinedBy = TransformDefinitionType.User,
                Period = Granularity.Day
            };

            // Act
            var json = JsonConvert.SerializeObject(transform, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("Type"));
            Assert.That(json, Does.Contain("SimpleShift"));
            Assert.That(json, Does.Contain("Period"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<TimeTransform>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<TimeTransformSimpleShift>());
            Assert.That(((TimeTransformSimpleShift)deserialized!).Period, Is.EqualTo(Granularity.Day));
        }

        [Test]
        public void TimeTransform_BaseClass_Deserializes_ToCorrectType()
        {
            // Arrange - Create JSON representing a TimeTransformSimpleShift
            var transform = new TimeTransformSimpleShift
            {
                ID = 2,
                Name = "TestShift",
                ETag = Guid.NewGuid(),
                DefinedBy = TransformDefinitionType.System,
                Period = Granularity.Hour
            };
            var json = JsonConvert.SerializeObject(transform, _newtonsoftSettings);

            // Act - Deserialize as base type
            var deserialized = JsonConvert.DeserializeObject<TimeTransform>(json, _newtonsoftSettings);

            // Assert
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<TimeTransformSimpleShift>());
            var shift = (TimeTransformSimpleShift)deserialized!;
            Assert.That(shift.Type, Is.EqualTo(TransformType.SimpleShift));
            Assert.That(shift.Name, Is.EqualTo("TestShift"));
        }

        #endregion

        #region DerivedCfg Polymorphic Tests

        [Test]
        public void DerivedCfg_MUV_Serializes_WithAlgorithm()
        {
            // Arrange
            var cfg = new DerivedCfgMuv
            {
                Version = 1
            };

            // Act
            var json = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("DerivedAlgorithm"));
            Assert.That(json, Does.Contain("MUV"));
            Assert.That(json, Does.Contain("Version"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<DerivedCfgBase>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgMuv>());
            Assert.That(((DerivedCfgMuv)deserialized!).Version, Is.EqualTo(1));
        }

        [Test]
        public void DerivedCfg_Sum_Serializes_Correctly()
        {
            // Arrange
            var cfg = new DerivedCfgSum
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act
            var json = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("DerivedAlgorithm"));
            Assert.That(json, Does.Contain("Sum"));
            Assert.That(json, Does.Contain("OrderedReferencedMarketDataIds"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<DerivedCfgBase>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgSum>());
            var sum = (DerivedCfgSum)deserialized!;
            Assert.That(sum.OrderedReferencedMarketDataIds, Has.Length.EqualTo(2));
        }

        [Test]
        public void DerivedCfg_Coalesce_Serializes_Correctly()
        {
            // Arrange
            var cfg = new DerivedCfgCoalesce
            {
                OrderedReferencedMarketDataIds = new int[] { 100000001, 100000002 }
            };

            // Act
            var json = JsonConvert.SerializeObject(cfg, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("DerivedAlgorithm"));
            Assert.That(json, Does.Contain("Coalesce"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<DerivedCfgBase>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized, Is.InstanceOf<DerivedCfgCoalesce>());
        }

        #endregion

        #region UpsertData Dictionary Properties Tests

        [Test]
        public void UpsertCurveData_Rows_Serializes_Correctly()
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
                    { new LocalDateTime(2024, 1, 2, 0, 0), 101.2 },
                    { new LocalDateTime(2024, 1, 3, 0, 0), null } // Test null value
                }
            };

            // Act
            var json = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("Rows"));
            Assert.That(json, Does.Contain("2024-01-01"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<UpsertCurveData>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Rows, Is.Not.Null);
            Assert.That(deserialized.Rows!.Count, Is.EqualTo(3));
            Assert.That(deserialized.Rows[new LocalDateTime(2024, 1, 1, 0, 0)], Is.EqualTo(100.5));
            Assert.That(deserialized.Rows[new LocalDateTime(2024, 1, 3, 0, 0)], Is.Null);
        }

        [Test]
        public void UpsertCurveData_MarketAssessment_Serializes_Correctly()
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

            // Act
            var json = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("MarketAssessment"));
            Assert.That(json, Does.Contain("Product1"));
            Assert.That(json, Does.Contain("Settlement"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<UpsertCurveData>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.MarketAssessment, Is.Not.Null);
            Assert.That(deserialized.MarketAssessment!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_AuctionRows_Serializes_Correctly()
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

            // Act
            var json = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("AuctionRows"));
            Assert.That(json, Does.Contain("Bid"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<UpsertCurveData>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.AuctionRows, Is.Not.Null);
            Assert.That(deserialized.AuctionRows!.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpsertCurveData_BidAsk_Serializes_Correctly()
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

            // Act
            var json = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("BidAsk"));
            Assert.That(json, Does.Contain("BestBidPrice"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<UpsertCurveData>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.BidAsk, Is.Not.Null);
            Assert.That(deserialized.BidAsk!.Count, Is.EqualTo(1));
        }

        #endregion

        #region Null Handling Tests

        [Test]
        public void Serialization_SkipsNullProperties()
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

            // Act
            var json = JsonConvert.SerializeObject(entity, _newtonsoftSettings);

            // Assert - Null properties should not be present
            Assert.That(json, Does.Not.Contain("Tags"));
            Assert.That(json, Does.Not.Contain("ProviderDescription"));
            Assert.That(json, Does.Not.Contain("TransformID"));
        }

        [Test]
        public void UpsertCurveData_NullDictionaries_SkippedInSerialization()
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

            // Act
            var json = JsonConvert.SerializeObject(upsertData, _newtonsoftSettings);

            // Assert
            Assert.That(json, Does.Contain("Rows"));
            Assert.That(json, Does.Not.Contain("MarketAssessment"));
            Assert.That(json, Does.Not.Contain("AuctionRows"));
            Assert.That(json, Does.Not.Contain("BidAsk"));
        }

        #endregion

        #region Dictionary Key Format Tests

        [Test]
        public void Dictionary_WithComplexKey_SerializesCorrectly()
        {
            // Arrange - LocalDateTime as dictionary key
            var rows = new Dictionary<LocalDateTime, double?>
            {
                { new LocalDateTime(2024, 1, 1, 0, 0), 100.5 },
                { new LocalDateTime(2024, 1, 2, 12, 30), 101.2 }
            };

            // Act
            var json = JsonConvert.SerializeObject(rows, _newtonsoftSettings);

            // Assert - Should use the DictionaryJsonConverter format
            Assert.That(json, Does.Contain("Key"));
            Assert.That(json, Does.Contain("Value"));

            // Verify deserialization
            var deserialized = JsonConvert.DeserializeObject<Dictionary<LocalDateTime, double?>>(json, _newtonsoftSettings);
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Count, Is.EqualTo(2));
            Assert.That(deserialized[new LocalDateTime(2024, 1, 1, 0, 0)], Is.EqualTo(100.5));
        }

        #endregion
    }
}
