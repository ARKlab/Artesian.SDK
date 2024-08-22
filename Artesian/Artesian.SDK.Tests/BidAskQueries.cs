using System;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using System.Net.Http;
using NodaTime;
using NUnit.Framework;
using Artesian.SDK.Dto;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for BidAskQueries
    /// </summary>
    [TestFixture]
    public class BidAskQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData Ids
        [Test]
        public void BidAskInRelativeIntervalExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1","GY+1"})
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P5D")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParam("id" , new [] { 100000001, 100000002, 100000003 })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P6M")
                    .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "UTC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "WET")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public void BidAsk_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = qs.CreateBidAsk()
                    .ForMarketData(new [] {
                        100001250, 100001251, 100001252, 100001253, 100001254,
                        100001255, 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274,
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301,
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315 })
                    .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                    .ForProducts(new [] { "M+1", "GY+1" })
                    .ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParamMultiple("id", new [] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParamMultiple("id", new [] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParamMultiple("id", new [] {
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315
                    })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        #region FilterId
        [Test]
        public void BidAskInRelativeIntervalExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInRelativePeriodExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P5D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "UTC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "WET")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void BidAskWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }
        #endregion

        #region Filler
        [Test]
        public void FillerNullBidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNull()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK",FillerKindType.Null)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerNoFillBidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNone()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.NoFill)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        
        [Test]
        public void FillerLatestValidValueBidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillLatestValue(Period.FromDays(7))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.LatestValidValue)
                    .WithQueryParam("fillerP","P7D")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerCustomValueBidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new BidAskValue { BestBidPrice = 123, BestBidQuantity = 456, BestAskPrice = 789, BestAskQuantity = 321, LastPrice = 654, LastQuantity = 987})
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.CustomValue)
                    .WithQueryParam("fillerDVbbp",123)
                    .WithQueryParam("fillerDVbap", 789)
                    .WithQueryParam("fillerDVbbq", 456)
                    .WithQueryParam("fillerDVbaq", 321)
                    .WithQueryParam("fillerDVlp", 654)
                    .WithQueryParam("fillerDVlq", 987)                    
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void FillerCustomValueBBPBBQBAPBAQBidAskInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var bask = qs.CreateBidAsk()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new BidAskValue { BestBidPrice = 456, BestBidQuantity = 789, BestAskPrice = 321, BestAskQuantity = 654 })
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.CustomValue)
                    .WithQueryParam("fillerDVbbp", 456)
                    .WithQueryParam("fillerDVbap", 321)
                    .WithQueryParam("fillerDVbbq", 789)
                    .WithQueryParam("fillerDVbaq", 654)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        [Test]
        public void BidAskExtractionWindow_MktChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateBidAsk()
                           .ForMarketData(new [] { 100000001 })
                           .ForProducts(new [] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = partialQuery
                    .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                var test2 = partialQuery
                            .ForFilterId(1)
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                        .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = partialQuery
                            .ForMarketData(new [] { 100000004, 100000005, 100000006 })
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                        .WithQueryParamMultiple("id", new [] { 100000004, 100000005, 100000006 })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }

        [Test]
        public void BidAskExtractionWindow_RelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateBidAsk()
                           .ForMarketData(new [] { 100000001 })
                           .ForProducts(new [] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/RollingMonth")
                        .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test2 = partialQuery
                            .InRelativePeriod(Period.FromDays(5))
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P5D")
                        .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = partialQuery
                            .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/P2W/P20D")
                        .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test4 = partialQuery
                            .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ba/2018-01-01/2018-01-10")
                        .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);


            }
        }
    }
}
