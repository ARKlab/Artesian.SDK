﻿using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents a configuration with referenced market data IDs for derived data.
    /// </summary>
    // [MessagePackObject] // this is not a serializable object type: only an helper for common properties
    public abstract record DerivedCfgWithReferencedIds : DerivedCfgBase
    {
        /// <summary>
        /// The referenced market data IDs: order MAY be relevant depending on the selected algorithm.
        /// </summary>
        [Key("OrderedReferencedMarketDataIds")]
        public int[] OrderedReferencedMarketDataIds { get; set; }
    }
}
