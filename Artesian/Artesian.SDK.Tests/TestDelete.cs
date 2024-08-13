using Artesian.SDK.Dto;
using Artesian.SDK.Factory;
using Artesian.SDK.Service;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Tests
{
    public class TestDelete
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void DeleteActual()
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

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load();

            var writeMarketData = marketData.EditActual();

            writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
            writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);
            writeMarketData.AddData(new LocalDate(2018, 10, 05), 18);
            writeMarketData.AddData(new LocalDate(2018, 10, 06), 22);

            writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

            var act = qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(act.First().Value, Is.EqualTo(10));
            Assert.That(act.Last().Value, Is.EqualTo(22));

            writeMarketData.Delete(new LocalDateTime(2018, 10, 05, 0, 0), new LocalDateTime(2018, 10, 07, 0, 0)).ConfigureAwait(true).GetAwaiter().GetResult();

            act = qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(act.First().Value, Is.EqualTo(10));
            Assert.That(act.Last().Value, Is.Null);


            writeMarketData.Delete().ConfigureAwait(true).GetAwaiter().GetResult();

            act = qs.CreateActual()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(act.First().Value, null);
            Assert.That(act.Last().Value, null);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void DeleteVersion()
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

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load();

            var writeMarketData = marketData.EditVersioned(new LocalDateTime(2018, 10, 03, 0, 0));

            writeMarketData.AddData(new LocalDate(2018, 10, 03), 10);
            writeMarketData.AddData(new LocalDate(2018, 10, 04), 15);
            writeMarketData.AddData(new LocalDate(2018, 10, 05), 18);
            writeMarketData.AddData(new LocalDate(2018, 10, 06), 22);

            writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

            var ts = qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(ts.First().Value, Is.EqualTo(10));
            Assert.That(ts.Last().Value, Is.EqualTo(22));

            writeMarketData.Delete(new LocalDateTime(2018, 10, 05, 0, 0), new LocalDateTime(2018, 10, 07, 0, 0)).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(ts.First().Value, Is.EqualTo(10));
            Assert.That(ts.Last().Value, Is.Null);


            writeMarketData.Delete().ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateVersioned()
                       .ForMarketData(new[] { marketData.MarketDataId.Value })
                       .InGranularity(Granularity.Day)
                       .ForLastOfDays(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .InAbsoluteDateRange(new LocalDate(2018, 10, 03), new LocalDate(2018, 10, 07))
                       .ExecuteAsync().Result;

            Assert.That(ts.Count(), Is.EqualTo(0));
        }


        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void DeleteAuction()
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

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load();

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

            writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

            var ts = qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().Price, Is.EqualTo(100));
            Assert.That(ts.Last().Price, Is.EqualTo(320));

            writeMarketData.Delete(new LocalDateTime(2018, 09, 26, 0, 0), new LocalDateTime(2018, 09, 27, 0, 0)).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().Price, Is.EqualTo(100));
            Assert.That(ts.Last().Price, Is.EqualTo(220));


            writeMarketData.Delete().ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateAuction()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .InAbsoluteDateRange(new LocalDate(2018, 09, 24), new LocalDate(2018, 09, 27))
                        .ExecuteAsync().Result;

            Assert.That(ts.Count(), Is.EqualTo(0));
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void DeleteMAS()
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

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load();

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

            writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

            var ts = qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().High, Is.EqualTo(47));
            Assert.That(ts.Last().High, Is.EqualTo(49));

            writeMarketData.Delete(new LocalDateTime(2014, 01, 03, 0, 0), new LocalDateTime(2014, 01, 04, 0, 0), new List<string>() { "Jan-15" }).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().High, Is.EqualTo(47));
            Assert.That(ts.Last().High, Is.Null);


            writeMarketData.Delete(product: new List<string>() { "Jan-15" }, deferCommandExecution: false, deferDataGeneration: false).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateMarketAssessment()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().High, Is.Null);
            Assert.That(ts.Last().High, Is.Null);
        }

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        public void DeleteBidAsk()
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

            var isRegistered = marketData.IsRegistered().GetAwaiter().GetResult();

            if (!isRegistered)
                marketData.Register(marketDataEntity).ConfigureAwait(true).GetAwaiter().GetResult();

            marketData.Load();

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

            writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime())).ConfigureAwait(true).GetAwaiter().GetResult();

            var ts = qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().BestBidPrice, Is.EqualTo(47));
            Assert.That(ts.Last().BestBidPrice, Is.EqualTo(49));

            writeMarketData.Delete(new LocalDateTime(2014, 01, 03, 0, 0), new LocalDateTime(2014, 01, 04, 0, 0), new List<string>() { "Jan-15" }).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().BestBidPrice, Is.EqualTo(47));
            Assert.That(ts.Last().BestBidPrice, Is.Null);


            writeMarketData.Delete(product: new List<string>() { "Jan-15" }).ConfigureAwait(true).GetAwaiter().GetResult();

            ts = qs.CreateBidAsk()
                        .ForMarketData(new int[] { marketData.MarketDataId.Value })
                        .ForProducts(new string[] { "Jan-15" })
                        .InAbsoluteDateRange(new LocalDate(2014, 01, 01), new LocalDate(2014, 01, 04))
                        .ExecuteAsync().Result;

            Assert.That(ts.First().BestBidPrice, Is.Null);
            Assert.That(ts.Last().BestBidPrice, Is.Null);
        }
    }
}
