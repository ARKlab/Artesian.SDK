// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using MessagePack;
using Newtonsoft.Json;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AuctionRow entity
    /// </summary>
    [MessagePackObject]
    public class AuctionRow
    {
        /// <summary>
        /// Provider Name
        /// </summary>
        [JsonProperty(PropertyName = "P")]
        [Key(0)]
        public virtual string ProviderName { get; set; }

        /// <summary>
        /// Curve Name
        /// </summary>
        [JsonProperty(PropertyName = "N")]
        [Key(1)]
        public virtual string CurveName { get; set; }

        /// <summary>
        /// Market Data ID
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        [Key(2)]
        public virtual int TSID { get; set; }

        /// <summary>
        /// Bid Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        [Key(3)]
        public virtual DateTimeOffset BidTimestamp { get; set; }

        /// <summary>
        /// Side
        /// </summary>
        [JsonProperty(PropertyName = "S")]
        [Key(4)]
        public virtual AuctionSide Side { get; set; }

        /// <summary>
        /// The Offer Price
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        [Key(5)]
        public virtual double Price { get; set; }

        /// <summary>
        /// The Offer Quantity
        /// </summary>
        [JsonProperty(PropertyName = "Q")]
        [Key(6)]
        public virtual double Quantity { get; set; }

        /// <summary>
        /// The Accepted Bid Price
        /// </summary>
        [JsonProperty(PropertyName = "AD")]
        [Key(7)]
        public double? AcceptedPrice { get; set; }

        /// <summary>
        /// Accepted Quantity, Sum of the accepted quantities per offered price level
        /// </summary>
        [JsonProperty(PropertyName = "AQ")]
        [Key(8)]
        public double? AcceptedQuantity { get; set; }

        /// <summary>
        /// Block Type the bid's block type:
        /// Single - bid/offer refers to a single BidTimestamp
        /// Block - bid/offer is part of a block, referencing multiple contiguous BidTimestamp
        /// </summary>
        [JsonProperty(PropertyName = "BT")]
        [Key(9)]
        public BlockType? BlockType { get; set; }
    }
}