using MessagePack;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Auction Bid entity
    /// </summary>
    [MessagePackObject]
    public record AuctionBids
    {
        /// <summary>
        /// The Bid Timestamp
        /// </summary>
        [Required]
        [Key(0)]
        public required LocalDateTime BidTimestamp { get; init; }

        /// <summary>
        /// The BID
        /// </summary>
        [Required]
        [Key(1)]
        public required AuctionBidValue[] Bid { get; init; }

        /// <summary>
        /// The OFFER
        /// </summary>
        [Required]
        [Key(2)]
        public required AuctionBidValue[] Offer { get; init; }
    }
}