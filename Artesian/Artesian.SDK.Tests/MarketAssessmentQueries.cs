using System;
using System.Threading.Tasks;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using System.Net.Http;
using NodaTime;
using NUnit.Framework;
using Artesian.SDK.Dto;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for MarketAssessmentQueries
    /// </summary>
    [TestFixture]
    public class MarketAssessmentQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData Ids
        [Test]
        public async Task MasInRelativeIntervalExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1","GY+1"})
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P5D")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
                    .WithQueryParam("id" , new [] { 100000001, 100000002, 100000003 })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P6M")
                    .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "UTC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "WET")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .Times(1);
            }
        }

        [Test]
        public async Task Mas_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = await qs.CreateMarketAssessment()
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
                    .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
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

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
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

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
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
        public async Task MasInRelativeIntervalExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInRelativePeriodExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P5D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "UTC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("tz", "WET")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task MasWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForFilterId(1)
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
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
        public async Task FillerNullMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNull()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK",FillerKindType.Null)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task FillerNoFillMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillNone()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.NoFill)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        
        [Test]
        public async Task FillerLatestValidValueMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillLatestValue(Period.FromDays(7))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.LatestValidValue)
                    .WithQueryParam("fillerP","P7D")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task FillerCustomValueMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new MarketAssessmentValue { Settlement = 123, Open = 456, Close = 789, High = 321, Low = 654, VolumePaid = 987, VolumeGiven = 213, Volume = 435 })
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.CustomValue)
                    .WithQueryParam("fillerDVs",123)
                    .WithQueryParam("fillerDVo", 456)
                    .WithQueryParam("fillerDVc", 789)
                    .WithQueryParam("fillerDVh", 321)
                    .WithQueryParam("fillerDVl", 654)
                    .WithQueryParam("fillerDVvp", 987)
                    .WithQueryParam("fillerDVvg", 213)
                    .WithQueryParam("fillerDVvt", 435)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task FillerCustomValueOHLCMasInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var mas = await qs.CreateMarketAssessment()
                       .ForMarketData(new [] { 100000001 })
                       .ForProducts(new [] { "M+1", "GY+1" })
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithFillCustomValue(new MarketAssessmentValue { Open = 456, Close = 789, High = 321, Low = 654 })
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithQueryParam("fillerK", FillerKindType.CustomValue)
                    .WithQueryParam("fillerDVo", 456)
                    .WithQueryParam("fillerDVc", 789)
                    .WithQueryParam("fillerDVh", 321)
                    .WithQueryParam("fillerDVl", 654)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }
        #endregion

        [Test]
        public async Task MasExtractionWindow_MktChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateMarketAssessment()
                           .ForMarketData(new [] { 100000001 })
                           .ForProducts(new [] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = await partialQuery
                    .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                    .WithQueryParam("id", 100000001)
                    .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                    .WithVerb(HttpMethod.Get)
                    .Times(1);

                var test2 = await partialQuery
                            .ForFilterId(1)
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                        .WithQueryParam("filterId", 1)
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = await partialQuery
                            .ForMarketData(new [] { 100000004, 100000005, 100000006 })
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                        .WithQueryParamMultiple("id", new [] { 100000004, 100000005, 100000006 })
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }

        [Test]
        public async Task MasExtractionWindow_RelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateMarketAssessment()
                           .ForMarketData(new [] { 100000001 })
                           .ForProducts(new [] { "M+1", "GY+1" })
                           .InRelativeInterval(RelativeInterval.RollingMonth);


                var test1 = await partialQuery
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/RollingMonth")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test2 = await partialQuery
                            .InRelativePeriod(Period.FromDays(5))
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P5D")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test3 = await partialQuery
                            .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/P2W/P20D")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                var test4 = await partialQuery
                            .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                            .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/mas/2018-01-01/2018-01-10")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParamMultiple("p", new [] { "M+1", "GY+1" })
                        .WithVerb(HttpMethod.Get)
                        .Times(1);


            }
        }
    }
}
