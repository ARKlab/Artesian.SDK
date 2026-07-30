using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Outlier detection model using fixed absolute bounds.
    /// A data point is flagged as an outlier if its value falls below <see cref="LowerBound"/> or above <see cref="UpperBound"/>.
    /// </summary>
    [MessagePackObject]
    public class OutlierAbsoluteBoundConfigDto : OutlierModelConfigDto
    {
        /// <summary>
        /// The maximum acceptable value. Data points with values above this threshold are flagged as outliers.
        /// </summary>
        [Key("UpperBound")]
        public double UpperBound { get; set; }

        /// <summary>
        /// The minimum acceptable value. Data points with values below this threshold are flagged as outliers.
        /// </summary>
        [Key("LowerBound")]
        public double LowerBound { get; set; }

        /// <inheritdoc />
        [IgnoreMember]
        public override OutlierModel Model => OutlierModel.AbsoluteBound;
    }
}
