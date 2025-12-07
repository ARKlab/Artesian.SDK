// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Flurl;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// QueryService class
    /// Contains query types to be created
    /// </summary>
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class QueryService : IQueryService
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private readonly IArtesianServiceConfig _cfg;
        private readonly ArtesianPolicyConfig _policy;
        private readonly Client _client;
        private readonly IPartitionStrategy _partitionStrategy = new PartitionByIDStrategy();

        /// <summary>
        /// Query service for building a query
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        public QueryService(IArtesianServiceConfig cfg)
            : this(cfg, new ArtesianPolicyConfig())
        {
        }

        /// <summary>
        /// Query service for building a query
        /// </summary>
        /// <param name="cfg">IArtesianServiceConfig</param>
        /// <param name="policy">ArtesianPolicyConfig</param>
        public QueryService(IArtesianServiceConfig cfg, ArtesianPolicyConfig policy)
        {
            _cfg = cfg;
            _policy = policy;
            _client = new Client(cfg, ArtesianConstants._queryRoute.AppendPathSegment(ArtesianConstants._queryVersion), _policy);
        }

        /// <summary>
        /// Create Actual Time Serie Query
        /// </summary>
        /// <returns>
        /// Actual Time Serie <see cref="ActualQuery"/>
        /// </returns>
        public ActualQuery CreateActual()
        {
            return new ActualQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create  Actual Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy">Partition Strategy</param>
        /// <returns>
        /// Actual Time Serie <see cref="ActualQuery"/>
        /// </returns>
        public ActualQuery CreateActual(IPartitionStrategy partitionStrategy)
        {
            return new ActualQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Versioned Time Serie Query
        /// </summary>
        /// <returns>
        /// Versioned Time Serie <see cref="VersionedQuery"/>
        /// </returns>
        public VersionedQuery CreateVersioned()
        {
            return new VersionedQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Versioned Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy">Partition Strategy</param>
        /// <returns>
        /// Versioned Time Serie <see cref="VersionedQuery"/>
        /// </returns>
        public VersionedQuery CreateVersioned(IPartitionStrategy partitionStrategy)
        {
            return new VersionedQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Market Assessment Time Serie Query
        /// </summary>
        /// <returns>
        /// Market Assessment Time Serie <see cref="MasQuery"/>
        /// </returns>
        public MasQuery CreateMarketAssessment()
        {
            return new MasQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Market Assessment Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy">Partition Strategy</param>
        /// <returns>
        /// Market Assessment Time Serie <see cref="MasQuery"/>
        /// </returns>
        public MasQuery CreateMarketAssessment(IPartitionStrategy partitionStrategy)
        {
            return new MasQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Auction Time Serie Query
        /// </summary>
        /// <returns>
        /// Auction Time Serie <see cref="AuctionQuery"/>
        /// </returns>
        public AuctionQuery CreateAuction()
        {
            return new AuctionQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Auction Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy">Partition Strategy</param>
        /// <returns>
        /// Auction Time Serie <see cref="AuctionQuery"/>
        /// </returns>
        public AuctionQuery CreateAuction(IPartitionStrategy partitionStrategy)
        {
            return new AuctionQuery(_client, partitionStrategy ?? _partitionStrategy);
        }

        /// <summary>
        /// Create Bid Ask Time Serie Query
        /// </summary>
        /// <returns>
        /// Bid Ask Time Serie <see cref="MasQuery"/>
        /// </returns>
        public BidAskQuery CreateBidAsk()
        {
            return new BidAskQuery(_client, _partitionStrategy);
        }

        /// <summary>
        /// Create Bid Ask Time Serie Query
        /// </summary>
        /// <param name="partitionStrategy">Partition Strategy</param>
        /// <returns>
        /// Bid Ask Time Serie <see cref="MasQuery"/>
        /// </returns>
        public BidAskQuery CreateBidAsk(IPartitionStrategy partitionStrategy)
        {
            return new BidAskQuery(_client, partitionStrategy ?? _partitionStrategy);
        }
    }
}