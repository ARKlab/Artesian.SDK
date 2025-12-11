using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Artesian.SDK.Dto.GMEPublicOffer;
using Artesian.SDK.Service;
using Artesian.SDK.Service.GMEPublicOffer;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class GMEPublicOfferQueries
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region Raw PO curve query
        [Test]
        public async Task ExtractRawCurveBasic()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019,1,1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/extract/2019-01-01/OFF/INC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ExtractRawCurve()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019, 1, 1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ForGenerationType(new GenerationType[] { GenerationType.GAS })
                       .ForBAType(new BAType[] { BAType.REV })
                       .ForMarket(new Market[] { Market.MB4 })
                       .ForOperator(new [] { "op1" })
                       .ForScope(new Scope[] { Scope.GR1 })
                       .ForUnit(new [] { "unit1" })
                       .ForUnitType(new UnitType[] { UnitType.UP })
                       .ForZone(new Zone[] { Zone.CNOR })
                       .WithPagination(2,20)
                       .WithSort(new [] { "id asc" })
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/extract/2019-01-01/OFF/INC")
                    .WithQueryParam("generationType", "GAS")
                    .WithQueryParam("baType", "REV")
                    .WithQueryParam("market", "MB4")
                    .WithQueryParam("operators", "op1")
                    .WithQueryParam("scope", "GR1")
                    .WithQueryParam("unit", "unit1")
                    .WithQueryParam("unitType", "UP")
                    .WithQueryParam("zone", "CNOR")
                    .WithQueryParam("page", "2")
                    .WithQueryParam("pageSize", "20")
                    .WithQueryParam("sort", "id asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ExtractRawCurveXBID()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019, 1, 1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ForGenerationType(new GenerationType[] { GenerationType.GAS })
                       .ForBAType(new BAType[] { BAType.REV })
                       .ForMarket(new Market[] { Market.MIXBID })
                       .ForOperator(new[] { "op1" })
                       .ForScope(new Scope[] { Scope.GR1 })
                       .ForUnit(new[] { "unit1" })
                       .ForUnitType(new UnitType[] { UnitType.UP })
                       .ForZone(new Zone[] { Zone.CNOR })
                       .WithPagination(2, 20)
                       .WithSort(new[] { "id asc" })
                       .ExecuteAsync();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/extract/2019-01-01/OFF/INC")
                    .WithQueryParam("generationType", "GAS")
                    .WithQueryParam("baType", "REV")
                    .WithQueryParam("market", "MIXBID")
                    .WithQueryParam("operators", "op1")
                    .WithQueryParam("scope", "GR1")
                    .WithQueryParam("unit", "unit1")
                    .WithQueryParam("unitType", "UP")
                    .WithQueryParam("zone", "CNOR")
                    .WithQueryParam("page", "2")
                    .WithQueryParam("pageSize", "20")
                    .WithQueryParam("sort", "id asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        #endregion


        #region Metadata 
        [Test]
        public async Task ReadOperatorsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.ReadOperatorsAsync(2,20)
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/enums/operators")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ReadOperatorsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.ReadOperatorsAsync(2, 20, operatorFilter: "myFilter", sort: new[] { "operator asc" })
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/enums/operators")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("operatorFilter", "myFilter")
                    .WithQueryParam("sort", "operator asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ReadUnitsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.ReadUnitsAsync(2, 4)
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/enums/units")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 4)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ReadUnitsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = await qs.ReadUnitsAsync(2, 20, unitFilter: "myFilter", sort: new[] { "unit asc" })
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/enums/units")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("unitFilter", "myFilter")
                    .WithQueryParam("sort", "unit asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }


        [Test]
        public async Task ReadUnitConfigurationMappingVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var req = await qs.ReadUnitConfigurationMappingAsync("myUnit")
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/unitconfigurationmappings/myUnit")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ReadUnitConfigurationMappingsVar1()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = await qs.ReadUnitConfigurationMappingsAsync(1, 20)
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/unitconfigurationmappings")
                    .WithQueryParam("page", 1)
                    .WithQueryParam("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task ReadUnitConfigurationMappingsVar2()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = await qs.ReadUnitConfigurationMappingsAsync(1, 20, unitFilter: "unitFilterTest", sort: new[] { "unit desc" })
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/unitconfigurationmappings")
                    .WithQueryParam("page", 1)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("unitFilter", "unitFilterTest")
                    .WithQueryParam("sort", "unit desc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public async Task UpsertUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var unitCfg = new UnitConfigurationDto("unitName") {
                    Mappings = new List<GenerationTypeMapping>() {
                        new GenerationTypeMapping(){
                            From = new LocalDate(2019,1,1),
                            To = new LocalDate(2020,1,1),
                            GenerationType = GenerationType.COAL
                        }
                    }
                };

                var req = await qs.UpsertUnitConfigurationMappingAsync(unitCfg)
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/unitconfigurationmappings/{unitCfg.Unit}")
                    .WithVerb(HttpMethod.Put)
                    .Times(1);
            }
        }

        [Test]
        public async Task DeleteUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                await qs.DeleteUnitConfigurationMappingAsync("unitToDelete")
                       ;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/unitconfigurationmappings/unitToDelete")
                    .WithVerb(HttpMethod.Delete)
                    .Times(1);
            }
        }

        #endregion

        #region UpsertGME
        [Test]
        public async Task UpsertDataAsync()
        {
            using (var httpTest = new HttpTest())
            {
                var gmeService = new GMEPublicOfferService(_cfg);

                var gmePublicOFfer = new GMEPublicOfferDto()
                {
                      Purpose = Purpose.BID,
                      Type = Dto.GMEPublicOffer.Type.REG,
                      Status = Status.ACC,
                      Market = Market.MGP,
                      UnitReference = "Something",
                      Zone = Zone.AUST,
                      Operator = "SomethingAgain",
                      Data = new List<GMEPublicOfferDataDto>()
                      {
                          new GMEPublicOfferDataDto()
                          {
                              Date = new LocalDate(2020, 1, 1),
                              Hour = 1,
                              Quarter = 1,
                              Quantity = 100,
                              AwardedQuantity = 50,
                              AwardedPrice = 45.5m,
                              EnergyPrice = 45m,
                              MeritOrder = 1,
                              PartialQuantityAccepted = false,
                              ADJQuantity = 0,
                              ADJEnergyPrice = 0
                          }
                      }
                };

                var gmeData = new GMEPublicOfferUpsertDataDto()
                {
                    GMEPublicOffer = new List<GMEPublicOfferDto>()
                    {
                        gmePublicOFfer
                    }
                };

                await gmeService.UpsertDataAsync(gmeData);

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v2.0/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion
    }
}
