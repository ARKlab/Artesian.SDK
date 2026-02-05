using Artesian.SDK.Dto;
using Artesian.SDK.Factory;
using Artesian.SDK.Service;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artesian.SDK.Tests.Samples
{
    public class MasTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateMasTimeSeriesAndUpdateWithMergeMode()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var providerName = "TestMasProvider";
            var curveName = "MasCurveMerge";
            var date = new LocalDate(2025, 1, 1);
            var value1 = 99.99;
            var products1 = new List<string> { "Feb-25", "Mar-25", "Apr-25", "2026", "2027" };
            var value2 = 55.55;
            var products2 = new List<string> { "May-25" };
            var originalTimezone = "CET";
            var originalGranularity = Granularity.Day;
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = originalGranularity,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.Undefined,
                Type = MarketDataType.MarketAssessment,
                MarketDataId = 0,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegd = await mktData.IsRegistered();

            if (!isRegd)
                await mktData.Register(marketDataEntity);

            await mktData.Load();

            //SET CURVEID
            curveId = mktData.MarketDataId!.Value;

            var writableMas = mktData.EditMarketAssessment();

            //CHECK UPSERT MODE MERGE
            foreach (var p in products1)
            {
                writableMas.TryAddData(date, p, new MarketAssessmentValue() 
                { 
                    Settlement = value1,
                    Volume = value1
                });
            }

            await writableMas.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            await Task.Delay(1000);

            var writableMasForMerge = mktData.EditMarketAssessment();
            foreach (var p in products2)
            {
                writableMasForMerge.TryAddData(date, p, new MarketAssessmentValue() 
                { 
                    Settlement = value2,
                    Volume = value2
                });
            }

            await writableMasForMerge.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            var prodsIn = products1.Concat(products2);

            var results = await qs.CreateMarketAssessment()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .ForProducts(prodsIn.ToArray())
                        .InAbsoluteDateRange(date, date.PlusDays(1))
                        .ExecuteAsync();

            var prodsOut = results.Where(x => x.Settlement.HasValue).Select(x => x.Product);
            
            ClassicAssert.AreEqual(prodsIn, prodsOut);


            await marketDataService.DeleteMarketDataAsync(curveId);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateMasTimeSeriesAndUpdateWithReplaceMode()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var providerName = "TestMasProvider";
            var curveName = "MasCurveReplace";
            var date = new LocalDate(2025, 1, 1);
            var value1 = 99.99;
            var products1 = new List<string> { "Feb-25", "Mar-25", "Apr-25", "2026", "2027" };
            var value2 = 55.55;
            var products2 = new List<string> { "May-25" };
            var originalTimezone = "CET";
            var originalGranularity = Granularity.Day;
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = originalGranularity,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.Undefined,
                Type = MarketDataType.MarketAssessment,
                MarketDataId = 0,
            };

            var mktData = marketDataService.GetMarketDataReference(
                new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegd = await mktData.IsRegistered();

            if (!isRegd)
                await mktData.Register(marketDataEntity);

            await mktData.Load();

            //SET CURVEID
            curveId = mktData.MarketDataId!.Value;

            var writableMas = mktData.EditMarketAssessment();

            //CHECK UPSERT MODE MERGE
            foreach (var p in products1)
            {
                writableMas.TryAddData(date, p, new MarketAssessmentValue()
                {
                    Settlement = value1,
                    Volume = value1
                });
            }

            await writableMas.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            await Task.Delay(1000);

            var writableMasForReplace = mktData.EditMarketAssessment();
            foreach (var p in products2)
            {
                writableMasForReplace.TryAddData(date, p, new MarketAssessmentValue()
                {
                    Settlement = value2,
                    Volume = value2
                });
            }

            await writableMasForReplace.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()), upsertMode: UpsertMode.Replace); // default upsertmode = Replace

            var prodsIn = products1.Concat(products2);

            var results = await qs.CreateMarketAssessment()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .ForProducts(prodsIn.ToArray())
                        .InAbsoluteDateRange(date, date.PlusDays(1))
                        .ExecuteAsync();

            var prodsOut = results.Where(x => x.Settlement.HasValue).Select(x => x.Product);

            ClassicAssert.AreEqual(products2.AsEnumerable(), prodsOut);


            await marketDataService.DeleteMarketDataAsync(curveId);
        }

    }
}
