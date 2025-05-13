using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using Artesian.SDK.Dto;
using System.Net.Http;
using NodaTime;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for VersionedTimeSerieQueries
    /// </summary>
    [TestFixture]
    public class VersionedTimeSerieQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData ids
        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfMonths_Policy()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/P5D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodRangeExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDateTime(2018, 6, 22, 0, 0, 0), new LocalDateTime(2018, 7, 23, 0, 0, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromMonths(-1))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/P-1M")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/2018-06-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/P-1M/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromMonths(-1))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/P-1M")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/P-1M/P20D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativeIntervalMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDateTime(2018, 5, 21, 12, 30, 15), new LocalDateTime(2018, 7, 23, 8, 45, 30))
                        .InRelativeInterval(RelativeInterval.MonthToDate)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-05-21T12:30:15/2018-07-23T08:45:30/Day/MonthToDate")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }


        [Test]
        public async Task VerInRelativeIntervalVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalVersion_Millisecond()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV(new LocalDateTime(2019, 05, 01, 2, 0, 0))
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("versionLimit", new LocalDateTime(2019, 05, 01, 2, 0, 0))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }


        [Test]
        public async Task VerWithMultipleMarketDataWindowLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithMultipleMarketDataWindowLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithMultipleMarketDataWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithMultipleMarketDataWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithMultipleMarketDataWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tr", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfMonthsWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }

        [Test]
        public async Task Ver_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
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
                    .InRelativePeriod(Period.FromDays(5))
                    .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                    .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamMultiple("id", new [] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .Times(1);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
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
        public async Task VerInPeriodRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInDateRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalVersion_Millisecond_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativeIntervalMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInAbsoluteDateRangeMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerInRelativePeriodRangeExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeTransformVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tr", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerWithTimeZoneVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task VerInPeriodRelativeIntervalLastOfMonthsWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .WithHeadersTest()
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }


        [Test]
        public async Task VerWithAggregationRule_InUnitOfMeasure()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .InUnitOfMeasure("kWh")
                        .WithAggregationRule(AggregationRule.SumAndDivide)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithQueryParam("unitOfMeasure", "kWh")
                   .WithQueryParam("aggregationRule", AggregationRule.SumAndDivide)
                   .WithVerb(HttpMethod.Get)
                   .WithHeadersTest()
                   .Times(1);
            }
        }
        #endregion

        #region partialQueryChanges
        [Test]
        public async Task Ver_PeriodRelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateVersioned()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastOfMonths(Period.FromMonths(-4))
                                    .InRelativeInterval(RelativeInterval.RollingMonth);

                var test1 = await partialQuery
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = await partialQuery
                            .ForLastOfDays(Period.FromDays(-20))
                            .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);
            }
        }


        [Test]
        public async Task Ver_RelativePeriodExtractionWindowChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateVersioned()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastNVersions(3)
                                    .InRelativePeriod(Period.FromDays(5));

                var test1 = await partialQuery
                            .ExecuteAsync();
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = await partialQuery
                            .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                            .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test3 = await partialQuery
                            .ForMUV()
                            .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);
            }
        }

        [Test]
        public async Task Ver_RelativePeriodForAnalysisDate()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateVersioned()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Hour)
                                    .ForMUV()
                                    .InRelativePeriod(Period.FromDays(5));

                var test = await partialQuery
                            .ForAnalysisDate(new LocalDate(2018, 07, 19))
                            .ExecuteAsync();
            
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Hour/P5D")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParam("ad", "2018-07-19")
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }

        [Test]
        public void Ver_RelativePeriodExtractionWindowFail()
        {
            var qs = new QueryService(_cfg);

            var query = qs.CreateVersioned()
                                .ForMarketData(new [] {100000001})
                                .InGranularity(Granularity.Day)
                                .ForLastNVersions(3)
                                .InAbsoluteDateRange(
                                    new LocalDate(2018, 07, 15),
                                    new LocalDate(2018, 07, 20)
                                )
                                .ForAnalysisDate(new LocalDate(2018, 07, 19))
                                ;

            Assert.ThrowsAsync<ArtesianSdkClientException>(
                () => {
                    return query.ExecuteAsync(); 
                }
            );
        }

        #endregion

        #region Filler
        [Test]
        public async Task FillerNoneVerInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNone()
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK",FillerKindType.NoFill)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerNullVerInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNull()
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.Null)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerLatestValueVerInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillLatestValue(Period.FromDays(7))
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.LatestValidValue)
                 .WithQueryParam("fillerP","P7D")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public async Task FillerCustomValueVerInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = await qs.CreateVersioned()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillCustomValue(123)
                        .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.CustomValue)
                 .WithQueryParam("fillerDV",123)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }
        #endregion
    }
}
