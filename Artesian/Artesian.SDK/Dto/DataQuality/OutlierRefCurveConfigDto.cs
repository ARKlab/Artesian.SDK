using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Outlier detection model that compares data against a reference Market Data curve.
    /// A data point is flagged as an outlier if it deviates from the reference value by more than the configured tolerance percentage.
    /// </summary>
    [MessagePackObject]
    public class OutlierRefCurveConfigDto : OutlierModelConfigDto
    {
        /// <summary>
        /// The identifier of the reference Market Data entity whose values serve as the baseline for comparison.
        /// </summary>
        [Key("ReferenceMarketDataId")]
        public int ReferenceMarketDataId { get; set; }

        /// <summary>
        /// The maximum allowed percentage deviation from the reference curve value (e.g., 0.1 = 10%).
        /// Data points deviating more than this percentage are flagged as outliers.
        /// </summary>
        [Key("TolerancePerc")]
        public double TolerancePerc { get; set; }

        /// <inheritdoc />
        [IgnoreMember]
        public override OutlierModel Model => OutlierModel.RefCurve;
    }
}
