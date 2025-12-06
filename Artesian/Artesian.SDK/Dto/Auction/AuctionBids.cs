using MessagePack;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Auction Bid constructor
    /// </summary>
    [MessagePackObject]
    public class AuctionBids
    {
        /// <summary>
        /// The Auction Bid constructor
        /// </summary>
        public AuctionBids(LocalDateTime bidTimestamp, AuctionBidValue[] bid, AuctionBidValue[] offer)
        {
            this.BidTimestamp = bidTimestamp;
            this.Bid = bid;
            this.Offer = offer;
        }

        /// <summary>
        /// The Bid Timestamp
        /// </summary>
        [Required]
        [Key(0)]
        public LocalDateTime BidTimestamp { get; set; }


        /// <summary>
        /// The BID
        /// </summary>
        [Required]
        [Key(1)]
        public AuctionBidValue[] Bid { get; set; } = null!;

        /// <summary>
        /// The OFFER
        /// </summary>
        [Required]
        [Key(2)]
        public AuctionBidValue[] Offer { get; set; } = null!;
    }
}