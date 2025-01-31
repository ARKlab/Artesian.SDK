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
        public AuctionBidValue(double price, double quantity, double? acceptedPrice, double? acceptedQuantity, OfferType? block = (OfferType?)null)
        {
            Price = price;
            Quantity = quantity;
            AcceptedPrice = acceptedPrice;
            AcceptedQuantity = acceptedQuantity;
            Block = block;
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

        /// <summary>
        /// The Accepted Bid Price
        /// </summary>
        [Key(2)]
        public double? AcceptedPrice { get; set; }

        /// <summary>
        /// The Accepted Bid Quantity
        /// </summary>
        [Key(3)]
        public double? AcceptedQuantity { get; set; }

        /// <summary>
        /// Offer Type
        /// Single OR Block
        /// </summary>
        [Key(4)]
        public OfferType? Block { get; set; }
    }
}