using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Configuration for Outlier detection rules.
    /// Contains a polymorphic <see cref="Model"/> that defines the specific statistical model and its parameters
    /// used to identify anomalous data points.
    /// </summary>
    [MessagePackObject]
    public class OutlierConfigDto : DataQualityRuleConfigDto
    {
        /// <summary>
        /// The outlier detection model configuration. The concrete subtype determines the statistical approach used
        /// (e.g., absolute bounds, moving average, reference curve, AI-based).
        /// </summary>
        [Key("Model")]
        public required OutlierModelConfigDto Model { get; set; }

        /// <inheritdoc />
        [IgnoreMember]
        public override RuleType Type => RuleType.Outlier;
    }
}
