using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class MarketDataQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData
        [Test]
        public async Task MarketData_ReadMarketDataByProviderCurveName()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadMarketDataRegistryAsync(new MarketDataIdentifier("TestProvider", "TestCurveName"));

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity")
                    .WithQueryParam("provider", "TestProvider")
                    .WithQueryParam("curveName", "TestCurveName")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MarketData_ReadMarketDataByCurveRange()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadCurveRangeAsync(100000001, 1, 1, "M+1", new LocalDateTime(2018, 07, 19, 12, 0), new LocalDateTime(2017, 07, 19, 12, 0));

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
        public async Task MarketData_ReadMarketDataRegistryAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadMarketDataRegistryAsync(100000001);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/100000001")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task MarketData_RegisterMarketDataAsync()
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

                var mdq = await mds.RegisterMarketDataAsync(marketDataEntity);

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

                Assert.ThrowsAsync<ArtesianSdkValidationException>(
                  () => { 
                      return mds.RegisterMarketDataAsync(marketDataEntity); 
                  }
                );

            }
        }

        [Test]
        public async Task MarketData_UpdateMarketDataAsync()
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

                var mdq = await mds.UpdateMarketDataAsync(marketDataEntity);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task MarketData_DeleteMarketDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                await mds.DeleteMarketDataAsync(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region SearchFacet
        [Test]
        public async Task SearchFacet_SearchFacet()
        {
            using (var httpTest = new HttpTest())
            {
                Dictionary<string, string[]> filterDict = new Dictionary<string, string[]>(StringComparer.Ordinal)
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
                var mdq = await mds.SearchFacetAsync(filter, false);

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
        public async Task Operations_PerformOperationsAsync_Enable()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task Operations_PerformOperationsAsync_Disable()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task Operations_PerformOperationsAsync_Aggregation()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task Operations_PerformOperationsAsync_TimeZone()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task Operations_PerformOperationsAsync_TimeTransform()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task Operations_PerformOperationsAsync_ProviderDescription()
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

                var mdq = await mds.PerformOperationsAsync(operations);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/operations")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region UpsertCurve
        [Test]
        public async Task UpsertCurve_UpsertCurveDataAsync_MarketAssessment()
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

                data.MarketAssessment.Add(localDateTime, new Dictionary<string, MarketAssessmentValue>(StringComparer.Ordinal));
                data.MarketAssessment[localDateTime].Add("test", new MarketAssessmentValue());

                await mds.UpsertCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task UpsertCurve_UpsertCurveDataAsync_Auction()
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

                await mds.UpsertCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task UpsertCurve_UpsertCurveDataAsync_AuctionWithAcceptedBids()
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
                bid.Add(new AuctionBidValue(100, 10, 101, 10.1));
                offer.Add(new AuctionBidValue(120, 12, 121, 12.1));

                data.AuctionRows.Add(localDateTime, new AuctionBids(localDateTime, bid.ToArray(), offer.ToArray()));

                await mds.UpsertCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task UpsertCurve_UpsertCurveDataAsync_Versioned()
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

                await mds.UpsertCurveDataAsync(data);

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

                data.BidAsk.Add(localDateTime, new Dictionary<string, BidAskValue>(StringComparer.Ordinal));
                data.BidAsk[localDateTime].Add("test", new BidAskValue());

                var ex = Assert.ThrowsAsync<ArgumentException>(() => mds.UpsertCurveDataAsync(data));
                ClassicAssert.AreEqual("UpsertCurveData BidAsk must be NULL if Rows are Valorized (Parameter 'upsertCurveData')", ex.Message);
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

                data.MarketAssessment.Add(localDateTime, new Dictionary<string, MarketAssessmentValue>(StringComparer.Ordinal));
                data.MarketAssessment[localDateTime].Add("test", new MarketAssessmentValue());

                var ex = Assert.ThrowsAsync<ArgumentException>(() => mds.UpsertCurveDataAsync(data));
                ClassicAssert.AreEqual("UpsertCurveData Auctions must be NULL if MarketAssessment are Valorized (Parameter 'upsertCurveData')", ex.Message);
            }
        }
        #endregion

        #region DeleteCurve
        [Test]
        public async Task DeleteCurve_DeleteCurveDataAsync_Product()
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

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteCurve_DeleteCurveDataWholeRangeAsync_Product()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    Product = new List<string> { "Jan-15" }
                };

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteCurve_DeleteCurveDataAsync_Actual()
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

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteCurve_DeleteCurveDataWholeRangeAsync_Actual()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                };

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteCurve_DeleteCurveDataAsync_Versioned()
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

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteCurve_DeleteCurveDataWholeRangeAsync_Versioned()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                //Create Version
                var data = new DeleteCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "CET",
                    Version = new LocalDateTime(2018, 09, 25, 12, 0, 0, 123).PlusNanoseconds(100)
                };

                await mds.DeleteCurveDataAsync(data);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/marketdata/deletedata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion

        #region TimeTransform
        [Test]
        public async Task TimeTransform_ReadTimeTransformBaseAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadTimeTransformBaseAsync(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadTimeTransformBaseAsync(2);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/2")
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task TimeTransform_ReadTimeTransform()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadTimeTransformsAsync(1, 1, true);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithQueryParam("pageSize", 1)
                    .WithQueryParam("page", 1)
                    .WithQueryParam("userDefined", true)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task TimeTransform_ReadTimeTransformWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadTimeTransformsAsync(1, 1, true);

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
        public async Task TimeTransform_RegisterTimeTransformBaseAsync()
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

                var mdq = await mds.RegisterTimeTransformBaseAsync(timeTransformEntity);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task TimeTransform_UpdateTimeTransformBaseAsync()
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

                var mdq = await mds.UpdateTimeTransformBaseAsync(timeTransformEntity);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task TimeTransform_DeleteTimeTransformSimpleShift()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                await mds.DeleteTimeTransformSimpleShiftAsync(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/timeTransform/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region CustomFilter
        [Test]
        public async Task CustomFilter_CreateFilter()
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

                var mdq = await mds.CreateFilter(filter);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task CustomFilter_UpdateFilter()
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

                var mdq = await mds.UpdateFilter(1, filter);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task CustomFilter_RemoveFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = await mds.RemoveFilter(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task CustomFilter_ReadFilter()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = await mds.ReadFilter(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/filter/")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task CustomFilter_ReadFilters()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var filter = new CustomFilter();

                var mdq = await mds.ReadFilters(1, 1);

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
        public async Task Acl_ReadRolesByPath()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var path = new PathString(new[] { "Path1" });

                var mdq = await mds.ReadRolesByPath(path);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl/me")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Acl_GetRoles()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.GetRoles(1, 1, new[] { "Principals" });

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
        public async Task Acl_AddRoles()
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

                await mds.AddRoles(auth);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl/roles")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Acl_UpsertRoles()
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

                await mds.UpsertRoles(auth);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/acl")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Acl_RemoveRoles()
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

                await mds.RemoveRoles(auth);

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
        public async Task Admin_CreateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = await mds.CreateAuthGroup(group);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Admin_UpdateAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var group = new AuthGroup()
                {
                    ID = 1,
                    Name = "AuthGroupTest"
                };

                var mdq = await mds.UpdateAuthGroup(1, group);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Put)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Admin_RemoveAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                await mds.RemoveAuthGroup(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Admin_ReadAuthGroup()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                await mds.ReadAuthGroup(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/group/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Admin_ReadAuthGroups()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadAuthGroups(1, 1);

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
        public async Task ApiKey_CreateApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var apiKey = new ApiKey.Input()
                {
                    Id = 0
                };

                var mdq = await mds.CreateApiKeyAsync(apiKey);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithVerb(HttpMethod.Post)
                    .WithContentType("application/x.msgpacklz4")
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task ApiKey_ReadApiKeyByIdAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadApiKeyByIdAsync(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task ApiKey_ReadApiKeyByKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadApiKeyByKeyAsync("testKey");

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity")
                    .WithQueryParam("key", "testKey")
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task ApiKey_ReadApiKeysAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                var mdq = await mds.ReadApiKeysAsync(1, 1, "testName");

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
        public async Task ApiKey_DeleteApiKeyAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var mds = new MarketDataService(_cfg);

                await mds.DeleteApiKeyAsync(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}v2.1/apikey/entity/1")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion
    }
}
