// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;
using Newtonsoft.Json;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Bid Ask Row class
    /// </summary>
    [MessagePackObject]
    public class BidAskRow
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
        [JsonProperty(PropertyName = "N")]
        [Key(1)]
        public virtual string? CurveName { get; set; }

        /// <summary>
        /// Market Data ID
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        [Key(2)]
        public virtual int TSID { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [JsonProperty(PropertyName = "PR")]
        [Key(3)]
        public virtual string? Product { get; set; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        [Key(4)]
        public virtual DateTimeOffset Time { get; set; }

        #region Bid Ask Values

        /// <summary>
        /// Best Bid Price
        /// </summary>
        [JsonProperty(PropertyName = "BBP")]
        [Key(5)]
        public double? BestBidPrice { get; set; }

        /// <summary>
        /// Best Ask Price
        /// </summary>
        [JsonProperty(PropertyName = "BAP")]
        [Key(6)]
        public double? BestAskPrice { get; set; }

        /// <summary>
        /// Best Bid Quantity
        /// </summary>
        [JsonProperty(PropertyName = "BBQ")]
        [Key(7)]
        public double? BestBidQuantity { get; set; }

        /// <summary>
        /// Best Ask Quantity
        /// </summary>
        [JsonProperty(PropertyName = "BAQ")]
        [Key(8)]
        public double? BestAskQuantity { get; set; }

        /// <summary>
        /// Last Price
        /// </summary>
        [JsonProperty(PropertyName = "LP")]
        [Key(9)]
        public double? LastPrice { get; set; }

        /// <summary>
        /// Last Quantity
        /// </summary>
        [JsonProperty(PropertyName = "LQ")]
        [Key(10)]
        public double? LastQuantity { get; set; }

        #endregion Bid Ask Values
    }
}