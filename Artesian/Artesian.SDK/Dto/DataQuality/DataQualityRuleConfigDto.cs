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
    /// <see cref="MasCompletenessAndFreshnessConfigDto"/> for Market Assessment,
    /// <see cref="BidAskCompletenessAndFreshnessConfigDto"/> for BidAsk,
    /// <see cref="OutlierConfigDto"/> for actual or versioned outliers,
    /// <see cref="MasOutlierConfigDto"/> for Market Assessment outliers, and
    /// <see cref="BidAskOutlierConfigDto"/> for BidAsk outliers.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(ActualCompletenessAndFreshnessConfigDto))]
    [Union(1, typeof(VersionedCompletenessAndFreshnessConfigDto))]
    [Union(2, typeof(OutlierConfigDto))]
    [Union(3, typeof(MasCompletenessAndFreshnessConfigDto))]
    [Union(4, typeof(BidAskCompletenessAndFreshnessConfigDto))]
    [Union(5, typeof(MasOutlierConfigDto))]
    [Union(6, typeof(BidAskOutlierConfigDto))]
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
