using System.Linq;

using Artesian.SDK.Dto.DerivedCfg.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DerivedCfg
{
    [MessagePackObject]
    public record DerivedCfgSum : DerivedCfgWithReferencedIds
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Sum;
    }
}
