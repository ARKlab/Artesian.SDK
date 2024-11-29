using Artesian.SDK.Dto.DerivedCfg.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DerivedCfg
{
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
