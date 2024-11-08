using Artesian.SDK.Dto.DerivedCfg.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DerivedCfg
{
    [MessagePackObject]
    public record DerivedCfgCoalesce : DerivedCfgBase
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Coalesce;

        [Key("OrderedReferencedMarketDataIds")]
        public int[] OrderedReferencedMarketDataIds { get; set; }
    }
}
