using Artesian.SDK.Dto.Enums;

using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents a derived configuration that uses the Sum algorithm.
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgSum : DerivedCfgWithReferencedIds
    {
        /// <summary>
        /// The Derived Algorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Sum;
    }
}
