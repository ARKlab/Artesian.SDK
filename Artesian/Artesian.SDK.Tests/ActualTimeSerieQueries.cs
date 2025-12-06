using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class ActualTimeSerieQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData ids
        [Test]
        public async Task ActInRelativeIntervalExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/RollingMonth")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInAbsoluteDateRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInRelativePeriodExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInRelativePeriodRangeExtractionWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActMultipleMarketDataWindow()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Month)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromMonths(6))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Month/P2W/P6M")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithTimeZone()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .WithQueryParam("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Hour)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Hour/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .WithQueryParam("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithTimeTransfrom()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .WithQueryParam("tr", 1)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(SystemTimeTransform.THERMALYEAR)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("id", 100000001)
                    .WithQueryParam("tr", 2)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithQueryParam("id", 100000001)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithCustomHeader()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithQueryParam("id", 100000001)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .WithHeader("X-Artesian-Agent", ArtesianConstants.SDKVersionHeaderValue)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithCustomHeaderFormatCheck()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                var headerXAgent = httpTest.CallLog.FirstOrDefault().Request.Headers.FirstOrDefault(w => string.Equals(w.Name, "X-Artesian-Agent"));

                //ArtesianSDK-C#: 2.2.1.0,Win32NT: 10.0.19041.0,.NETFramework: 4.6.1
                //ArtesianSDK-C#:1.0.0.0,Unix:6.11.0.1018,.NETCoreApp:8.0

                // Determine expected OS string based on runtime OS
#pragma warning disable MA0144 // Use System.OperatingSystem to check the current OS
                var expectedOsString = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) 
                    ? "Win32NT:" 
                    : "Unix:";
#pragma warning restore MA0144 // Use System.OperatingSystem to check the current OS

                Assert.That(headerXAgent.Value, Does.Contain("ArtesianSDK-C#:").And.Contain(expectedOsString) & ( Does.Contain(".NETCoreApp:").Or.Contain(".NETFramework") ) );
            }
        }

        [Test]
        public async Task Act_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
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
                    .InGranularity(Granularity.Day)
                    .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                    .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] {
                        100001302, 100001303, 100001304, 100001305, 100001306,
                        100001307, 100001308, 100001309, 100001310, 100001311,
                        100001312, 100001313, 100001314, 100001315, 100001315
                    })
                    .Times(1);
            }
        }
        #endregion

        #region FilterId
        [Test]
        public async Task ActInRelativeIntervalExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/RollingMonth")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInAbsoluteDateRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInRelativePeriodExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInRelativePeriodRangeExtractionWindow_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithTimeZone_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("tz", "UTC")
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Hour)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("WET")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Hour/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("tz", "WET")
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithTimeTransfrom_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("tr", 1)
                    .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 1, 1), new LocalDate(2018, 1, 10))
                       .WithTimeTransform(SystemTimeTransform.THERMALYEAR)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-01-01/2018-01-10")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("tr", 2)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithQueryParam("filterId", 1)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActWithAggregationRule_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithAggregationRule(AggregationRule.SumAndDivide)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("aggregationRule", AggregationRule.SumAndDivide)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }

        [Test]
        public async Task ActInUnitOfMeasure_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .InUnitOfMeasure(CommonUnitOfMeasure.kWh)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/P2W/P20D")
                    .WithQueryParam("filterId", 1)
                    .WithQueryParam("unitOfMeasure", CommonUnitOfMeasure.kWh)
                    .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                    .WithHeader("X-Api-Key", TestConstants.APIKey)
                    .Times(1);
            }
        }
        #endregion

        #region Filler
        [Test]
        public async Task FillerNoneActInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNone()
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.NoFill)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerNullActInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNull()
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.Null)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerLatestValueActInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillLatestValue(Period.FromDays(7))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.LatestValidValue)
                 .WithQueryParam("fillerP", "P7D")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerCustomValueActInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = await qs.CreateActual()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillCustomValue(123)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/ts/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.CustomValue)
                 .WithQueryParam("fillerDV", 123)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }
        #endregion
    }
}
