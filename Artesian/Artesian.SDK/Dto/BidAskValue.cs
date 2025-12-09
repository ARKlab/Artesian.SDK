// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Bid Ask Value class
    /// </summary>
    [MessagePackObject]
    public record BidAskValue
    {
        /// <summary>
        /// Bid Ask Value Construtor by parameters
        /// </summary>
        public BidAskValue(
            double? bestBidPrice = null,
            double? bestAskPrice = null,
            double? bestBidQuantity = null,
            double? bestAskQuantity = null,
            double? lastPrice = null,
            double? lastQuantity = null)
        {
            BestBidPrice = bestBidPrice;
            BestAskPrice = bestAskPrice;
            BestBidQuantity = bestBidQuantity;
            BestAskQuantity = bestAskQuantity;
            LastPrice = lastPrice;
            LastQuantity = lastQuantity;
        }
        /// <summary>
        /// The Bid Ask Best Bid Price
        /// </summary>
        [Key(0)]
        public double? BestBidPrice { get; init; }
        /// <summary>
        /// The Bid Ask Best Ask Price
        /// </summary>
        [Key(1)]
        public double? BestAskPrice { get; init; }
        /// <summary>
        /// The Bid Ask Best Bid Quantity
        /// </summary>
        [Key(2)]
        public double? BestBidQuantity { get; init; }
        /// <summary>
        /// The Bid Ask Best Ask Quantity
        /// </summary>
        [Key(3)]
        public double? BestAskQuantity { get; init; }
        /// <summary>
        /// The Bid Ask Last Price
        /// </summary>
        [Key(4)]
        public double? LastPrice { get; init; }
        /// <summary>
        /// The Bid Ask Last Quantity
        /// </summary>
        [Key(5)]
        public double? LastQuantity { get; init; }
    }
}
