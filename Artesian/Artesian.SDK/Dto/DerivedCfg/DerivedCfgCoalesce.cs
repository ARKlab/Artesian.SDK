using Artesian.SDK.Dto.DerivedCfg.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DerivedCfg
{
    /// <summary>
    /// Represents a derived configuration that uses the Coalesce algorithm.
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgCoalesce : DerivedCfgWithReferencedIds
    {
        /// <summary>
        /// The Derived Algorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Coalesce;
    }
}
