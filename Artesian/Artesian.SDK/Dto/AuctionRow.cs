// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using MessagePack;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AuctionRow entity
    /// </summary>
    [MessagePackObject]
    public record AuctionRow
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
        /// Bid Timestamp
        /// </summary>
        [JsonPropertyName("T")]
        [Key(3)]
        public DateTimeOffset BidTimestamp { get; init; }

        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("S")]
        [Key(4)]
        public AuctionSide Side { get; init; }

        /// <summary>
        /// The Offer Price
        /// </summary>
        [JsonPropertyName("D")]
        [Key(5)]
        public double Price { get; init; }

        /// <summary>
        /// The Offer Quantity
        /// </summary>
        [JsonPropertyName("Q")]
        [Key(6)]
        public double Quantity { get; init; }

        /// <summary>
        /// The Accepted Bid Price
        /// </summary>
        [JsonPropertyName("AD")]
        [Key(7)]
        public double? AcceptedPrice { get; init; }

        /// <summary>
        /// Accepted Quantity, Sum of the accepted quantities per offered price level
        /// </summary>
        [JsonPropertyName("AQ")]
        [Key(8)]
        public double? AcceptedQuantity { get; init; }

        /// <summary>
        /// Block Type the bid's block type:
        /// Single - bid/offer refers to a single BidTimestamp
        /// Block - bid/offer is part of a block, referencing multiple contiguous BidTimestamp
        /// </summary>
        [JsonPropertyName("BT")]
        [Key(9)]
        public BlockType? BlockType { get; init; }

        /// <summary>
        /// Original side before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OS")]
        [Key(10)]
        public AuctionSide? OriginalSide { get; init; }

        /// <summary>
        /// Original price before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OD")]
        [Key(11)]
        public double? OriginalPrice { get; init; }

        /// <summary>
        /// Original quantity before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OQ")]
        [Key(12)]
        public double? OriginalQuantity { get; init; }

        /// <summary>
        /// Original accepted price before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OAD")]
        [Key(13)]
        public double? OriginalAcceptedPrice { get; init; }

        /// <summary>
        /// Original accepted quantity before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OAQ")]
        [Key(14)]
        public double? OriginalAcceptedQuantity { get; init; }

        /// <summary>
        /// Original block type before overrides and fallbacks
        /// </summary>
        [JsonPropertyName("OBT")]
        [Key(15)]
        public BlockType? OriginalBlockType { get; init; }

        /// <summary>
        /// Override side
        /// </summary>
        [JsonPropertyName("XS")]
        [Key(16)]
        public AuctionSide? OverrideSide { get; init; }

        /// <summary>
        /// Override price
        /// </summary>
        [JsonPropertyName("XD")]
        [Key(17)]
        public double? OverridePrice { get; init; }

        /// <summary>
        /// Override quantity
        /// </summary>
        [JsonPropertyName("XQ")]
        [Key(18)]
        public double? OverrideQuantity { get; init; }

        /// <summary>
        /// Override accepted price
        /// </summary>
        [JsonPropertyName("XAD")]
        [Key(19)]
        public double? OverrideAcceptedPrice { get; init; }

        /// <summary>
        /// Override accepted quantity
        /// </summary>
        [JsonPropertyName("XAQ")]
        [Key(20)]
        public double? OverrideAcceptedQuantity { get; init; }

        /// <summary>
        /// Override block type
        /// </summary>
        [JsonPropertyName("XBT")]
        [Key(21)]
        public BlockType? OverrideBlockType { get; init; }

        /// <summary>
        /// Fallback side
        /// </summary>
        [JsonPropertyName("FS")]
        [Key(22)]
        public AuctionSide? FallbackSide { get; init; }

        /// <summary>
        /// Fallback price
        /// </summary>
        [JsonPropertyName("FD")]
        [Key(23)]
        public double? FallbackPrice { get; init; }

        /// <summary>
        /// Fallback quantity
        /// </summary>
        [JsonPropertyName("FQ")]
        [Key(24)]
        public double? FallbackQuantity { get; init; }

        /// <summary>
        /// Fallback accepted price
        /// </summary>
        [JsonPropertyName("FAD")]
        [Key(25)]
        public double? FallbackAcceptedPrice { get; init; }

        /// <summary>
        /// Fallback accepted quantity
        /// </summary>
        [JsonPropertyName("FAQ")]
        [Key(26)]
        public double? FallbackAcceptedQuantity { get; init; }

        /// <summary>
        /// Fallback block type
        /// </summary>
        [JsonPropertyName("FBT")]
        [Key(27)]
        public BlockType? FallbackBlockType { get; init; }

        /// <summary>
        /// Override identifier
        /// </summary>
        [JsonPropertyName("OID")]
        [Key(28)]
        public Guid? OverrideId { get; init; }

        /// <summary>
        /// Fallback identifier
        /// </summary>
        [JsonPropertyName("FID")]
        [Key(29)]
        public Guid? FallbackId { get; init; }

        /// <summary>
        /// Replacement state
        /// </summary>
        [JsonPropertyName("R")]
        [Key(30)]
        public int? Replaced { get; init; }
    }
}