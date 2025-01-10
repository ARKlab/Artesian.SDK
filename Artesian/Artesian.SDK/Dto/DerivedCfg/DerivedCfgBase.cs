using Artesian.SDK.Dto.Enums;
using Artesian.SDK.Dto.Serialize;

using MessagePack;

using Newtonsoft.Json;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Base class for Derived Configuration.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(DerivedCfgMuv))]
    [Union(1, typeof(DerivedCfgCoalesce))]
    [Union(2, typeof(DerivedCfgSum))]
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
