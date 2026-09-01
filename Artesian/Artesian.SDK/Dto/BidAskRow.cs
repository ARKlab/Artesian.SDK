// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Bid Ask Row class
    /// </summary>
    [MessagePackObject]
    public record BidAskRow
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
        public int TSID { get; init; }

        /// <summary>
        /// Product Name
        /// </summary>
        [JsonPropertyName("PR")]
        [Key(3)]
        public string? Product { get; init; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("T")]
        [Key(4)]
        public DateTimeOffset Time { get; init; }

        #region Bid Ask Values

        /// <summary>
        /// Best Bid Price
        /// </summary>
        [JsonPropertyName("BBP")]
        [Key(5)]
        public double? BestBidPrice { get; init; }

        /// <summary>
        /// Best Ask Price
        /// </summary>
        [JsonPropertyName("BAP")]
        [Key(6)]
        public double? BestAskPrice { get; init; }

        /// <summary>
        /// Best Bid Quantity
        /// </summary>
        [JsonPropertyName("BBQ")]
        [Key(7)]
        public double? BestBidQuantity { get; init; }

        /// <summary>
        /// Best Ask Quantity
        /// </summary>
        [JsonPropertyName("BAQ")]
        [Key(8)]
        public double? BestAskQuantity { get; init; }

        /// <summary>
        /// Last Price
        /// </summary>
        [JsonPropertyName("LP")]
        [Key(9)]
        public double? LastPrice { get; init; }

        /// <summary>
        /// Last Quantity
        /// </summary>
        [JsonPropertyName("LQ")]
        [Key(10)]
        public double? LastQuantity { get; init; }

        /// <summary>
        /// Original best bid price before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OBBP")]
        [Key(11)]
        public double? OriginalBestBidPrice { get; init; }

        /// <summary>
        /// Original best ask price before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OBAP")]
        [Key(12)]
        public double? OriginalBestAskPrice { get; init; }

        /// <summary>
        /// Original best bid quantity before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OBBQ")]
        [Key(13)]
        public double? OriginalBestBidQuantity { get; init; }

        /// <summary>
        /// Original best ask quantity before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OBAQ")]
        [Key(14)]
        public double? OriginalBestAskQuantity { get; init; }

        /// <summary>
        /// Original last price before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OLP")]
        [Key(15)]
        public double? OriginalLastPrice { get; init; }

        /// <summary>
        /// Original last quantity before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OLQ")]
        [Key(16)]
        public double? OriginalLastQuantity { get; init; }

        /// <summary>
        /// Override best bid price
        /// </summary>
        [JsonPropertyName("XBBP")]
        [Key(17)]
        public double? OverrideBestBidPrice { get; init; }

        /// <summary>
        /// Override best ask price
        /// </summary>
        [JsonPropertyName("XBAP")]
        [Key(18)]
        public double? OverrideBestAskPrice { get; init; }

        /// <summary>
        /// Override best bid quantity
        /// </summary>
        [JsonPropertyName("XBBQ")]
        [Key(19)]
        public double? OverrideBestBidQuantity { get; init; }

        /// <summary>
        /// Override best ask quantity
        /// </summary>
        [JsonPropertyName("XBAQ")]
        [Key(20)]
        public double? OverrideBestAskQuantity { get; init; }

        /// <summary>
        /// Override last price
        /// </summary>
        [JsonPropertyName("XLP")]
        [Key(21)]
        public double? OverrideLastPrice { get; init; }

        /// <summary>
        /// Override last quantity
        /// </summary>
        [JsonPropertyName("XLQ")]
        [Key(22)]
        public double? OverrideLastQuantity { get; init; }

        /// <summary>
        /// Fallback best bid price
        /// </summary>
        [JsonPropertyName("FBBP")]
        [Key(23)]
        public double? FallbackBestBidPrice { get; init; }

        /// <summary>
        /// Fallback best ask price
        /// </summary>
        [JsonPropertyName("FBAP")]
        [Key(24)]
        public double? FallbackBestAskPrice { get; init; }

        /// <summary>
        /// Fallback best bid quantity
        /// </summary>
        [JsonPropertyName("FBBQ")]
        [Key(25)]
        public double? FallbackBestBidQuantity { get; init; }

        /// <summary>
        /// Fallback best ask quantity
        /// </summary>
        [JsonPropertyName("FBAQ")]
        [Key(26)]
        public double? FallbackBestAskQuantity { get; init; }

        /// <summary>
        /// Fallback last price
        /// </summary>
        [JsonPropertyName("FLP")]
        [Key(27)]
        public double? FallbackLastPrice { get; init; }

        /// <summary>
        /// Fallback last quantity
        /// </summary>
        [JsonPropertyName("FLQ")]
        [Key(28)]
        public double? FallbackLastQuantity { get; init; }

        /// <summary>
        /// Override identifier
        /// </summary>
        [JsonPropertyName("OID")]
        [Key(29)]
        public Guid? OverrideId { get; init; }

        /// <summary>
        /// Fallback identifier
        /// </summary>
        [JsonPropertyName("FID")]
        [Key(30)]
        public Guid? FallbackId { get; init; }

        /// <summary>
        /// Replacement state
        /// </summary>
        [JsonPropertyName("R")]
        [Key(31)]
        public int? Replaced { get; init; }

        #endregion Bid Ask Values
    }
}