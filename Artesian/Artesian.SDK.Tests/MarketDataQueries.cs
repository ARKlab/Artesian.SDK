using System;
using System.Collections.Generic;
using System.Net.Http;

using Ark.Tools.Nodatime;

using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class MarketDataQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData
        [Test]
        public void MarketData_ReadMarketDataByProviderCurveName()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadMarketDataRegistryAsync(new MarketDataIdentifier("TestProvider", "TestCurveName")).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity")
                    .WithQueryParam("provider", "TestProvider")
                    .WithQueryParam("curveName", "TestCurveName")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MarketData_ReadMarketDataByCurveRange()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadCurveRangeAsync(100000001, 1, 1, "M+1", new LocalDateTime(2018, 07, 19, 12, 0), new LocalDateTime(2017, 07, 19, 12, 0)).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/100000001/curves")
                    .WithQueryParam("versionFrom", new LocalDateTime(2018, 07, 19, 12, 0))
                    .WithQueryParam("versionTo", new LocalDateTime(2017, 07, 19, 12, 0))
                    .WithQueryParam("product", "M+1")
                    .WithQueryParam("page", 1)
                    .WithQueryParam("pageSize", 1)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void MarketData_ReadMarketDataRegistryAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadMarketDataRegistryAsync(100000001).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/100000001")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void MarketData_RegisterMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = "Test",
                    MarketDataName = "TestName",
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = "CET",
                    AggregationRule = AggregationRule.Undefined,
                    Type = MarketDataType.VersionedTimeSerie
                };

                var mdq = mds.RegisterMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        private const string _realProblemDetailsJson = @"{""Errors"":[{""Key"":""MarketDataEntity.Tags[0].Value[0]"",""Value"":[{""ErrorMessage"":""'Value' must be between 1 and 50 characters. You entered 155 characters."",""AttemptedValue"":""PowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPower"",""CustomState"":null,""ErrorCode"":""LengthValidator"",""FormattedMessagePlaceholderValues"":[{""Key"":""CollectionIndex"",""Value"":0},{""Key"":""MinLength"",""Value"":1},{""Key"":""MaxLength"",""Value"":50},{""Key"":""TotalLength"",""Value"":155},{""Key"":""PropertyName"",""Value"":""Value""},{""Key"":""PropertyValue"",""Value"":""PowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPowerPower""}]}]}],""type"":""https://httpstatuses.com/400"",""title"":""Bad Request"",""status"":400,""detail"":""'Value' must be between 1 and 50 characters. You entered 155 characters."",""instance"":null}";

        [Test]
        public void MarketData_RegisterIsFailingMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = "Test",
                    MarketDataName = "TestName",
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = "CET",
                    AggregationRule = AggregationRule.Undefined,
                    Type = MarketDataType.VersionedTimeSerie
                };

                httpTest
                    .RespondWith(_realProblemDetailsJson, 
                    status: 400, 
                    headers: new { content_type = "application/problem+json; charset=utf-8"});

                Assert.Throws<ArtesianSdkValidationException>(
                  () => { 
                      var mdq = mds.RegisterMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult(); 
                  }
                );

            }
        }

        [Test]
        public void MarketData_UpdateMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = "Test",
                    MarketDataName = "TestName",
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = "CET",
                    AggregationRule = AggregationRule.Undefined,
                    Type = MarketDataType.VersionedTimeSerie,
                    MarketDataId = 1
                };

                var mdq = mds.UpdateMarketDataAsync(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void MarketData_DeleteMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                mds.DeleteMarketDataAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region SearchFacet
        [Test]
        public void SearchFacet_SearchFacet()
        {
            using (var httpTest = new HttpTest())
            {
                Dictionary<string, string[]> filterDict = new Dictionary<string, string[]>
                {
                    {"TestKey",new []{"TestValue"} }
                };
                var mds = new MarketDataService(_cfg);
                var filter = new ArtesianSearchFilter
                {
                    Page = 1,
                    PageSize = 1,
                    SearchText = "testText",
                    Filters = filterDict,
                    Sorts = new List<string>() { "OriginalTimezone" }
                };
                var mdq = mds.SearchFacetAsync(filter, false).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/searchfacet")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("searchText", "testText")
                    .WithQueryParam("filters", "TestKey:TestValue")
                    .WithQueryParam("sorts", "OriginalTimezone")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        #endregion

        #region Operations
        [Test]
        public void Operations_PerformOperationsAsync_Enable()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag")},
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationEnableDisableTag()
                            {
                                TagKey = "Pippo",
                                TagValue = "Valore"
                            },
                            Type = OperationType.EnableTag,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void Operations_PerformOperationsAsync_Disable()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag") },
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationEnableDisableTag()
                            {
                                TagKey = "Pippo",
                                TagValue = "Valore"
                            },
                            Type = OperationType.DisableTag,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void Operations_PerformOperationsAsync_Aggregation()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag") },
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationUpdateAggregationRule()
                            {
                                Value = AggregationRule.Undefined
                            },
                            Type = OperationType.UpdateAggregationRule,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void Operations_PerformOperationsAsync_TimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag") },
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationUpdateOriginalTimeZone()
                            {
                                Value = "CET"
                            },
                            Type = OperationType.UpdateOriginalTimeZone,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void Operations_PerformOperationsAsync_TimeTransform()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag") },
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationUpdateTimeTransform()
                            {
                                Value = 0
                            },
                            Type = OperationType.UpdateTimeTransformID,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void Operations_PerformOperationsAsync_ProviderDescription()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var operations = new Operations()
                {
                    IDS = new HashSet<MarketDataETag>() { new MarketDataETag(0, "provaEtag") },
                    OperationList = new List<OperationParams>() {
                        new  OperationParams()
                        {
                            Params = new OperationUpdateProviderDescription()
                            {
                                Value = "prova"
                            },
                            Type = OperationType.UpdateProviderDescription,
                        }
                    }
                };

                var mdq = mds.PerformOperationsAsync(operations).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region UpsertCurve
        [Test]
        public void UpsertCurve_UpsertCurveDataAsync_MarketAssessment()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    DownloadedAt = SystemClock.Instance.GetCurrentInstant(),
                    MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>()
                };

                var localDateTime = new LocalDateTime(2018, 09, 24, 00, 00);

                data.MarketAssessment.Add(localDateTime, new Dictionary<string, MarketAssessmentValue>());
                data.MarketAssessment[localDateTime].Add("test", new MarketAssessmentValue());

                mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void UpsertCurve_UpsertCurveDataAsync_Auction()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    DownloadedAt = SystemClock.Instance.GetCurrentInstant(),
                    AuctionRows = new Dictionary<LocalDateTime, AuctionBids>()
                };

                var localDateTime = new LocalDateTime(2018, 09, 24, 00, 00);
                var bid = new List<AuctionBidValue>();
                var offer = new List<AuctionBidValue>();
                bid.Add(new AuctionBidValue(100, 10));
                offer.Add(new AuctionBidValue(120, 12));

                data.AuctionRows.Add(localDateTime, new AuctionBids(localDateTime, bid.ToArray(), offer.ToArray()));

                mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void UpsertCurve_UpsertCurveDataAsync_Versioned()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                //Create Version
                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    DownloadedAt = SystemClock.Instance.GetCurrentInstant(),
                    Rows = new Dictionary<LocalDateTime, double?>() { { new LocalDateTime(2018, 01, 01, 0, 0), 21.4 } },
                    Version = new LocalDateTime(2018, 09, 25, 12, 0, 0, 123).PlusNanoseconds(100)
                };

                mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void ValidationCheck_UpsertCurveData_Versioned_with_BidAsk()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                //Create invalid Version by populating BidAsk
                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    DownloadedAt = SystemClock.Instance.GetCurrentInstant(),
                    Rows = new Dictionary<LocalDateTime, double?>() { { new LocalDateTime(2018, 01, 01, 0, 0), 21.4 } },
                    BidAsk = new Dictionary<LocalDateTime, IDictionary<string, BidAskValue>>(),
                    Version = new LocalDateTime(2018, 09, 25, 12, 0, 0, 123).PlusNanoseconds(100)
                };

                var localDateTime = new LocalDateTime(2018, 09, 24, 00, 00);

                data.BidAsk.Add(localDateTime, new Dictionary<string, BidAskValue>());
                data.BidAsk[localDateTime].Add("test", new BidAskValue());

                var ex = Assert.Throws<ArgumentException>(() => mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult());
                Assert.AreEqual("UpsertCurveData BidAsk must be NULL if Rows are Valorized", ex.Message);
            }
        }

        [Test]
        public void ValidationCheck_UpsertCurveData_MarketAssesment_with_Auction()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                //Create invalid MarketAssessment by populating AutioonRows
                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    DownloadedAt = SystemClock.Instance.GetCurrentInstant(),
                    AuctionRows = new Dictionary<LocalDateTime, AuctionBids>(),
                    MarketAssessment = new Dictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>()
                };

                var localDateTime = new LocalDateTime(2018, 09, 24, 00, 00);
                var bid = new List<AuctionBidValue>();
                var offer = new List<AuctionBidValue>();
                bid.Add(new AuctionBidValue(100, 10));
                offer.Add(new AuctionBidValue(120, 12));

                data.AuctionRows.Add(localDateTime, new AuctionBids(localDateTime, bid.ToArray(), offer.ToArray()));

                data.MarketAssessment.Add(localDateTime, new Dictionary<string, MarketAssessmentValue>());
                data.MarketAssessment[localDateTime].Add("test", new MarketAssessmentValue());

                var ex = Assert.Throws<ArgumentException>(() => mds.UpsertCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult());
                Assert.AreEqual("UpsertCurveData Auctions must be NULL if MarketAssessment are Valorized", ex.Message);
            }
        }
        #endregion

        #region DeleteCurve
        [Test]
        public void DeleteCurve_DeleteCurveDataAsync_Product()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    RangeStart = new LocalDateTime(2018, 01, 01, 0, 0),
                    RangeEnd = new LocalDateTime(2018, 01, 03, 0, 0),
                    Product = new List<string> { "Jan-15" }
                };

                mds.DeleteCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void DeleteCurve_DeleteCurveDataAsync_Actual()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    RangeStart = new LocalDateTime(2018, 01, 01, 0, 0),
                    RangeEnd = new LocalDateTime(2018, 01, 03, 0, 0),
                };

                mds.DeleteCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public void DeleteCurve_DeleteCurveDataAsync_Versioned()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                //Create Version
                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    RangeStart = new LocalDateTime(2018, 01, 01, 0, 0),
                    RangeEnd = new LocalDateTime(2018, 01, 03, 0, 0),
                    Version = new LocalDateTime(2018, 09, 25, 12, 0, 0, 123).PlusNanoseconds(100)
                };

                mds.DeleteCurveDataAsync(data).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region TimeTransform
        [Test]
        public void TimeTransform_ReadTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadTimeTransformBaseAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadTimeTransformBaseAsync(2).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/2")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void TimeTransform_ReadTimeTransform()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadTimeTransformsAsync(1, 1, true).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("userDefined", true)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_ReadTimeTransformWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadTimeTransformsAsync(1, 1, true).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("userDefined", true)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_RegisterTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var timeTransformEntity = new TimeTransformSimpleShift()
                {
                    ID = 1,
                    Name = "TimeTName",
                    ETag = Guid.Empty,
                    DefinedBy = TransformDefinitionType.System,
                    Period = Granularity.Year,
                    PositiveShift = "",
                    NegativeShift = "P3M",
                };

                var mdq = mds.RegisterTimeTransformBaseAsync(timeTransformEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_UpdateTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var timeTransformEntity = new TimeTransformSimpleShift()
                {
                    ID = 1,
                    Name = "TimeTName",
                    ETag = Guid.Empty,
                    DefinedBy = TransformDefinitionType.System,
                    Period = Granularity.Year,
                    PositiveShift = "",
                    NegativeShift = "P3M",
                };

                var mdq = mds.UpdateTimeTransformBaseAsync(timeTransformEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void TimeTransform_DeleteTimeTransformSimpleShift()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                mds.DeleteTimeTransformSimpleShiftAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region CustomFilter
        [Test]
        public void CustomFilter_CreateFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter()
                {
                    Id = 1,
                    SearchText = "Text",
                    Name = "TestName"
                };

                var mdq = mds.CreateFilter(filter).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_UpdateFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter()
                {
                    Id = 1,
                    SearchText = "Text",
                    Name = "TestName"
                };

                var mdq = mds.UpdateFilter(1, filter).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_RemoveFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.RemoveFilter(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_ReadFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.ReadFilter(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void CustomFilter_ReadFilters()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = mds.ReadFilters(1, 1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region Acl
        [Test]
        public void Acl_ReadRolesByPath()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var path = new PathString(new[] { "Path1" });

                var mdq = mds.ReadRolesByPath(path).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl/me")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Acl_GetRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.GetRoles(1, 1, new[] { "Principals" }).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("principalIds", "Principals")
                    .WithoutQueryParam("asOf")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Acl_AddRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.AddRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl/roles")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Acl_UpsertRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.UpsertRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Acl_RemoveRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var auth = new AuthorizationPath.Input()
                {
                    Path = "",
                    Roles = new List<AuthorizationPrincipalRole>()
                    {
                        new AuthorizationPrincipalRole()
                        {
                            Role = "Role",
                            InheritedFrom = "InheritedFrom",
                            Principal = new Principal()
                            {
                                PrincipalId = "Id",
                                PrincipalType = PrincipalType.User
                            }
                        }
                    }
                };

                mds.RemoveRoles(auth).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl/roles")
                    .WithVerb(HttpMethod.Delete)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region Admin
        [Test]
        public void Admin_CreateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = mds.CreateAuthGroup(group).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Admin_UpdateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = mds.UpdateAuthGroup(1, group).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Admin_RemoveAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                mds.RemoveAuthGroup(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Admin_ReadAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                mds.ReadAuthGroup(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void Admin_ReadAuthGroups()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadAuthGroups(1, 1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region ApiKey
        [Test]
        public void ApiKey_CreateApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var apiKey = new ApiKey.Input()
                {
                    Id = 0
                };

                var mdq = mds.CreateApiKeyAsync(apiKey).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeyByIdAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadApiKeyByIdAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeyByKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadApiKeyByKeyAsync("testKey").ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithQueryParam("key", "testKey")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_ReadApiKeysAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = mds.ReadApiKeysAsync(1, 1, "testName").ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("userId", "testName")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void ApiKey_DeleteApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                mds.DeleteApiKeyAsync(1).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion
    }
}
