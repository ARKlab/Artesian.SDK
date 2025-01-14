﻿using Artesian.SDK.Dto.Enums;

using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents the configuration for the MostUpdatedVersion derived algorithm.
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgMuv : DerivedCfgBase
    {
        /// <summary>
        /// The Derived Algorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.MUV;
    }
}
