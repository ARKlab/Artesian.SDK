using MessagePack;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Auction Data for a single Multiple timestamps
    /// </summary>
    [MessagePackObject]
    public record AuctionBidValue
    {
        /// <summary>
        /// The Auction Data for a single Multiple timestamps
        /// </summary>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="acceptedPrice"></param>
        /// <param name="acceptedQuantity"></param>
        /// <param name="blockType"></param>
        public AuctionBidValue(double price, double quantity, double? acceptedPrice = null, double? acceptedQuantity = null, BlockType? blockType = null)
        {
            Price = price;
            Quantity = quantity;
            AcceptedPrice = acceptedPrice;
            AcceptedQuantity = acceptedQuantity;
            BlockType = blockType;
        }

        /// <summary>
        /// The Offered Bid Price
        /// </summary>
        [Required]
        [Key(0)]
        public double Price { get; protected set; }

        /// <summary>
        /// Quantity, sum of the offered quantities per offered price level
        /// </summary>
        [Required]
        [Key(1)]
        public double Quantity { get; protected set; }

        /// <summary>
        /// The Accepted Bid Price
        /// </summary>
        [Key(2)]
        public double? AcceptedPrice { get; init; }

        /// <summary>
        /// Accepted Quantity, Sum of the accepted quantities per offered price level
        /// </summary>
        [Key(3)]
        public double? AcceptedQuantity { get; init; }

        /// <summary>
        /// Block Type the bid's block type:
        /// Single - bid/offer refers to a single BidTimestamp
        /// Block - bid/offer is part of a block, referencing multiple contiguous BidTimestamp
        /// </summary>
        [Key(4)]
        public BlockType? BlockType { get; init; }
    }
}