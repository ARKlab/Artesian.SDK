using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Derived Curve Configuartion
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgCoalesce : DerivedCfgBase
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Coalesce;
        /// <summary>
        /// Ordered list of market data ids that provide the data for the derived curve
        /// </summary>
        [Key("OrderedReferencedMarketDataIds")]
        public int[] OrderedReferencedMarketDataIds { get; set; }
    }
}
