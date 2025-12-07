// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Artesian search results
    /// </summary>
    [MessagePackObject]
    public record ArtesianSearchResults
    {
        /// <summary>
        /// Results
        /// </summary>
        [Key(0)]
        public List<MarketDataEntity.Output>? Results { get; init; }
        /// <summary>
        /// Facets
        /// </summary>
        [Key(1)]
        public List<ArtesianMetadataFacet>? Facets { get; init; }
        /// <summary>
        /// Results count
        /// </summary>
        [Key(2)]
        public long? CountResults { get; init; }
    }
}
