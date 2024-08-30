using System;
using NUnit.Framework;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using Artesian.SDK.Dto;
using System.Net.Http;
using NodaTime;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for DerivedTimeSerieQueries
    /// </summary>
    [TestFixture]
    public class DerivedTimeSerieQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region MarketData ids
        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                    .ForMarketData(new [] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .ForLastOfMonths(Period.FromMonths(-4))
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfMonths_Policy()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                .WithQueryParam("id", 100000001)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/P5D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodRangeExtractionWindowMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDateTime(2018, 6, 22, 0, 0, 0), new LocalDateTime(2018, 7, 23, 0, 0, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-06-22T00:00:00/2018-07-23T00:00:00/Day/P2W/P20D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromMonths(-1))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/P-1M")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/2018-06-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-20D/Day/P-1M/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromMonths(-1))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/P-1M")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/P-1M/P20D/Day/P-1M/P20D")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativeIntervalMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent(new LocalDateTime(2018, 5, 21, 12, 30, 15), new LocalDateTime(2018, 7, 23, 8, 45, 30))
                        .InRelativeInterval(RelativeInterval.MonthToDate)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/2018-05-21T12:30:15/2018-07-23T08:45:30/Day/MonthToDate")
                 .WithQueryParam("id", 100000001)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }


        [Test]
        public void DerTSInRelativeIntervalVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalVersion_Millisecond()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV(new LocalDateTime(2019, 05, 01, 2, 0, 0))
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("versionLimit", new LocalDateTime(2019, 05, 01, 2, 0, 0))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }


        [Test]
        public void DerTSWithMultipleMarketDataWindowLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithMultipleMarketDataWindowLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithMultipleMarketDataWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithMultipleMarketDataWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithMultipleMarketDataWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParam("id", 100000001)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tr", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfMonthsWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                    .WithHeadersTest()
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("id", 100000001)
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }

        [Test]
        public void DerTS_Partitioned_By_ID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
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
                    .ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult();

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
        public void DerTSInPeriodRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInDateRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalVersion_Millisecond_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativeIntervalMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInAbsoluteDateRangeMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSInRelativePeriodRangeExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                   .WithQueryParam("filterId", 1)
                  .WithQueryParam("tr", 1)
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeTransformVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tr", 1)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSWithTimeZoneVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10")
                   .WithQueryParam("filterId", 1)
                 .WithQueryParam("tz", "UTC")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void DerTSInPeriodRelativeIntervalLastOfMonthsWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   .WithHeadersTest()
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                   .WithQueryParam("filterId", 1)
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }
        #endregion

        #region partialQueryChanges
        [Test]
        public void DerTS_PeriodRelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateDerived()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastOfMonths(Period.FromMonths(-4))
                                    .InRelativeInterval(RelativeInterval.RollingMonth);

                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = partialQuery
                            .ForLastOfDays(Period.FromDays(-20))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);
            }
        }


        [Test]
        public void DerTS_RelativePeriodExtractionWindowChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateDerived()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastNVersions(3)
                                    .InRelativePeriod(Period.FromDays(5));

                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = partialQuery
                            .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test3 = partialQuery
                            .ForMUV()
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                           .WithQueryParam("id", 100000001)
                           .WithVerb(HttpMethod.Get)
                           .Times(1);
            }
        }

        [Test]
        public void DerTS_RelativePeriodForAnalysisDate()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateDerived()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Hour)
                                    .ForMUV()
                                    .InRelativePeriod(Period.FromDays(5));

                var test = partialQuery
                            .ForAnalysisDate(new LocalDate(2018, 07, 19))
                            .ExecuteAsync().Result;
            
                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Hour/P5D")
                        .WithQueryParam("id", 100000001)
                        .WithQueryParam("ad", "2018-07-19")
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }

        [Test]
        public void DerTS_RelativePeriodExtractionWindowFail()
        {
            var qs = new QueryService(_cfg);

            var query = qs.CreateDerived()
                                .ForMarketData(new [] {100000001})
                                .InGranularity(Granularity.Day)
                                .ForLastNVersions(3)
                                .InAbsoluteDateRange(
                                    new LocalDate(2018, 07, 15),
                                    new LocalDate(2018, 07, 20)
                                )
                                .ForAnalysisDate(new LocalDate(2018, 07, 19))
                                ;

            Assert.Throws<ArtesianSdkClientException>(
                () => {
                    query.ExecuteAsync().ConfigureAwait(true).GetAwaiter().GetResult(); 
                }
            );
        }

        #endregion

        #region Filler
        [Test]
        public void FillerNoneDerTSInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNone()
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK",FillerKindType.NoFill)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void FillerNullDerTSInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillNull()
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.Null)
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void FillerLatestValueDerTSInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillLatestValue(Period.FromDays(7))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MostRecent/Day/2018-06-22/2018-07-23")
                 .WithQueryParam("id", 100000001)
                 .WithQueryParam("fillerK", FillerKindType.LatestValidValue)
                 .WithQueryParam("fillerP","P7D")
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void FillerCustomValueDerTSInAbsoluteDateRangeMostRecent()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                        .ForMarketData(new [] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForMostRecent()
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .WithFillCustomValue(123)
                        .ExecuteAsync().Result;

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
