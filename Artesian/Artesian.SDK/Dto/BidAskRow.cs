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

        #endregion Bid Ask Values
    }
}