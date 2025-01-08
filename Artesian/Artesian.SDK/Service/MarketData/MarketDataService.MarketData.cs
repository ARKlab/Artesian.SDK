// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using NodaTime;
using System;
using Flurl;
using System.Globalization;
using Artesian.SDK.Dto.DerivedCfg;
using Artesian.SDK.Dto.MarketData;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Read marketdata metadata by provider and curve name with MarketDataIdentifier
        /// </summary>
        /// <param name="id">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(MarketDataIdentifier id, CancellationToken ctk = default)
        {
            id.Validate();
            var url = "/marketdata/entity"
                    .SetQueryParam("provider", id.Provider)
                    .SetQueryParam("curveName", id.Name)
                    ;
            return _client.Exec<MarketDataEntity.Output>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Read marketdata metadata by id
        /// </summary>
        /// <param name="id">Id of the marketdata to be retrieved</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<MarketDataEntity.Output> ReadMarketDataRegistryAsync(int id, CancellationToken ctk = default)
        {
            if (id < 1)
                throw new ArgumentException("Id invalid: " + id, nameof(id));

            var url = "/marketdata/entity/".AppendPathSegment(id.ToString(CultureInfo.InvariantCulture));
            return _client.Exec<MarketDataEntity.Output>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Read paged set of available versions of the marketdata by id
        /// </summary>
        /// <param name="id">Id of the marketdata to be retrieved</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="product">Market product in the case of Market Assessment</param>
        /// <param name="versionFrom">Start date of version range</param>
        /// <param name="versionTo">End date of version range</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Paged result of CurveRange entity</returns>
        public Task<PagedResult<CurveRange>> ReadCurveRangeAsync(int id, int page, int pageSize, string product = null, LocalDateTime? versionFrom = null, LocalDateTime? versionTo = null, CancellationToken ctk = default)
        {

            var url = "/marketdata/entity/".AppendPathSegment(id.ToString(CultureInfo.InvariantCulture)).AppendPathSegment("curves")
                     .SetQueryParam("versionFrom", versionFrom)
                     .SetQueryParam("versionTo", versionTo)
                     .SetQueryParam("product", product)
                     .SetQueryParam("page", page)
                     .SetQueryParam("pageSize", pageSize)
                     ;

            return _client.Exec<PagedResult<CurveRange>>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Register the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<MarketDataEntity.Output> RegisterMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            metadata.ValidateRegister();

            var url = "/marketdata/entity/";

            return _client.Exec<MarketDataEntity.Output, MarketDataEntity.Input>(HttpMethod.Post, url, metadata, ctk: ctk);
        }
        /// <summary>
        /// Save the given MarketData entity
        /// </summary>
        /// <param name="metadata">MarketDataEntity</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<MarketDataEntity.Output> UpdateMarketDataAsync(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            metadata.ValidateUpdate();

            var url = "/marketdata/entity/".AppendPathSegment(metadata.MarketDataId);

            return _client.Exec<MarketDataEntity.Output, MarketDataEntity.Input>(HttpMethod.Put, url, metadata, ctk: ctk);
        }
        /// <summary>
        /// Delete the specific MarketData entity by id
        /// </summary>
        /// <param name="id">Int</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public Task DeleteMarketDataAsync(int id, CancellationToken ctk = default)
        {
            var url = "/marketdata/entity/".AppendPathSegment(id);

            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }
        /// <summary>
        /// Update Derived Configuration for marketData with id supplied in <paramref name="marketDataId"/> and Rebuild
        /// </summary>
        /// <param name="marketDataId">Id of the marketData</param>
        /// <param name="derivedCfg">The Derived Configuration to be updated</param>
        /// <param name="force">Force the update of configuration also if another rebuild process is running (Defualt=false)</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns>MarketData Entity Output</returns>
        public Task<MarketDataEntity.Output> UpdateDerivedConfigurationAsync(int marketDataId, DerivedCfgBase derivedCfg, bool force = false, CancellationToken ctk = default)
        {
            var marketDataOutput = ReadMarketDataRegistryAsync(marketDataId, ctk).ConfigureAwait(true).GetAwaiter().GetResult();

            marketDataOutput.ValidateDerivedCfg(derivedCfg);

            var url = "/marketdata/entity/".AppendPathSegment(marketDataId.ToString(CultureInfo.InvariantCulture)).AppendPathSegment("updateDerivedConfiguration")
                .SetQueryParam("force", force);

            return _client.Exec<MarketDataEntity.Output, DerivedCfgBase>(HttpMethod.Post, url, derivedCfg, ctk: ctk);
        }
    }
}
