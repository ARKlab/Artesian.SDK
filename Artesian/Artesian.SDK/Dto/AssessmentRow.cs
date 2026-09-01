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

        /// <summary>
        /// Original settlement before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OS")]
        [Key(13)]
        public double? OriginalSettlement { get; init; }

        /// <summary>
        /// Original open before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OO")]
        [Key(14)]
        public double? OriginalOpen { get; init; }

        /// <summary>
        /// Original close before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OC")]
        [Key(15)]
        public double? OriginalClose { get; init; }

        /// <summary>
        /// Original high before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OH")]
        [Key(16)]
        public double? OriginalHigh { get; init; }

        /// <summary>
        /// Original low before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OL")]
        [Key(17)]
        public double? OriginalLow { get; init; }

        /// <summary>
        /// Original paid volume before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OVP")]
        [Key(18)]
        public double? OriginalVolumePaid { get; init; }

        /// <summary>
        /// Original given volume before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OVG")]
        [Key(19)]
        public double? OriginalVolumeGiven { get; init; }

        /// <summary>
        /// Original total volume before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OVT")]
        [Key(20)]
        public double? OriginalVolumeTotal { get; init; }

        /// <summary>
        /// Override settlement
        /// </summary>
        [JsonPropertyName("XS")]
        [Key(21)]
        public double? OverrideSettlement { get; init; }

        /// <summary>
        /// Override open
        /// </summary>
        [JsonPropertyName("XO")]
        [Key(22)]
        public double? OverrideOpen { get; init; }

        /// <summary>
        /// Override close
        /// </summary>
        [JsonPropertyName("XC")]
        [Key(23)]
        public double? OverrideClose { get; init; }

        /// <summary>
        /// Override high
        /// </summary>
        [JsonPropertyName("XH")]
        [Key(24)]
        public double? OverrideHigh { get; init; }

        /// <summary>
        /// Override low
        /// </summary>
        [JsonPropertyName("XL")]
        [Key(25)]
        public double? OverrideLow { get; init; }

        /// <summary>
        /// Override paid volume
        /// </summary>
        [JsonPropertyName("XVP")]
        [Key(26)]
        public double? OverrideVolumePaid { get; init; }

        /// <summary>
        /// Override given volume
        /// </summary>
        [JsonPropertyName("XVG")]
        [Key(27)]
        public double? OverrideVolumeGiven { get; init; }

        /// <summary>
        /// Override total volume
        /// </summary>
        [JsonPropertyName("XVT")]
        [Key(28)]
        public double? OverrideVolumeTotal { get; init; }

        /// <summary>
        /// Fallback settlement
        /// </summary>
        [JsonPropertyName("FS")]
        [Key(29)]
        public double? FallbackSettlement { get; init; }

        /// <summary>
        /// Fallback open
        /// </summary>
        [JsonPropertyName("FO")]
        [Key(30)]
        public double? FallbackOpen { get; init; }

        /// <summary>
        /// Fallback close
        /// </summary>
        [JsonPropertyName("FC")]
        [Key(31)]
        public double? FallbackClose { get; init; }

        /// <summary>
        /// Fallback high
        /// </summary>
        [JsonPropertyName("FH")]
        [Key(32)]
        public double? FallbackHigh { get; init; }

        /// <summary>
        /// Fallback low
        /// </summary>
        [JsonPropertyName("FL")]
        [Key(33)]
        public double? FallbackLow { get; init; }

        /// <summary>
        /// Fallback paid volume
        /// </summary>
        [JsonPropertyName("FVP")]
        [Key(34)]
        public double? FallbackVolumePaid { get; init; }

        /// <summary>
        /// Fallback given volume
        /// </summary>
        [JsonPropertyName("FVG")]
        [Key(35)]
        public double? FallbackVolumeGiven { get; init; }

        /// <summary>
        /// Fallback total volume
        /// </summary>
        [JsonPropertyName("FVT")]
        [Key(36)]
        public double? FallbackVolumeTotal { get; init; }

        /// <summary>
        /// Override identifier
        /// </summary>
        [JsonPropertyName("OID")]
        [Key(37)]
        public Guid? OverrideId { get; init; }

        /// <summary>
        /// Fallback identifier
        /// </summary>
        [JsonPropertyName("FID")]
        [Key(38)]
        public Guid? FallbackId { get; init; }

        /// <summary>
        /// Replacement state
        /// </summary>
        [JsonPropertyName("R")]
        [Key(39)]
        public int? Replaced { get; init; }

        #endregion Mas Values
    }
}
