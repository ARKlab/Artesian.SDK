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
        }
    }
}
