using Artesian.SDK.Dto;
using Artesian.SDK.Factory;
using Artesian.SDK.Service;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Artesian.SDK.Tests
{
    public class DerivedTimeSeriesTest
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void CreateDerivedTimeSeries()
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
                    MarketDataId = 0
                };

                var mktData = marketDataService.GetMarketDataReference(
                    new MarketDataIdentifier(
                        marketDataEntity.ProviderName,
                        marketDataEntity.MarketDataName)
                    );

                var isRegd = mktData.IsRegistered().GetAwaiter().GetResult();

                if (!isRegd)
                    mktData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

                mktData.Load();

                curveIds.Add(mktData.MarketDataId.Value);

                var writeMarketData = mktData.EditVersioned(curve.Version);

                for (var dt = curve.Start; dt <= curve.End; dt = dt.PlusDays(1))
                {
                    writeMarketData.AddData(dt, curve.Value);
                }

                writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

                var vts = qs.CreateVersioned()
                           .ForMarketData(new[] { mktData.MarketDataId.Value })
                           .InGranularity(Granularity.Day)
                           .ForLastOfDays(curve.Start, curve.End)
                           .InAbsoluteDateRange(curve.Start, curve.End)
                           .ExecuteAsync().Result;

                ClassicAssert.AreEqual(vts.First().Value, curve.Value);
                ClassicAssert.AreEqual(vts.Last().Value, curve.Value);
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
                Type = MarketDataType.DerivedTimeSerie,
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

            marketData.Load();

            var ts = qs.CreateDerived()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForMUV()
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 01), new LocalDate(2018, 10, 10))
                       .ExecuteAsync().Result;

            foreach (var item in ts)
            {
                if (item.Time.Date <= new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(item.Value, 100);

                if (item.Time.Date > new DateTime(2018, 10, 6))
                    ClassicAssert.AreEqual(item.Value, 200);
            }

            /*******************************************************************
             * Tidy Up
             *******************************************************************/
            marketDataService.DeleteMarketDataAsync(marketData.MarketDataId.Value).GetAwaiter().GetResult();
        }
    }
}
