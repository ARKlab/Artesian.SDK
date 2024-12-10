﻿using Artesian.SDK.Dto;
using Artesian.SDK.Dto.DerivedCfg;
using Artesian.SDK.Factory;
using Artesian.SDK.Service;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Artesian.SDK.Tests.Samples
{
    public class DerivedTimeSerieTest
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");
        
        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void CreateDerivedCoalesceTimeSeries()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            /*******************************************************************
             * Register Versioned curves to use in the Derived curve definition
             *******************************************************************/
            var providerName = "TestProviderNameDerived";
            var originalTimezone = "CET";
            var curveIds = new List<int>();
            var curvesToBuild = new List<(string Name, LocalDate Start, LocalDate End, double Value, LocalDateTime Version)>
            {
                ("CurveOne", new LocalDate(2018, 10, 01), new LocalDate(2018, 10, 06), 100, new LocalDateTime(2018, 10, 04, 0, 0)),
                ("CurveTwo", new LocalDate(2018, 10, 04), new LocalDate(2018, 10, 09), 200, new LocalDateTime(2018, 10, 06, 0, 0)),
            };

            foreach (var curve in curvesToBuild)
            {
                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = providerName,
                    MarketDataName = curve.Name,
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = originalTimezone,
                    AggregationRule = AggregationRule.AverageAndReplicate,
                    Type = MarketDataType.VersionedTimeSerie,
                    MarketDataId = 0,
                };

                var mktData = marketDataService.GetMarketDataReference(
                    new MarketDataIdentifier(
                        marketDataEntity.ProviderName,
                        marketDataEntity.MarketDataName)
                    );

                var isRegd = mktData.IsRegistered().GetAwaiter().GetResult();

                if (!isRegd)
                    mktData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                mktData.Load().GetAwaiter().GetResult();

                curveIds.Add(mktData.MarketDataId.Value);

                var writeMarketData = mktData.EditVersioned(curve.Version);

                for (var dt = curve.Start; dt <= curve.End; dt = dt.PlusDays(1))
                {
                    writeMarketData.AddData(dt, curve.Value);
                }

                writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

                var dts = qs.CreateActual()
                           .ForMarketData(new[] { mktData.MarketDataId.Value })
                           .InGranularity(Granularity.Day)
                           .InAbsoluteDateRange(curve.Start, curve.End)
                           .ExecuteAsync().Result;

                ClassicAssert.AreEqual(curve.Value, dts.First().Value);
                ClassicAssert.AreEqual(curve.Value, dts.Last().Value);
            }

            /*******************************************************************
             * Register Derived curve
             *******************************************************************/
            var marketDataEntityDerived = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = "DerivedCurveOne",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.ActualTimeSerie,
                DerivedCfg = new DerivedCfgCoalesce()
                {
                    OrderedReferencedMarketDataIds = curveIds.ToArray(),
                    Version = 1,
                },
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    marketDataEntityDerived.ProviderName,
                    marketDataEntityDerived.MarketDataName)
                );

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntityDerived).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load().GetAwaiter().GetResult();

            Thread.Sleep(2000);

            var ts = qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 01), new LocalDate(2018, 10, 10))
                       .ExecuteAsync().Result;

            foreach (var item in ts)
            {
                if (item.Time.Date <= new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(100, item.Value);

                if (item.Time.Date > new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(200, item.Value);
            }

            /*******************************************************************
             * Tidy Up
             *******************************************************************/
            foreach (var curve in curveIds)
            {
                marketDataService.DeleteMarketDataAsync(curve).GetAwaiter().GetResult();
            }
            marketDataService.DeleteMarketDataAsync(marketData.MarketDataId.Value).GetAwaiter().GetResult();
        }


        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void CreateDerivedSumTimeSeries()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            /*******************************************************************
             * Register Versioned curves to use in the Derived curve definition
             *******************************************************************/
            var providerName = "TestProviderNameDerivedSum";
            var originalTimezone = "CET";
            var curveIds = new List<int>();
            var curvesToBuild = new List<(string Name, LocalDate Start, LocalDate End, double Value, LocalDateTime Version)>
            {
                ("CurveOne", new LocalDate(2018, 10, 01), new LocalDate(2018, 10, 06), 100, new LocalDateTime(2018, 10, 04, 0, 0)),
                ("CurveTwo", new LocalDate(2018, 10, 04), new LocalDate(2018, 10, 09), 200, new LocalDateTime(2018, 10, 06, 0, 0)),
            };

            foreach (var curve in curvesToBuild)
            {
                var marketDataEntity = new MarketDataEntity.Input()
                {
                    ProviderName = providerName,
                    MarketDataName = curve.Name,
                    OriginalGranularity = Granularity.Day,
                    OriginalTimezone = originalTimezone,
                    AggregationRule = AggregationRule.AverageAndReplicate,
                    Type = MarketDataType.VersionedTimeSerie,
                    MarketDataId = 0,
                };

                var mktData = marketDataService.GetMarketDataReference(
                    new MarketDataIdentifier(
                        marketDataEntity.ProviderName,
                        marketDataEntity.MarketDataName)
                    );

                var isRegd = mktData.IsRegistered().GetAwaiter().GetResult();

                if (!isRegd)
                    mktData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                mktData.Load().GetAwaiter().GetResult();

                curveIds.Add(mktData.MarketDataId.Value);

                var writeMarketData = mktData.EditVersioned(curve.Version);

                for (var dt = curve.Start; dt <= curve.End; dt = dt.PlusDays(1))
                {
                    writeMarketData.AddData(dt, curve.Value);
                }

                writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

                var dts = qs.CreateActual()
                           .ForMarketData(new[] { mktData.MarketDataId.Value })
                           .InGranularity(Granularity.Day)
                           .InAbsoluteDateRange(curve.Start, curve.End)
                           .ExecuteAsync().Result;

                ClassicAssert.AreEqual(curve.Value, dts.First().Value);
                ClassicAssert.AreEqual(curve.Value, dts.Last().Value);
            }

            /*******************************************************************
             * Register Derived curve
             *******************************************************************/
            var marketDataEntityDerived = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = "DerivedCurveTwo",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.ActualTimeSerie,
                DerivedCfg = new DerivedCfgSum()
                {
                    OrderedReferencedMarketDataIds = curveIds.ToArray(),
                    Version = 1,
                },
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    marketDataEntityDerived.ProviderName,
                    marketDataEntityDerived.MarketDataName)
                );

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntityDerived).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load().GetAwaiter().GetResult();

            Thread.Sleep(2000);

            var ts = qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 01), new LocalDate(2018, 10, 10))
                       .ExecuteAsync().Result;

            foreach (var item in ts)
            {
                if (item.Time.Date <= new DateTime(2018, 10, 3))
                    ClassicAssert.AreEqual(100, item.Value);
                else if (item.Time.Date > new DateTime(2018, 10, 3) && item.Time.Date <= new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(100+200, item.Value);
                else// if (item.Time.Date > new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(200, item.Value);
            }

            /*******************************************************************
             * Tidy Up
             *******************************************************************/
            foreach (var curve in curveIds)
            {
                marketDataService.DeleteMarketDataAsync(curve).GetAwaiter().GetResult();
            }
            marketDataService.DeleteMarketDataAsync(marketData.MarketDataId.Value).GetAwaiter().GetResult();
        }
    }
}
