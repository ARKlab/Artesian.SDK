using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;

using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Abstract base class for Data Quality Rule configurations.
    /// The concrete subtype determines the validation behavior:
    /// <see cref="ActualCompletenessAndFreshnessConfigDto"/> for actual time series,
    /// <see cref="VersionedCompletenessAndFreshnessConfigDto"/> for versioned time series,
    /// or <see cref="OutlierConfigDto"/> for outlier detection.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(ActualCompletenessAndFreshnessConfigDto))]
    [Union(1, typeof(VersionedCompletenessAndFreshnessConfigDto))]
    [Union(2, typeof(OutlierConfigDto))]
    [JsonConverter(typeof(DataQualityRuleConfigConverterSTJ))]
    public abstract class DataQualityRuleConfigDto
    {
        /// <summary>
        /// Discriminator indicating the rule type. Determines which configuration properties are relevant.
        /// </summary>
        [IgnoreMember]
        public abstract RuleType Type { get; }
    }
}
