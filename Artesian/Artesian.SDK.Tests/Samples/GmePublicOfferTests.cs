using Artesian.SDK.Dto.GMEPublicOffer;
using Artesian.SDK.Service;
using Artesian.SDK.Service.GMEPublicOffer;

using NodaTime;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artesian.SDK.Tests.Samples
{
    public class GmePublicOfferTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri("https://arkive.artesian.cloud/tenantName/"), "APIKey");

        [Test]
        [Ignore("Run only manually with proper artesian URI and ApiKey set")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
        public async Task SaveGmePublicOffer()
        {
            var qs = new QueryService(_cfg);
            var poService = new GMEPublicOfferService(_cfg);

            var upsertDto = new GMEPublicOfferUpsertDataDto()
            {
                GMEPublicOffer = new List<GMEPublicOfferDto>()
                {
                    new GMEPublicOfferDto()
                    {
                        Market = Market.MGP,
                        Purpose = Purpose.BID,
                        BAType = BAType.NULL,
                        Scope = Scope.NULL,
                        Status = Status.ACC,
                        Zone = Zone.NORD,
                        Type = Dto.GMEPublicOffer.Type.REG,
                        OfferType = "FAKE",
                        BALANCED_REFERENCE_NO = "MyBalanceReferenceNumber",
                        UnitReference = string.Empty,
                        BlockId = "S",
                        Operator = "OperatorName",
                        STORAGE_SOURCE = "TheStorageSourceName",
                        MARKET_PARTECIPANT_XREF_NO = "MarketPartXRefNo",
                        Granularity = "P15M",
                        Filename = "FakeFileName",
                        Data = new List<GMEPublicOfferDataDto>()
                        {
                            new GMEPublicOfferDataDto()
                            {
                                Date = new LocalDate(1999, 01, 01),
                                Hour = 0,
                                Period = 0,
                                Quantity = 99.99M,
                                EnergyPrice = 111.11M,
                                ADJEnergyPrice = 90.09M,
                                ADJQuantity = 111.01M,
                                AwardedPrice = 9.9M,
                                AwardedQuantity = 3.3M,
                                MeritOrder = 123.456M,
                                Prodotto = "ProductNameThing",
                                GridSupplyPoint = "GridSupplyPoint",
                                Quarter = 1,
                                TransactionReference = "TransRef",
                                PartialQuantityAccepted = false,
                            }
                        }
                    }
                }
            };

            await poService.UpsertDataAsync(upsertDto);

            var gmePoResults = await poService.CreateRawCurveQuery()
                .ForDate(new LocalDate(1999, 1, 1))
                .ForPurpose(Purpose.BID)
                .ForStatus(Status.ACC)
                .ExecuteAsync();

            var data = gmePoResults.Data.FirstOrDefault();
            
            var inputData = upsertDto.GMEPublicOffer.FirstOrDefault().Data.FirstOrDefault();

            ClassicAssert.AreEqual(inputData.ADJEnergyPrice, data.ADJEnergyPrice);
            ClassicAssert.AreEqual(inputData.AwardedPrice, data.AwardedPrice);
            ClassicAssert.AreEqual(inputData.Prodotto, data.Prodotto);
        }
    }
}
