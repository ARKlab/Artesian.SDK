using Artesian.SDK.Dto.Enums;

using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents a derived configuration that uses the Sum algorithm.
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgTransform : DerivedCfgWithReferencedIds
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Transform;

        /// <summary>
        /// The Transform query to apply on the referenced timeseries.
        /// </summary>
        [Key("Transform")]
        public string? Transform { get; set; }
    }
}
