﻿using Artesian.SDK.Dto;
using Artesian.SDK.Dto.PublicOffer;
using Flurl;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.PublicOffer
{
    /// <summary>
    /// PublicOfferService class
    /// Contains query types to be created
    /// </summary>
    public partial class PublicOfferService
    {

        /// <summary>
        /// Get paged OperatorDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="operatorFilter">Operator substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of OperatorDto</returns>
        public Task<PagedResult<OperatorDto>> ReadOperatorsAsync(int page, int pageSize, string operatorFilter = null, string[] sort = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/enums/operators"
                .SetQueryParam("operatorFilter", operatorFilter)
                .SetQueryParam("sort", sort)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page)
                ;

            return _client.Exec<PagedResult<OperatorDto>>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Get paged UnitDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of UnitDto</returns>
        public Task<PagedResult<UnitDto>> ReadUnitsAsync(int page, int pageSize, string unitFilter = null, string[] sort = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/enums/units"
                .SetQueryParam("unitFilter", unitFilter)
                .SetQueryParam("sort", sort)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page)
                ;

            return _client.Exec<PagedResult<UnitDto>>(HttpMethod.Get, url);
        }


        /// <summary>
        /// Get unit configuration mapping by id
        /// </summary>
        /// <param name="unit">Page number</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>UnitConfiguration result</returns>
        public Task<UnitConfigurationDto> ReadUnitConfigurationMappingAsync(string unit, CancellationToken ctk = default)
        {
            if (string.IsNullOrEmpty(unit))
                throw new ArgumentException("Unit must be provided");

            var url = $"/unitconfigurationmappings/{unit}";

            return _client.Exec<UnitConfigurationDto>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Get paged unit configuration mappings
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>UnitConfiguration result</returns>
        public Task<PagedResult<UnitConfigurationDto>> ReadUnitConfigurationMappingsAsync(int page, int pageSize, string unitFilter = null, string[] sort = null, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = $"/unitconfigurationmappings"
                .SetQueryParam("unitFilter", unitFilter)
                .SetQueryParam("sort", sort)
                .SetQueryParam("pageSize", pageSize)
                .SetQueryParam("page", page);

            return _client.Exec<PagedResult<UnitConfigurationDto>>(HttpMethod.Get, url);
        }

        /// <summary>
        /// Upsert unit configuration mapping by id
        /// </summary>
        /// <param name="unitCfg">UnitConfiguration to upsert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Upsert UnitConfiguration</returns>
        public Task<UnitConfigurationDto> UpsertUnitConfigurationMappingAsync(UnitConfigurationDto unitCfg, CancellationToken ctk = default)
        {
            if (unitCfg?.Unit == null)
                throw new ArgumentException($"Valid UnitConfiguration must be provided");

            var url = $"/unitconfigurationmappings/{unitCfg.Unit}";

            return _client.Exec<UnitConfigurationDto, UnitConfigurationDto>(HttpMethod.Put, url, unitCfg);

        }

        /// <summary>
        /// Delete unit configuration mapping by id
        /// </summary>
        /// <param name="unit">Unit to be deleted</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task DeleteUnitConfigurationMappingAsync(string unit, CancellationToken ctk = default)
        {
            if (string.IsNullOrEmpty(unit))
                throw new ArgumentException("Unit must be provided");

            var url = $"/unitconfigurationmappings/{unit}";

            return _client.Exec<UnitConfigurationDto>(HttpMethod.Delete, url);
        }
    }
}
