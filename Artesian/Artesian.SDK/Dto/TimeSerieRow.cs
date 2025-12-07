// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using Newtonsoft.Json;
using System;

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
            [JsonProperty(PropertyName = "P")]
            [Key(0)]
            public virtual string? ProviderName { get; init; }

            /// <summary>
            /// Curve Name
            /// </summary>
            [JsonProperty(PropertyName = "C")]
            [Key(1)]
            public virtual string? CurveName { get; init; }

            /// <summary>
            /// Time serie Identifier
            /// </summary>
            [JsonProperty(PropertyName = "ID")]
            [Key(2)]
            public virtual int TSID { get; init; }

            /// <summary>
            /// Version
            /// </summary>
            [JsonProperty(PropertyName = "V")]
            [Key(3)]
            public virtual DateTime? Version { get; init; }

            /// <summary>
            /// Time
            /// </summary>
            [JsonProperty(PropertyName = "T")]
            [Key(4)]
            public virtual DateTimeOffset Time { get; init; }

            /// <summary>
            /// Time serie Version
            /// </summary>
            [JsonProperty(PropertyName = "D")]
            [Key(5)]
            public virtual double? Value { get; init; }
         
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonProperty(PropertyName = "S")]
            [Key(6)]
            public virtual DateTimeOffset CompetenceStart { get; init; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonProperty(PropertyName = "E")]
            [Key(7)]
            public virtual DateTimeOffset CompetenceEnd { get; init; }
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
            [JsonProperty(PropertyName = "P")]
            [Key(0)]
            public virtual string? ProviderName { get; init; }

            /// <summary>
            /// The Curve display name
            /// </summary>
            [JsonProperty(PropertyName = "C")]
            [Key(1)]
            public virtual string? CurveName { get; init; }

            /// <summary>
            /// The Market Data ID
            /// </summary>
            [JsonProperty(PropertyName = "ID")]
            [Key(2)]
            public virtual int TSID { get; init; }

            /// <summary>
            /// The timestamp
            /// </summary>
            [JsonProperty(PropertyName = "T")]
            [Key(3)]
            public virtual DateTimeOffset Time { get; init; }

            /// <summary>
            /// The Value
            /// </summary>
            [JsonProperty(PropertyName = "D")]
            [Key(4)]
            public virtual double? Value { get; init; }
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonProperty(PropertyName = "S")]
            [Key(5)]
            public virtual DateTimeOffset CompetenceStart { get; init; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonProperty(PropertyName = "E")]
            [Key(6)]
            public virtual DateTimeOffset CompetenceEnd { get; init; }
        }
    }
}
