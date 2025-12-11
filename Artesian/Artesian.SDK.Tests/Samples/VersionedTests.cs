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
    public class VersionedTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateVersionedTimeSeriesAndUpdateWithMergeMode()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var providerName = "TestVersionProvider";
            var curveName = "VersionedCurveOne";
            var start1 = new LocalDate(2025, 1, 1);
            var end1 = new LocalDate(2025, 1, 10);
            var value1 = 99.99;
            var start2 = new LocalDate(2025, 1, 8);
            var end2 = new LocalDate(2025, 1, 16);
            var value2 = 55.55;
            var versionName = new LocalDateTime(2025, 1, 1, 12, 0);
            var originalTimezone = "CET";
            var originalGranularity = Granularity.Day;
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = originalGranularity,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.VersionedTimeSerie,
                MarketDataId = 0,
            };

            var values = new Dictionary<DateTimeOffset, double?>();

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

            var writeMarketData = mktData.EditVersioned(versionName);

            //CHECK UPSERT MODE MERGE
            for (var dt = start1; dt <= end1; dt = dt.PlusDays(1))
            {
                writeMarketData.AddData(dt, value1);
                values.Add(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset(), value1);
            }

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            await Task.Delay(1000);

            var writeMarketData2 = mktData.EditVersioned(versionName);
            for (var dt = start2; dt <= end2; dt = dt.PlusDays(1))
            {
                writeMarketData2.AddData(dt, value2);
                if (values.ContainsKey(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset()))
                    values[dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset()] = value2;
                else
                    values.Add(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset(), value2);

            }

            await writeMarketData2.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            var vtsResults = await qs.CreateVersioned()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .InGranularity(originalGranularity)
                        .ForVersion(versionName)
                        .InAbsoluteDateRange(start1, end2.PlusDays(1))
                        .ExecuteAsync();
            
            foreach (var v in values)
            {
                var testResult = vtsResults.Where(x => x.Time == v.Key);

                ClassicAssert.AreEqual(v.Value, testResult.First().Value);
            }

            await marketDataService.DeleteMarketDataAsync(curveId);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateVersionedTimeSeriesAndUseAddRange()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var providerName = "TestVersionProvider";
            var curveName = "VersionedCurveOne";
            var start1 = new LocalDate(2025, 1, 1);
            var end1 = new LocalDate(2025, 1, 10);
            var value1 = 99.99;
            var start2 = new LocalDate(2025, 1, 8);
            var end2 = new LocalDate(2025, 1, 16);
            var value2 = 55.55;
            var versionName = new LocalDateTime(2025, 1, 1, 12, 0);
            var originalTimezone = "CET";
            var originalGranularity = Granularity.Day;
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = originalGranularity,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.VersionedTimeSerie,
                MarketDataId = 0,
            };

            var values = new Dictionary<LocalDate, double?>();

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
            curveId = mktData.MarketDataId.Value;

            var writeMarketData = mktData.EditVersioned(versionName);

            //CHECK UPSERT MODE MERGE
            for (var dt = start1; dt <= end1; dt = dt.PlusDays(1))
            {
                values.Add(dt, value1);
            }

            writeMarketData.AddRange(values);

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            await Task.Delay(1000);

            var valuesNew = new Dictionary<LocalDate, double?>();

            var writeMarketData2 = mktData.EditVersioned(versionName);
            for (var dt = start2; dt <= end2; dt = dt.PlusDays(1))
            {
                if (values.ContainsKey(dt))
                    values[dt] = value2;
                else
                    values.Add(dt, value2);

                valuesNew.Add(dt, value2);
            }
            
            writeMarketData2.AddRange(valuesNew);

            await writeMarketData2.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            var vtsResults = await qs.CreateVersioned()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .InGranularity(originalGranularity)
                        .ForVersion(versionName)
                        .InAbsoluteDateRange(start1, end2.PlusDays(1))
                        .ExecuteAsync();

            foreach (var v in values)
            {
                var testResult = vtsResults.Where(x => x.Time == v.Key.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset());

                ClassicAssert.AreEqual(v.Value, testResult.FirstOrDefault().Value);
            }

            await marketDataService.DeleteMarketDataAsync(curveId);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateVersionedTimeSeriesAndUpdateWithReplaceMode()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var providerName = "TestVersionProvider";
            var curveName = "VersionedCurveTwo";
            var start1 = new LocalDate(2025, 1, 1);
            var end1 = new LocalDate(2025, 1, 10);
            var value1 = 99.99;
            var start2 = new LocalDate(2025, 1, 8);
            var end2 = new LocalDate(2025, 1, 16);
            var value2 = 55.55;
            var versionName = new LocalDateTime(2025, 1, 1, 12, 0);
            var originalTimezone = "CET";
            var originalGranularity = Granularity.Day;
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = originalGranularity,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.VersionedTimeSerie,
                MarketDataId = 0,
            };

            var values = new Dictionary<DateTimeOffset, double?>();

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

            var writeMarketData = mktData.EditVersioned(versionName);

            //CHECK UPSERT MODE REPLACE
            for (var dt = start1; dt <= end1; dt = dt.PlusDays(1))
            {
                writeMarketData.AddData(dt, value1);
                //these will be replaced so set them to null in the comparison set
                values.Add(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset(), null); 
            }

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())); // default upsertmode = Merge

            await Task.Delay(1000);

            var writeMarketData2 = mktData.EditVersioned(versionName);
            for (var dt = start2; dt <= end2; dt = dt.PlusDays(1))
            {
                writeMarketData2.AddData(dt, value2);
                if (values.ContainsKey(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset()))
                    values[dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset()] = value2;
                else
                    values.Add(dt.AtStartOfDayInZone(DateTimeZoneProviders.Tzdb[originalTimezone]).ToDateTimeOffset(), value2);
            }

            await writeMarketData2.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()), upsertMode: UpsertMode.Replace); // upsertmode = Replace

            var vtsResults = await qs.CreateVersioned()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .InGranularity(originalGranularity)
                        .ForVersion(versionName)
                        .InAbsoluteDateRange(start1, end2.PlusDays(1))
                        .ExecuteAsync();

            foreach (var v in values)
            {
                var testResult = vtsResults.Where(x => x.Time == v.Key);

                ClassicAssert.AreEqual(v.Value, testResult.First().Value);
            }

            await marketDataService.DeleteMarketDataAsync(curveId);
        }

    }
}
