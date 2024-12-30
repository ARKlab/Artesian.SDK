// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 

namespace Artesian.SDK.Service
{
    /// <summary>
    /// MarketData service
    /// </summary>
    public partial class MarketDataService : IMarketDataService
    {
        private readonly IArtesianServiceConfig _cfg;
        private readonly ArtesianPolicyConfig _policy;
        private readonly Client _client;

        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public MarketDataService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// Metadata service
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public MarketDataService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants.MetadataVersion, _policy);
        }
    }
}
