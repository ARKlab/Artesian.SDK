// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using Newtonsoft.Json;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Derived Curve Configuartion
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(DerivedCfgMuv))]
    [Union(1, typeof(DerivedCfgCoalesce))]
    [JsonConverter(typeof(DerivedCfgBaseConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(DerivedCfgBaseConverterSTJ))]
    public abstract record DerivedCfgBase
    {
        /// <summary>
        /// The DerivedCfg Version
        /// </summary>
        [Key("Version")]
        public int Version { get; set; }

        /// <summary>
        /// The DerivedAlgorithm
        /// </summary>
        [IgnoreMember]
        public abstract DerivedAlgorithm DerivedAlgorithm { get; }
    }
}