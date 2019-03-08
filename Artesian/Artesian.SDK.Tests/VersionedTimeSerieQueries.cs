﻿using System;
using NUnit.Framework;
using Artesian.SDK.Service;
using Flurl.Http.Testing;
using Artesian.SDK.Dto;
using System.Net.Http;
using NodaTime;
using Flurl;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Summary description for VersionedTimeSerieQueries
    /// </summary>
    [TestFixture]
    public class VersionedTimeSerieQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);
        private ArtesianPolicyConfig _policy = new ArtesianPolicyConfig();

        #region MarketData ids
        [Test]
        public void VerInPeriodRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativeIntervalLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeAbsoluteDateRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodRangeLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23"
                 .SetQueryParam("id", 100000001))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D"
                 .SetQueryParam("id", 100000001))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D"
                 .SetQueryParam("id", 100000001))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativeIntervalLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth"
                 .SetQueryParam("id", 100000001))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeAbsoluteDateRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23"
                 .SetQueryParam("id", 100000001))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodRangeLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D"
                .SetQueryParam("id", 100000001))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D"
                .SetQueryParam("id", 100000001))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth"
                .SetQueryParam("id", 100000001))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalVersion_Millisecond()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D"
                  .SetQueryParam("id", 100000001))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithMultipleMarketDataWindowLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D"
                  .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 }))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithMultipleMarketDataWindowLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D"
                  .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 }))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithMultipleMarketDataWindowLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth"
                  .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 }))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithMultipleMarketDataWindowMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                  .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 }))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithMultipleMarketDataWindowVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth"
                  .SetQueryParam("id", new int[] { 100000001, 100000002, 100000003 }))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D"
                  .SetQueryParam("id", 100000001)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D"
                  .SetQueryParam("id", 100000001)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth"
                  .SetQueryParam("id", 100000001)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                  .SetQueryParam("id", 100000001)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tr", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastOfDays()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastOfMonths()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastN()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneMUV()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneVersion()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForMarketData(new int[] { 100000001 })
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10"
                 .SetQueryParam("id", 100000001)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativeIntervalLastOfMonthsWithHeaders()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForMarketData(new int[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("id", 100000001))
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }

        [Test]
        public void Ver_WithMultiplePartitions()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                    .ForMarketData(new int[] {
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

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001250, 100001251, 100001252, 100001253 , 100001254,
                        100001255 , 100001256, 100001257, 100001258, 100001259,
                        100001260, 100001261, 100001262, 100001263, 100001264,
                        100001265, 100001266, 100001267, 100001268, 100001269,
                        100001270, 100001271, 100001272, 100001273, 100001274
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
                        100001275, 100001276, 100001277, 100001278, 100001279,
                        100001280, 100001281, 100001282, 100001283, 100001284,
                        100001285, 100001286, 100001287, 100001289, 100001290,
                        100001291, 100001292, 100001293, 100001294, 100001295,
                        100001296, 100001297, 100001298, 100001299, 100001301
                    })
                    .Times(1);

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D")
                    .WithVerb(HttpMethod.Get)
                    .WithQueryParamValue("id", new int[] {
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
        public void VerInPeriodRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/P-2M/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-2), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/P20D/Day/P-2M/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativeIntervalLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 3, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-03-22/2018-07-23/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeAbsoluteDateRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodRangeLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromMonths(-4), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P-4M/P20D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P5D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/2018-06-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P5D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromDays(-20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/P-1M/P5D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2018-06-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P5D"
                   .SetQueryParam("filterId", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                        .InRelativePeriodRange(Period.FromMonths(-1), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/P-1M/P20D"
                   .SetQueryParam("filterId", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativeIntervalLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeAbsoluteDateRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InAbsoluteDateRange(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/2018-06-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodRangeLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInDateRangeRelativePeriodLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfDays(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-05-22/2018-07-23/Day/P5D"
                   .SetQueryParam("filterId", 1))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                .WithVerb(HttpMethod.Get)
                .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InAbsoluteDateRange(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2018-05-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastNVersions(3)
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalVersion_Millisecond_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0, 0, 123))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00.123/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2018-07-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriod(Period.FromDays(5))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                        .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativeIntervalMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInAbsoluteDateRangeMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2018, 7, 22), new LocalDate(2018, 7, 23))
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2018-07-22/2018-07-23"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerInRelativePeriodRangeExtractionWindowMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .ForMUV()
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 6, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/2018-06-22/2018-07-23/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(new LocalDate(2018, 5, 22), new LocalDate(2018, 7, 23))
                       .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/2018-05-22/2018-07-23/Day/P2W/P20D"
                   .SetQueryParam("filterId", 1)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/RollingMonth"
                   .SetQueryParam("filterId", 1)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .InRelativePeriod(Period.FromDays(5))
                       .ForMUV()
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                   .SetQueryParam("filterId", 1)
                  .SetQueryParam("tr", 1))
                  .WithVerb(HttpMethod.Get)
                  .Times(1);
            }
        }

        [Test]
        public void VerWithTimeTransformVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .WithTimeTransform(1)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/RollingMonth"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tr", 1))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastOfDays_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-1M/P20D/Day/2017-01-01/2018-01-10"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastOfMonths_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastOfMonths(Period.FromMonths(-1), Period.FromDays(20))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-1M/P20D/Day/2017-01-01/2018-01-10"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneLastN_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForLastNVersions(3)
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/2017-01-01/2018-01-10"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneMUV_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/2017-01-01/2018-01-10"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerWithTimeZoneVersion_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var act = qs.CreateVersioned()
                       .ForFilterId(1)
                       .InGranularity(Granularity.Day)
                       .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                       .InAbsoluteDateRange(new LocalDate(2017, 1, 1), new LocalDate(2018, 1, 10))
                       .InTimezone("UTC")
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/2017-01-01/2018-01-10"
                   .SetQueryParam("filterId", 1)
                 .SetQueryParam("tz", "UTC"))
                 .WithVerb(HttpMethod.Get)
                 .Times(1);
            }
        }

        [Test]
        public void VerInPeriodRelativeIntervalLastOfMonthsWithHeaders_FilterId()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5")
                   .Times(1);
            }

            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var ver = qs.CreateVersioned()
                        .ForFilterId(1)
                        .InGranularity(Granularity.Day)
                        .ForLastOfMonths(Period.FromMonths(-4))
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                   .SetQueryParam("filterId", 1))
                   .WithVerb(HttpMethod.Get)
                   //.WithHeader
                   .Times(1);
            }
        }
        #endregion

        #region partialQueryChanges
        [Test]
        public void Ver_PeriodRelativeIntervalChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateVersioned()
                                    .ForMarketData(new int[] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastOfMonths(Period.FromMonths(-4))
                                    .InRelativeInterval(RelativeInterval.RollingMonth);

                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfMonths/P-4M/Day/RollingMonth"
                           .SetQueryParam("id", 100000001))
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = partialQuery
                            .ForLastOfDays(Period.FromDays(-20))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/LastOfDays/P-20D/Day/RollingMonth"
                           .SetQueryParam("id", 100000001))
                           .WithVerb(HttpMethod.Get)
                           .Times(1);
            }
        }

        public void Ver_RelativePeriodExtractionWindowChange()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new QueryService(_cfg);

                var partialQuery = qs.CreateVersioned()
                                    .ForMarketData(new int[] { 100000001 })
                                    .InGranularity(Granularity.Day)
                                    .ForLastNVersions(3)
                                    .InRelativePeriod(Period.FromDays(5));

                var test1 = partialQuery
                            .ExecuteAsync().Result; ;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Last3/Day/P5D"
                           .SetQueryParam("id", 100000001))
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test2 = partialQuery
                            .ForVersion(new LocalDateTime(2018, 07, 19, 12, 0))
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/Version/2018-07-19T12:00:00/Day/P5D"
                           .SetQueryParam("id", 100000001))
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

                var test3 = partialQuery
                            .ForMUV()
                            .ExecuteAsync().Result;

                httpTest.ShouldHaveCalled($"{_cfg.BaseAddress}query/v1.0/vts/MUV/Day/P5D"
                           .SetQueryParam("id", 100000001))
                           .WithVerb(HttpMethod.Get)
                           .Times(1);

            }
        }
        #endregion
    }
}
