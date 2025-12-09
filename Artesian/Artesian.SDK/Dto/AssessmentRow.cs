// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Assessment Row class
    /// </summary>
    [MessagePackObject]
    public record AssessmentRow
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
        [JsonPropertyName("N")]
        [Key(1)]
        public string? CurveName { get; init; }

        /// <summary>
        /// Market Data ID
        /// </summary>
        [JsonPropertyName("ID")]
        [Key(2)]
        public required int TSID { get; init; }

        /// <summary>
        /// Product Name
        /// </summary>
        [JsonPropertyName("PR")]
        [Key(3)]
        public required string Product { get; init; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("T")]
        [Key(4)]
        public required DateTimeOffset Time { get; init; }

        #region Mas Values
        /// <summary>
        /// Settlement
        /// </summary>
        [JsonPropertyName("S")]
        [Key(5)]
        public double? Settlement { get; init; }

        /// <summary>
        /// Open
        /// </summary>
        [JsonPropertyName("O")]
        [Key(6)]
        public double? Open { get; init; }

        /// <summary>
        /// Close
        /// </summary>
        [JsonPropertyName("C")]
        [Key(7)]
        public double? Close { get; init; }

        /// <summary>
        /// High
        /// </summary>
        [JsonPropertyName("H")]
        [Key(8)]
        public double? High { get; init; }

        /// <summary>
        /// Low
        /// </summary>
        [JsonPropertyName("L")]
        [Key(9)]
        public double? Low { get; init; }

        /// <summary>
        /// Volume Paid
        /// </summary>
        [JsonPropertyName("VP")]
        [Key(10)]
        public double? VolumePaid { get; init; }

        /// <summary>
        /// Volume Given
        /// </summary>
        [JsonPropertyName("VG")]
        [Key(11)]
        public double? VolumeGiven { get; init; }

        /// <summary>
        /// Volume Total
        /// </summary>
        [JsonPropertyName("VT")]
        [Key(12)]
        public double? VolumeTotal { get; init; }

        #endregion Mas Values
    }
}
