// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Strategy to partition service requests
    /// </summary>
    public interface IPartitionStrategy
    {
        /// <summary>
        /// Partition strategy for Actual Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Actual Time Serie Query paramaters to be partitioned. See <see cref="ActualQueryParamaters"/></param>
        /// <returns>
        /// The input list of Actual Time Serie Query paramaters partitioned with the defined strategy. See <see cref="ActualQueryParamaters"/>
        /// </returns>
        IEnumerable<ActualQueryParamaters> Partition(IEnumerable<ActualQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Versioned Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Versioned Time Serie Query paramaters to be partitioned. See <see cref="VersionedQueryParamaters"/></param>
        /// <returns>
        /// The input list of Versioned Time Serie Query paramaters partitioned with the defined strategy. See <see cref="VersionedQueryParamaters"/>
        /// </returns>
        IEnumerable<VersionedQueryParamaters> Partition(IEnumerable<VersionedQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Market Assessment Query
        /// </summary>
        /// <param name="paramaters">The list of Market Assessment Query paramaters to be partitioned. See <see cref="MasQueryParamaters"/></param>
        /// <returns>
        /// The input list of Market Assessment Query paramaters partitioned with the defined strategy. See <see cref="MasQueryParamaters"/>
        /// </returns>
        IEnumerable<MasQueryParamaters> Partition(IEnumerable<MasQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Auction Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Auction Time Serie Query paramaters to be partitioned. See <see cref="AuctionQueryParamaters"/></param>
        /// <returns>
        /// The input list of Auction Time Serie Query paramaters partitioned with the defined strategy. See <see cref="AuctionQueryParamaters"/>
        /// </returns>
        IEnumerable<AuctionQueryParamaters> Partition(IEnumerable<AuctionQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Bid Ask Query
        /// </summary>
        /// <param name="paramaters">The list of Bid Ask Query paramaters to be partitioned. See <see cref="BidAskQueryParamaters"/></param>
        /// <returns>
        /// The input list of Bid Ask Query paramaters partitioned with the defined strategy. See <see cref="BidAskQueryParamaters"/>
        /// </returns>
        IEnumerable<BidAskQueryParamaters> Partition(IEnumerable<BidAskQueryParamaters> paramaters);
        /// <summary>
        /// Partition strategy for Derived Time Serie Query
        /// </summary>
        /// <param name="paramaters">The list of Derived Time Serie Query paramaters to be partitioned. See <see cref="DerivedQueryParamaters"/></param>
        /// <returns>
        /// The input list of Derived Time Serie Query paramaters partitioned with the defined strategy. See <see cref="DerivedQueryParamaters"/>
        /// </returns>
        IEnumerable<DerivedQueryParamaters> Partition(IEnumerable<DerivedQueryParamaters> paramaters);

    }
}
