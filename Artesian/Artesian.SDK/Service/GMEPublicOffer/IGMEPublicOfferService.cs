using Artesian.SDK.Dto;
using Artesian.SDK.Dto.GMEPublicOffer;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GME Public Offer Service Interface
    /// </summary>
    public interface IGMEPublicOfferService
    {
        /// <summary>
        /// Create GME Public Offer Query
        /// </summary>
        /// <returns></returns>
        GMEPublicOfferQuery CreateRawCurveQuery();

        /// <summary>
        /// Get paged OperatorDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="operatorFilter">Operator substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of OperatorDto</returns>
        Task<PagedResult<OperatorDto>> ReadOperatorsAsync(int page, int pageSize, string? operatorFilter = null, string[]? sort = null, CancellationToken ctk = default);

        /// <summary>
        /// Get paged UnitDto 
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of UnitDto</returns>
        Task<PagedResult<UnitDto>> ReadUnitsAsync(int page, int pageSize, string? unitFilter = null, string[]? sort = null, CancellationToken ctk = default);



        /// <summary>
        /// Get unit configuration mapping by id
        /// </summary>
        /// <param name="unit">Unit to retrieve by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>UnitConfiguration result</returns>
        Task<UnitConfigurationDto> ReadUnitConfigurationMappingAsync(string unit, CancellationToken ctk = default);

        /// <summary>
        /// Get paged unit configuration mappings
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="unitFilter">Unit substring filter</param>
        /// <param name="sort">Sort by</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>UnitConfiguration result</returns>
        Task<PagedResult<UnitConfigurationDto>> ReadUnitConfigurationMappingsAsync(int page, int pageSize, string? unitFilter = null, string[]? sort = null, CancellationToken ctk = default);

        /// <summary>
        /// Upsert unit configuration mapping by id
        /// </summary>
        /// <param name="unit">Unit to upsert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>UnitConfiguration result</returns>
        Task<UnitConfigurationDto> UpsertUnitConfigurationMappingAsync(UnitConfigurationDto unit, CancellationToken ctk = default);

        /// <summary>
        /// Delete unit configuration mapping by id
        /// </summary>
        /// <param name="unit">Unit to be deleted</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        Task DeleteUnitConfigurationMappingAsync(string unit, CancellationToken ctk = default);
    }
}
