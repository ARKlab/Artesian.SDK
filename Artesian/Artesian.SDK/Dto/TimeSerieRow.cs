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
        public class Versioned
        {
            /// <summary>
            /// Provider Name
            /// </summary>
            [JsonProperty(PropertyName = "P")]
            [Key(0)]
            public virtual string? ProviderName { get; set; }

            /// <summary>
            /// Curve Name
            /// </summary>
            [JsonProperty(PropertyName = "C")]
            [Key(1)]
            public virtual string? CurveName { get; set; }

            /// <summary>
            /// Time serie Identifier
            /// </summary>
            [JsonProperty(PropertyName = "ID")]
            [Key(2)]
            public virtual int TSID { get; set; }

            /// <summary>
            /// Version
            /// </summary>
            [JsonProperty(PropertyName = "V")]
            [Key(3)]
            public virtual DateTime? Version { get; set; }

            /// <summary>
            /// Time
            /// </summary>
            [JsonProperty(PropertyName = "T")]
            [Key(4)]
            public virtual DateTimeOffset Time { get; set; }

            /// <summary>
            /// Time serie Version
            /// </summary>
            [JsonProperty(PropertyName = "D")]
            [Key(5)]
            public virtual double? Value { get; set; }
         
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonProperty(PropertyName = "S")]
            [Key(6)]
            public virtual DateTimeOffset CompetenceStart { get; set; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonProperty(PropertyName = "E")]
            [Key(7)]
            public virtual DateTimeOffset CompetenceEnd { get; set; }
        }

        /// <summary>
        /// The TimeSerieRow entity Actual
        /// </summary>
        [MessagePackObject]
        public class Actual
        {
            /// <summary>
            /// The Provider display name
            /// </summary>
            [JsonProperty(PropertyName = "P")]
            [Key(0)]
            public virtual string? ProviderName { get; set; }

            /// <summary>
            /// The Curve display name
            /// </summary>
            [JsonProperty(PropertyName = "C")]
            [Key(1)]
            public virtual string? CurveName { get; set; }

            /// <summary>
            /// The Market Data ID
            /// </summary>
            [JsonProperty(PropertyName = "ID")]
            [Key(2)]
            public virtual int TSID { get; set; }

            /// <summary>
            /// The timestamp
            /// </summary>
            [JsonProperty(PropertyName = "T")]
            [Key(3)]
            public virtual DateTimeOffset Time { get; set; }

            /// <summary>
            /// The Value
            /// </summary>
            [JsonProperty(PropertyName = "D")]
            [Key(4)]
            public virtual double? Value { get; set; }
            /// <summary>
            /// Start of first competence
            /// </summary>
            [JsonProperty(PropertyName = "S")]
            [Key(5)]
            public virtual DateTimeOffset CompetenceStart { get; set; }

            /// <summary>
            /// End of last competence
            /// </summary>
            [JsonProperty(PropertyName = "E")]
            [Key(6)]
            public virtual DateTimeOffset CompetenceEnd { get; set; }
        }
    }
}
