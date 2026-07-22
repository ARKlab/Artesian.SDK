using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using System;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Represents a reusable Data Quality Rule that defines validation logic for market data.
    /// Rules are categorized by <see cref="RuleType"/> and contain a polymorphic configuration
    /// that specifies the check parameters (e.g., completeness schedules, outlier thresholds).
    /// Uses the Input/Output pattern: <see cref="Input"/> for write operations, <see cref="Output"/> for read operations with enriched data.
    /// </summary>
    public static class DataQualityRuleDto
    {
        /// <summary>
        /// Write model for creating or updating a Data Quality Rule.
        /// Contains the rule definition fields and concurrency token.
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The unique identifier of the rule, assigned by the server on creation.
            /// </summary>
            [Key(0)]
            public int Id { get; set; }

            /// <summary>
            /// A human-readable name describing the rule's purpose (e.g., "Weather station hourly completeness").
            /// </summary>
            [Key(1)]
            public required string Name { get; set; }

            /// <summary>
            /// The type of the rule, which determines the expected configuration subtype.
            /// <see cref="RuleType.CompletenessAndFreshness"/> requires a completeness config;
            /// <see cref="RuleType.Outlier"/> requires an outlier config.
            /// </summary>
            [Key(2)]
            public RuleType Type { get; set; }

            /// <summary>
            /// The polymorphic configuration for this rule. The concrete type depends on <see cref="Type"/>:
            /// <see cref="CompletenessAndFreshnessConfigDto"/> (or its subtypes) for completeness rules,
            /// <see cref="OutlierConfigDto"/> for outlier detection rules.
            /// </summary>
            [Key(3)]
            public required DataQualityRuleConfigDto Configuration { get; set; }

            /// <summary>
            /// The monotonically increasing version number, incremented on each update for optimistic concurrency.
            /// </summary>
            [Key(4)]
            public int Version { get; set; }

            /// <summary>
            /// The entity tag for optimistic concurrency control.
            /// </summary>
            [Key(5)]
            public string? ETag { get; set; }
        }

        /// <summary>
        /// Read model returned by GET operations. Extends <see cref="Input"/> with the server-assigned identifier and aggregated check status.
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
            /// <summary>
            /// The latest aggregated check status across all assignments of this rule. Null when no check has been executed yet.
            /// </summary>
            [Key(6)]
            public CheckAggregatedStatus? AggregatedStatus { get; set; }
        }
    }

    internal static class DataQualityRuleExt
    {
        public static void Validate(this DataQualityRuleDto.Input dataQualityRule)
        {
            if (string.IsNullOrWhiteSpace(dataQualityRule.Name))
                throw new ArgumentException("DataQualityRule name must be provided.", nameof(dataQualityRule.Name));

            if (dataQualityRule.Configuration == null)
                throw new ArgumentException("DataQualityRule configuration must be provided.", nameof(dataQualityRule.Configuration));

            if (dataQualityRule.Configuration.Type != dataQualityRule.Type)
                throw new ArgumentException("DataQualityRule configuration type must match the rule type.", nameof(dataQualityRule.Type));

        }
    }
}