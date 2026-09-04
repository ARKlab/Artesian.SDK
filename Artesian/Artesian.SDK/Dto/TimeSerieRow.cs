// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The TimeSerieRow entity
    /// </summary>
    public static partial class TimeSerieRow
    {
        /// <summary>
        /// The TimeSerieRow entity Versioned
        /// </summary>
        [MessagePackObject]
        public record Versioned
        {
            /// <summary>
            /// Provider Name
            /// </summary>
            [JsonPropertyName("P")]
            [Key(0)]
            public string? ProviderName { get; init; }

            /// <summary>
            /// Curve Name
            /// </summary>
            [JsonPropertyName("C")]
            [Key(1)]
            public string? CurveName { get; init; }

            /// <summary>
            /// Time serie Identifier
            /// </summary>
            [JsonPropertyName("ID")]
            [Key(2)]
            public int TSID { get; init; }

            /// <summary>
            /// Version
            /// </summary>
            [JsonPropertyName("V")]
            [Key(3)]
            public DateTime? Version { get; init; }

            /// <summary>
            /// Time
            /// </summary>
            [JsonPropertyName("T")]
            [Key(4)]
            public DateTimeOffset Time { get; init; }

            /// <summary>
            /// Time serie Version
            /// </summary>
            [JsonPropertyName("D")]
            [Key(5)]
            public double? Value { get; init; }
         
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonPropertyName("S")]
            [Key(6)]
            public DateTimeOffset CompetenceStart { get; init; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonPropertyName("E")]
            [Key(7)]
            public DateTimeOffset CompetenceEnd { get; init; }

            /// <summary>
            /// The original value before overrides and fallbacks
            /// </summary>
            [JsonPropertyName("OriginalD")]
            [Key(8)]
            public double? OriginalValue { get; init; }

            /// <summary>
            /// The original version before overrides and fallbacks
            /// </summary>
            [JsonPropertyName("OriginalV")]
            [Key(9)]
            public DateTime? OriginalVersion { get; init; }

            /// <summary>
            /// The override value
            /// </summary>
            [JsonPropertyName("OverrideD")]
            [Key(10)]
            public double? OverrideValue { get; init; }

            /// <summary>
            /// The override version
            /// </summary>
            [JsonPropertyName("OverrideV")]
            [Key(11)]
            public DateTime? OverrideVersion { get; init; }

            /// <summary>
            /// The fallback value
            /// </summary>
            [JsonPropertyName("FallbackD")]
            [Key(12)]
            public double? FallbackValue { get; init; }

            /// <summary>
            /// The fallback version
            /// </summary>
            [JsonPropertyName("FallbackV")]
            [Key(13)]
            public DateTime? FallbackVersion { get; init; }

            /// <summary>
            /// The override identifier
            /// </summary>
            [JsonPropertyName("OverrideId")]
            [Key(14)]
            public Guid? OverrideId { get; init; }

            /// <summary>
            /// The fallback identifier
            /// </summary>
            [JsonPropertyName("FallbackId")]
            [Key(15)]
            public Guid? FallbackId { get; init; }

            /// <summary>
            /// The replacement state
            /// </summary>
            [JsonPropertyName("Replaced")]
            [Key(16)]
            public int? Replaced { get; init; }
        }

        /// <summary>
        /// The TimeSerieRow entity Actual
        /// </summary>
        [MessagePackObject]
        public record Actual
        {
            /// <summary>
            /// The Provider display name
            /// </summary>
            [JsonPropertyName("P")]
            [Key(0)]
            public string? ProviderName { get; init; }

            /// <summary>
            /// The Curve display name
            /// </summary>
            [JsonPropertyName("C")]
            [Key(1)]
            public string? CurveName { get; init; }

            /// <summary>
            /// The Market Data ID
            /// </summary>
            [JsonPropertyName("ID")]
            [Key(2)]
            public int TSID { get; init; }

            /// <summary>
            /// The timestamp
            /// </summary>
            [JsonPropertyName("T")]
            [Key(3)]
            public DateTimeOffset Time { get; init; }

            /// <summary>
            /// The Value
            /// </summary>
            [JsonPropertyName("D")]
            [Key(4)]
            public double? Value { get; init; }
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonPropertyName("S")]
            [Key(5)]
            public DateTimeOffset CompetenceStart { get; init; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonPropertyName("E")]
            [Key(6)]
            public DateTimeOffset CompetenceEnd { get; init; }

            /// <summary>
            /// The original value before overrides and fallbacks
            /// </summary>
            [JsonPropertyName("OriginalD")]
            [Key(7)]
            public double? OriginalValue { get; init; }

            /// <summary>
            /// The override value
            /// </summary>
            [JsonPropertyName("OverrideD")]
            [Key(8)]
            public double? OverrideValue { get; init; }

            /// <summary>
            /// The fallback value
            /// </summary>
            [JsonPropertyName("FallbackD")]
            [Key(9)]
            public double? FallbackValue { get; init; }

            /// <summary>
            /// The override identifier
            /// </summary>
            [JsonPropertyName("OverrideId")]
            [Key(10)]
            public Guid? OverrideId { get; init; }

            /// <summary>
            /// The fallback identifier
            /// </summary>
            [JsonPropertyName("FallbackId")]
            [Key(11)]
            public Guid? FallbackId { get; init; }

            /// <summary>
            /// The replacement state
            /// </summary>
            [JsonPropertyName("Replaced")]
            [Key(12)]
            public int? Replaced { get; init; }
        }
    }
}
