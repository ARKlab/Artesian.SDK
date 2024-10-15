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
        public void DerTSInRelativeIntervalMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForDerived()
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
                       .ForDerived(new LocalDateTime(2019, 05, 01, 2, 0, 0))
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
                       .ForDerived()
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
                       .ForDerived()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                  .WithQueryParam("id", 100000001)
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
                       .ForDerived()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D")
                  .WithQueryParamMultiple("id", new [] { 100000001, 100000002, 100000003 })
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
                       .ForDerived()
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
        public void DerTSWithTimeZoneMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateDerived()
                       .ForMarketData(new [] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForDerived()
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
        #endregion

        #region FilterId
        [Test]
        public void DerTSInRelativeIntervalMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateDerived()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForDerived()
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
                       .ForDerived()
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
                       .ForDerived()
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
                       .ForDerived()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D")
                   .WithQueryParam("filterId", 1)
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
                       .ForDerived()
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
        public void DerTS_RelativePeriodForAnalysisDate()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateDerived()
                                    .ForMarketData(new [] { 100000001 })
                                    .InGranularity(Granularity.Hour)
                                    .ForDerived()
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
        #endregion
    }
}
