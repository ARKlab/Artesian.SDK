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
        /// The Bid Price
        /// </summary>
        [Required]
        [Key(0)]
        public double Price { get; protected set; }

        /// <summary>
        /// The Bid Quantity
        /// </summary>
        [Required]
        [Key(1)]
        public double Quantity { get; protected set; }
    }
}