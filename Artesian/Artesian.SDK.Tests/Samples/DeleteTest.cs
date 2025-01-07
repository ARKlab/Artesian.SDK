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
    public class DeleteTest
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DeleteActual()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = "TestProviderName2",
                MarketDataName = "TestMarketDataName2",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.ActualTimeSerie,
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            //marketDataService.DeleteMarketDataAsync(marketData.MarketDataId.Value);

            var isRegistered = await marketData.IsRegistered();

            if (!isRegistered)
                await marketData.Register(marketDataEntity);

            await marketData.Load();

            var writeMarketData = marketData.EditActual();

            writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
            writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);
            writeMarketData.AddData(new LocalDate(2018, 10, 05), 18);
            writeMarketData.AddData(new LocalDate(2018, 10, 06), 22);

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var act = await qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(act.First().Value, 10);
            ClassicAssert.AreEqual(act.Last().Value, 22);

            await writeMarketData.Delete(new LocalDateTime(2018, 10, 05, 0, 0), new LocalDateTime(2018, 10, 07, 0, 0));

            act = await qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(act.First().Value, 10);
            ClassicAssert.AreEqual(act.Last().Value, null);


            await writeMarketData.Delete();

            act = await qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(act.First().Value, null);
            ClassicAssert.AreEqual(act.Last().Value, null);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DeleteVersion()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = "TestProviderName3",
                MarketDataName = "TestMarketDataName3",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.VersionedTimeSerie,
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegistered = await marketData.IsRegistered();

            if (!isRegistered)
                await marketData.Register(marketDataEntity);

            await marketData.Load();

            var writeMarketData = marketData.EditVersioned(new LocalDateTime(2018, 10, 03, 0, 0));

            writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
            writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);
            writeMarketData.AddData(new LocalDate(2018, 10, 05), 18);
            writeMarketData.AddData(new LocalDate(2018, 10, 06), 22);

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var ts = await qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().Value, 10);
            ClassicAssert.AreEqual(ts.Last().Value, 22);

            await writeMarketData.Delete(new LocalDateTime(2018, 10, 05, 0, 0), new LocalDateTime(2018, 10, 07, 0, 0));

            ts = await qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().Value, 10);
            ClassicAssert.AreEqual(ts.Last().Value, null);


            await writeMarketData.Delete();

            ts = await qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync();

            ClassicAssert.AreEqual(ts.Count(), 0);
        }


        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DeleteAuction()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = "TestProviderName4",
                MarketDataName = "TestMarketDataName4",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                AggregationRule = AggregationRule.AverageAndReplicate,
                Type = MarketDataType.Auction,
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegistered = await marketData.IsRegistered();

            if (!isRegistered)
                await marketData.Register(marketDataEntity);

            await marketData.Load();

            var writeMarketData = marketData.EditAuction();

            var localDate = new LocalDate(2018, 09, 24);
            var bid1 = new List<AuctionBidValue>();
            var offer1 = new List<AuctionBidValue>();
            bid1.Add(new AuctionBidValue(100, 10));
            offer1.Add(new AuctionBidValue(120, 12));
            var bid2 = new List<AuctionBidValue>();
            var offer2 = new List<AuctionBidValue>();
            bid2.Add(new AuctionBidValue(200, 20));
            offer2.Add(new AuctionBidValue(220, 22));
            var bid3 = new List<AuctionBidValue>();
            var offer3 = new List<AuctionBidValue>();
            bid3.Add(new AuctionBidValue(300, 30));
            offer3.Add(new AuctionBidValue(320, 32));

            writeMarketData.AddData(new LocalDate(2018, 09, 24), bid1.ToArray(), offer1.ToArray());
            writeMarketData.AddData(new LocalDate(2018, 09, 25), bid2.ToArray(), offer2.ToArray());
            writeMarketData.AddData(new LocalDate(2018, 09, 26), bid3.ToArray(), offer3.ToArray());

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var ts = await qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().Price, 100);
            ClassicAssert.AreEqual(ts.Last().Price, 320);

            await writeMarketData.Delete(new LocalDateTime(2018, 09, 26, 0, 0), new LocalDateTime(2018, 09, 27, 0, 0));

            ts = await qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().Price, 100);
            ClassicAssert.AreEqual(ts.Last().Price, 220);


            await writeMarketData.Delete();

            ts = await qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.Count(), 0);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DeleteMAS()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = "TestProviderName8",
                MarketDataName = "TestMarketDataName8",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.MarketAssessment,
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegistered = await marketData.IsRegistered();

            if (!isRegistered)
                await marketData.Register(marketDataEntity);

            await marketData.Load();

            var writeMarketData = marketData.EditMarketAssessment();

            var marketAssessmentValue = new MarketAssessmentValue()
            {
                High = 47,
                Close = 20,
                Low = 18,
                Open = 33,
                Settlement = 22,
                VolumePaid = 34,
                VolumeGiven = 23,
                Volume = 16

            };

            var marketAssessmentValue1 = new MarketAssessmentValue()
            {
                High = 48,
                Close = 21,
                Low = 19,
                Open = 34,
                Settlement = 23,
                VolumePaid = 35,
                VolumeGiven = 24,
                Volume = 17

            };

            var marketAssessmentValue2 = new MarketAssessmentValue()
            {
                High = 49,
                Close = 22,
                Low = 20,
                Open = 35,
                Settlement = 24,
                VolumePaid = 36,
                VolumeGiven = 25,
                Volume = 18

            };

            writeMarketData.AddData(new LocalDate(2014, 01, 01), "Jan-15", marketAssessmentValue);
            writeMarketData.AddData(new LocalDate(2014, 01, 02), "Jan-15", marketAssessmentValue1);
            writeMarketData.AddData(new LocalDate(2014, 01, 03), "Jan-15", marketAssessmentValue2);

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var ts = await qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().High, 47);
            ClassicAssert.AreEqual(ts.Last().High, 49);

            await writeMarketData.Delete(new LocalDateTime(2014, 01, 03, 0, 0), new LocalDateTime(2014, 01, 04, 0, 0), new List<string>() { "Jan-15" });

            ts = await qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().High, 47);
            ClassicAssert.AreEqual(ts.Last().High, null);


            await writeMarketData.Delete(product: new List<string>() { "Jan-15" }, deferCommandExecution: false, deferDataGeneration: false);

            ts = await qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().High, null);
            ClassicAssert.AreEqual(ts.Last().High, null);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public async Task DeleteBidAsk()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = "TestProviderName9",
                MarketDataName = "TestMarketDataName9",
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = "CET",
                Type = MarketDataType.BidAsk,
                MarketDataId = 0
            };

            var marketData = marketDataService.GetMarketDataReference(new MarketDataIdentifier(
                    marketDataEntity.ProviderName,
                    marketDataEntity.MarketDataName)
                );

            var isRegistered = await marketData.IsRegistered();

            if (!isRegistered)
                await marketData.Register(marketDataEntity);

            await marketData.Load();

            var writeMarketData = marketData.EditBidAsk();

            var bidAskValue = new BidAskValue()
            {
                BestBidPrice = 47,
                BestBidQuantity = 18,
                BestAskPrice = 20,
                BestAskQuantity = 33,
                LastPrice = 22,
                LastQuantity = 13
            };

            var bidAskValue1 = new BidAskValue()
            {
                BestBidPrice = 48,
                BestBidQuantity = 18,
                BestAskPrice = 20,
                BestAskQuantity = 33,
                LastPrice = 22,
                LastQuantity = 13
            };

            var bidAskValue2 = new BidAskValue()
            {
                BestBidPrice = 49,
                BestBidQuantity = 18,
                BestAskPrice = 20,
                BestAskQuantity = 33,
                LastPrice = 22,
                LastQuantity = 13
            };

            writeMarketData.AddData(new LocalDate(2014, 01, 01), "Jan-15", bidAskValue);
            writeMarketData.AddData(new LocalDate(2014, 01, 02), "Jan-15", bidAskValue1);
            writeMarketData.AddData(new LocalDate(2014, 01, 03), "Jan-15", bidAskValue2);

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var ts = await qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().BestBidPrice, 47);
            ClassicAssert.AreEqual(ts.Last().BestBidPrice, 49);

            await writeMarketData.Delete(new LocalDateTime(2014, 01, 03, 0, 0), new LocalDateTime(2014, 01, 04, 0, 0), new List<string>() { "Jan-15" });

            ts = await qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().BestBidPrice, 47);
            ClassicAssert.AreEqual(ts.Last().BestBidPrice, null);


            await writeMarketData.Delete(product: new List<string>() { "Jan-15" });

            ts = await qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync();

            ClassicAssert.AreEqual(ts.First().BestBidPrice, null);
            ClassicAssert.AreEqual(ts.Last().BestBidPrice, null);
        }
    }
}
