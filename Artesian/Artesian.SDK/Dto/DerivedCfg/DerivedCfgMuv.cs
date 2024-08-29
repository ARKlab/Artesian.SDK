// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Derived Curve Configuartion
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgMuv : DerivedCfgBase
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.MUV;
    }
}