// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;

using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Compact extraction results for data quality checks.
    /// Follows the <c>TimeSerieCurveElements</c> convention with abbreviated JSON property names
    /// and separate classes for TS (actual) and VTS (versioned) extracts.
    /// </summary>
    public static class CheckResultExtract
    {
        /// <summary>
        /// Compact extraction result for actual (non-versioned) time series.
        /// </summary>
        [MessagePackObject]
        public class Ts
        {
            /// <summary>
            /// The Provider display name.
            /// </summary>
            [JsonPropertyName("P")]
            [Key(0)]
            public string? ProviderName { get; set; }

            /// <summary>
            /// The Curve display name.
            /// </summary>
            [JsonPropertyName("C")]
            [Key(1)]
            public string? CurveName { get; set; }

            /// <summary>
            /// The Rule display name.
            /// </summary>
            [JsonPropertyName("R")]
            [Key(2)]
            public string? RuleName { get; set; }

            /// <summary>
            /// The Assignment Id.
            /// </summary>
            [JsonPropertyName("AID")]
            [Key(3)]
            public int AssignmentId { get; set; }

            /// <summary>
            /// The Market Data Id.
            /// </summary>
            [JsonPropertyName("MKID")]
            [Key(4)]
            public int MarketDataId { get; set; }

            /// <summary>
            /// The Rule Id.
            /// </summary>
            [JsonPropertyName("RID")]
            [Key(5)]
            public int RuleId { get; set; }

            /// <summary>
            /// The timestamp.
            /// </summary>
            [JsonPropertyName("T")]
            [Key(6)]
            public DateTimeOffset Time { get; set; }

            /// <summary>
            /// Number of issues found in the aggregated period.
            /// </summary>
            [JsonPropertyName("D")]
            [Key(7)]
            public int IssueCount { get; set; }

            /// <summary>
            /// Start of first competence.
            /// </summary>
            [JsonPropertyName("S")]
            [Key(8)]
            public DateTimeOffset CompetenceStart { get; set; }

            /// <summary>
            /// End of last competence.
            /// </summary>
            [JsonPropertyName("E")]
            [Key(9)]
            public DateTimeOffset CompetenceEnd { get; set; }
        }

        /// <summary>
        /// Compact extraction result for versioned time series (VTS).
        /// Adds the <see cref="Version"/> field compared to <see cref="Ts"/>.
        /// </summary>
        [MessagePackObject]
        public class Vts
        {
            /// <summary>
            /// The Provider display name.
            /// </summary>
            [JsonPropertyName("P")]
            [Key(0)]
            public string? ProviderName { get; set; }

            /// <summary>
            /// The Curve display name.
            /// </summary>
            [JsonPropertyName("C")]
            [Key(1)]
            public string? CurveName { get; set; }

            /// <summary>
            /// The Rule display name.
            /// </summary>
            [JsonPropertyName("R")]
            [Key(2)]
            public string? RuleName { get; set; }

            /// <summary>
            /// The Assignment Id.
            /// </summary>
            [JsonPropertyName("AID")]
            [Key(3)]
            public int AssignmentId { get; set; }

            /// <summary>
            /// The Market Data Id.
            /// </summary>
            [JsonPropertyName("MKID")]
            [Key(4)]
            public int MarketDataId { get; set; }

            /// <summary>
            /// The Rule Id.
            /// </summary>
            [JsonPropertyName("RID")]
            [Key(5)]
            public int RuleId { get; set; }

            /// <summary>
            /// The Version.
            /// </summary>
            [JsonPropertyName("V")]
            [Key(6)]
            public DateTime? Version { get; set; }

            /// <summary>
            /// The timestamp.
            /// </summary>
            [JsonPropertyName("T")]
            [Key(7)]
            public DateTimeOffset Time { get; set; }

            /// <summary>
            /// Number of issues found in the aggregated period.
            /// </summary>
            [JsonPropertyName("D")]
            [Key(8)]
            public int IssueCount { get; set; }

            /// <summary>
            /// Start of first competence.
            /// </summary>
            [JsonPropertyName("S")]
            [Key(9)]
            public DateTimeOffset CompetenceStart { get; set; }

            /// <summary>
            /// End of last competence.
            /// </summary>
            [JsonPropertyName("E")]
            [Key(10)]
            public DateTimeOffset CompetenceEnd { get; set; }
        }
    }
}
