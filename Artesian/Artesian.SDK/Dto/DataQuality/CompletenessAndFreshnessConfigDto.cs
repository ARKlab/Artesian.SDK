using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;

using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Abstract configuration for Completeness and Freshness rules.
    /// Validates that expected data records are present within the defined time window and arrive within an acceptable delay.
    /// Concrete subtypes: <see cref="ActualCompletenessAndFreshnessConfigDto"/> for actual (non-versioned) time series,
    /// <see cref="VersionedCompletenessAndFreshnessConfigDto"/> for versioned time series with version tolerance.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(ActualCompletenessAndFreshnessConfigDto))]
    [Union(1, typeof(VersionedCompletenessAndFreshnessConfigDto))]
    [JsonConverter(typeof(CompletenessAndFreshnessConfigConverterSTJ))]
    public abstract class CompletenessAndFreshnessConfigDto : DataQualityRuleConfigDto
    {
        /// <summary>
        /// The type of Market Data this rule applies to (e.g., ActualTimeSerie, VersionedTimeSerie, MarketAssessment).
        /// This determines the structure of the expected data.
        /// </summary>
        [Key("MarketDataType")]
        public MarketDataTypeV2 MarketDataType { get; set; }

        /// <summary>
        /// The schedule configuration defining when quality checks should run and the maximum allowed delay for data arrival.
        /// </summary>
        [Key("ScheduleConfig")]
        public required ScheduleConfigDto ScheduleConfig { get; set; }

        /// <summary>
        /// The record validation configuration defining the expected data range (which time slots should contain records).
        /// </summary>
        [Key("RecordValidationConfig")]
        public required RecordValidationConfigDto RecordValidationConfig { get; set; }

        /// <inheritdoc />
        [IgnoreMember]
        public override RuleType Type => RuleType.CompletenessAndFreshness;
    }
}
