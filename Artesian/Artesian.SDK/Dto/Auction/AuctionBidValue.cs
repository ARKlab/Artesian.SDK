using MessagePack;
using System.ComponentModel.DataAnnotations;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Auction Data for a single Multiple timestamps
    /// </summary>
    [MessagePackObject]
    public class AuctionBidValue
    {
        /// <summary>
        /// The Auction Data for a single Multiple timestamps
        /// </summary>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        public AuctionBidValue(double price, double quantity)
        {
            Price = price;
            Quantity = quantity;
        }

        /// <summary>
        /// The Auction Data for a single Multiple timestamps
        /// </summary>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="acceptedPrice"></param>
        /// <param name="acceptedQuantity"></param>
        /// <param name="block"></param>
        public AuctionBidValue(double price, double quantity, double? acceptedPrice, double? acceptedQuantity, BlockType? block = (BlockType?)null)
        {
            Price = price;
            Quantity = quantity;
            AcceptedPrice = acceptedPrice;
            AcceptedQuantity = acceptedQuantity;
            Block = block;
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
        public double? AcceptedPrice { get; set; }

        /// <summary>
        /// Accepted Quantity, Sum of the accepted quantities per offered price level
        /// </summary>
        [Key(3)]
        public double? AcceptedQuantity { get; set; }

        /// <summary>
        /// Block Type
        /// Single: bid/offer consists of a specified volume and price for a specific time of the day
        /// OR 
        /// Block: bid/offer consists of a specified volume and price for a certain number of consecutive times within the same day
        /// </summary>
        [Key(4)]
        public BlockType? Block { get; set; }
    }
}