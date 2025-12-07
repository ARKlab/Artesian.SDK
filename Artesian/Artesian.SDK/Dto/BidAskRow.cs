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
    public record BidAskRow
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
        [JsonProperty(PropertyName = "N")]
        [Key(1)]
        public virtual string? CurveName { get; init; }

        /// <summary>
        /// Market Data ID
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        [Key(2)]
        public virtual int TSID { get; init; }

        /// <summary>
        /// Product Name
        /// </summary>
        [JsonProperty(PropertyName = "PR")]
        [Key(3)]
        public virtual string? Product { get; init; }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        [Key(4)]
        public virtual DateTimeOffset Time { get; init; }

        #region Bid Ask Values

        /// <summary>
        /// Best Bid Price
        /// </summary>
        [JsonProperty(PropertyName = "BBP")]
        [Key(5)]
        public double? BestBidPrice { get; init; }

        /// <summary>
        /// Best Ask Price
        /// </summary>
        [JsonProperty(PropertyName = "BAP")]
        [Key(6)]
        public double? BestAskPrice { get; init; }

        /// <summary>
        /// Best Bid Quantity
        /// </summary>
        [JsonProperty(PropertyName = "BBQ")]
        [Key(7)]
        public double? BestBidQuantity { get; init; }

        /// <summary>
        /// Best Ask Quantity
        /// </summary>
        [JsonProperty(PropertyName = "BAQ")]
        [Key(8)]
        public double? BestAskQuantity { get; init; }

        /// <summary>
        /// Last Price
        /// </summary>
        [JsonProperty(PropertyName = "LP")]
        [Key(9)]
        public double? LastPrice { get; init; }

        /// <summary>
        /// Last Quantity
        /// </summary>
        [JsonProperty(PropertyName = "LQ")]
        [Key(10)]
        public double? LastQuantity { get; init; }

        #endregion Bid Ask Values
    }
}