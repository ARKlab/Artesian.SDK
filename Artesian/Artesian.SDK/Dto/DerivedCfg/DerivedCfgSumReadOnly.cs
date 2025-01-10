﻿using Artesian.SDK.Dto.Enums;

using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// DerivedSum Configuration Readonly
    /// </summary>
    public record DerivedCfgSumReadOnly : DerivedCfgSum
    {
        /// <summary>
        /// DerivedCoalesce constructor
        /// </summary>
        /// <param name="cfg"></param>
        public DerivedCfgSumReadOnly(DerivedCfgSum cfg)
        {
            // Copy properties from the original cfg to the read-only version
            this.Version = cfg.Version;
            this.DerivedAlgorithm = cfg.DerivedAlgorithm;
            this.OrderedReferencedMarketDataIds = new List<int>(cfg.OrderedReferencedMarketDataIds).AsReadOnly();
            // Copy other properties as needed
        }

        /// <summary>
        /// MarketData ReferenceIds list
        /// </summary>
        public new IReadOnlyList<int> OrderedReferencedMarketDataIds { get; }
        /// <summary>
        /// DerivedAlgorithm
        /// </summary>
        public new DerivedAlgorithm DerivedAlgorithm { get; }
        /// <summary>
        /// Version
        /// </summary>
        public new int Version { get; }

    }
}
