using Artesian.SDK.Dto;
using Artesian.SDK.Factory;
using Artesian.SDK.Service;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Artesian.SDK.Tests.Samples
{
    public class AuctionTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task CreateAuctionTimeSeries()
        {
            var qs = new QueryService(_cfg);
            var marketDataService = new MarketDataService(_cfg);

            /*******************************************************************
             * Register Auction curves
             *******************************************************************/
            var providerName = "TestProviderNameAuction";
            var curveName = "AuctionCurveOne";
            var start = new LocalDate(2025, 1, 1);
            var end = new LocalDate(2025, 1, 5);
            var originalTimezone = "CET";
            var curveId = 0;

            var marketDataEntity = new MarketDataEntity.Input()
            {
                ProviderName = providerName,
                MarketDataName = curveName,
                OriginalGranularity = Granularity.Day,
                OriginalTimezone = originalTimezone,
                AggregationRule = AggregationRule.Undefined,
                Type = MarketDataType.Auction,
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
            curveId = mktData.MarketDataId.Value;


            var bids = new AuctionBidValue[]
            {
                new AuctionBidValue(9, 1, 90, 10),
                new AuctionBidValue(8, 2, 80, 20, BlockType.Block),
                new AuctionBidValue(7, 3, 70, 30, BlockType.Single),
                new AuctionBidValue(6, 4, 60, 40),
                new AuctionBidValue(5, 5, 50, 50),
                new AuctionBidValue(4, 6, 40, 60),
                new AuctionBidValue(3, 7, 30, 70, BlockType.Single),
                new AuctionBidValue(2, 8, 20, 80, BlockType.Block),
                new AuctionBidValue(1, 9, 10, 90),
            };
            var offs = new AuctionBidValue[]
            {
                new AuctionBidValue(1, 9, 10, 90),
                new AuctionBidValue(2, 8, 20, 80, BlockType.Block),
                new AuctionBidValue(3, 7, 30, 70, BlockType.Single),
                new AuctionBidValue(4, 6, 40, 60),
                new AuctionBidValue(5, 5, 50, 50),
                new AuctionBidValue(6, 4, 60, 40),
                new AuctionBidValue(7, 3, 70, 30, BlockType.Single),
                new AuctionBidValue(8, 2, 80, 20, BlockType.Block),
                new AuctionBidValue(9, 1, 90, 10),
            };

            var writeMarketData = mktData.EditAuction();

            for (var dt = start; dt <= end; dt = dt.PlusDays(1))
            {
                writeMarketData.AddData(dt, bids, offs);
            }

            await writeMarketData.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));

            var auctionResults = await qs.CreateAuction()
                        .ForMarketData(new[] { mktData.MarketDataId.Value })
                        .InAbsoluteDateRange(start, end)
                        .ExecuteAsync();
            
            var firstAuctionReult = auctionResults.FirstOrDefault();

            ClassicAssert.AreEqual(1, firstAuctionReult.Quantity);
            ClassicAssert.AreEqual(10, firstAuctionReult.AcceptedQuantity);
            ClassicAssert.AreEqual(BlockType.Single, firstAuctionReult.BlockType);

            /*******************************************************************
             * Tidy Up
             *******************************************************************/
            await marketDataService.DeleteMarketDataAsync(curveId);
        }

    }
}
