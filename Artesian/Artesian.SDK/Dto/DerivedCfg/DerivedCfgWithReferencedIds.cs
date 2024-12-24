﻿using MessagePack;

namespace Artesian.SDK.Dto.DerivedCfg
{
    /// <summary>
    /// Represents a configuration with referenced market data IDs for derived data.
    /// </summary>
    [MessagePackObject]
    public abstract record DerivedCfgWithReferencedIds : DerivedCfgBase
    {
        /// <summary>
        /// The referenced market data IDs: order MAY be relevant depending on the selected algorithm.
        /// </summary>
        [Key("OrderedReferencedMarketDataIds")]
        public int[] OrderedReferencedMarketDataIds { get; set; }
    }
}
