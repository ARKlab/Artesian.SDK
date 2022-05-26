using System;
using System.Collections.Generic;
using System.Net.Http;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.GMEPublicOffer;
using Artesian.SDK.Service;
using Artesian.SDK.Service.GMEPublicOffer;
using Flurl;
using Flurl.Http.Testing;
using NodaTime;
using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    [TestFixture]
    public class GMEPublicOfferQueries
    {
        private ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        #region Raw PO curve query
        [Test]
        public void ExtractRawCurveBasic()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.CreateRawCurveQuery()
                       .ForDate(new LocalDate(2019,1,1))
                       .ForPurpose(Purpose.OFF)
                       .ForStatus(Status.INC)
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/extract/2019-01-01/OFF/INC")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ExtractRawCurve()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.CreateRawCurveQuery()
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
                       .ExecuteAsync().Result;

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/extract/2019-01-01/OFF/INC")
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

        #endregion


        #region Metadata 
        [Test]
        public void ReadOperatorsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadOperatorsAsync(2,20)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/operators")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadOperatorsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadOperatorsAsync(2, 20, operatorFilter: "myFilter", sort: new [] { "operator asc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/operators")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("operatorFilter", "myFilter")
                    .WithQueryParam("sort", "operator asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitsEnumVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadUnitsAsync(2, 4)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/units")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 4)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitsEnumVar2()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var act = qs.ReadUnitsAsync(2, 20, unitFilter: "myFilter", sort: new [] { "unit asc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/enums/units")
                    .WithQueryParam("page", 2)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("unitFilter", "myFilter")
                    .WithQueryParam("sort", "unit asc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }


        [Test]
        public void ReadUnitConfigurationMappingVar1()
        {
            using (var httpTest = new HttpTest())
            {

                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingAsync("myUnit")
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/myUnit")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitConfigurationMappingsVar1()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingsAsync(1, 20)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings")
                    .WithQueryParam("page", 1)
                    .WithQueryParam("pageSize", 20)
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void ReadUnitConfigurationMappingsVar2()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var req = qs.ReadUnitConfigurationMappingsAsync(1, 20, unitFilter: "unitFilterTest", sort: new [] { "unit desc" })
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings")
                    .WithQueryParam("page", 1)
                    .WithQueryParam("pageSize", 20)
                    .WithQueryParam("unitFilter", "unitFilterTest")
                    .WithQueryParam("sort", "unit desc")
                    .WithVerb(HttpMethod.Get)
                    .Times(1);
            }
        }

        [Test]
        public void UpsertUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                var unitCfg = new UnitConfigurationDto() {
                
                    Unit = "unitName",
                    Mappings = new List<GenerationTypeMapping>() {
                        new GenerationTypeMapping(){
                            From = new LocalDate(2019,1,1),
                            To = new LocalDate(2020,1,1),
                            GenerationType = GenerationType.COAL
                        }
                    }
                };

                var req = qs.UpsertUnitConfigurationMappingAsync(unitCfg)
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/{unitCfg.Unit}")
                    .WithVerb(HttpMethod.Put)
                    .Times(1);
            }
        }

        [Test]
        public void DeleteUnitConfigurationMapping()
        {
            using (var httpTest = new HttpTest())
            {
                var qs = new GMEPublicOfferService(_cfg);

                qs.DeleteUnitConfigurationMappingAsync("unitToDelete")
                       .ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/unitconfigurationmappings/unitToDelete")
                    .WithVerb(HttpMethod.Delete)
                    .Times(1);
            }
        }

        #endregion

        #region UpsertGME
        [Test]
        public void UpsertDataAsync()
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
                          new GMEPublicOfferDataDto(){ }
                      }
                };

                var gmeData = new GMEPublicOfferUpsertDataDto()
                {
                    GMEPublicOffer = new List<GMEPublicOfferDto>()
                    {
                        gmePublicOFfer
                    }
                };

                gmeService.UpsertDataAsync(gmeData).ConfigureAwait(true).GetAwaiter().GetResult();

                httpTest.ShouldHaveCalledPath($"{_cfg.BaseAddress}gmepublicoffer/v1.0/upsertdata")
                    .WithVerb(HttpMethod.Post)
                    .Times(1);
            }
        }
        #endregion
    }
}
