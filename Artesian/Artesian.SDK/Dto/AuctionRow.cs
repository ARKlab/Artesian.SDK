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
    }
}