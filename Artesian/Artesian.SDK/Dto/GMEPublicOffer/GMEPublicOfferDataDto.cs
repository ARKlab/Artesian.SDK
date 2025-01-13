using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferDataDto class
    /// </summary>
    [MessagePackObject]
    public class GMEPublicOfferDataDto
    {

        /// <summary>
        /// The Date
        /// </summary>
        [Key(0)]
        public LocalDate Date { get; set; }

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(1)]
        public int Hour { get; set; }

        /// <summary>
        /// Quarter Value
        /// </summary>
        [Key(2)]
        public int Quarter { get; set; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Key(3)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(4)]
        public decimal AwardedQuantity { get; set; }

        /// <summary>
        /// The Awarded Price
        /// </summary>
        [Key(5)]
        public decimal AwardedPrice { get; set; }

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(6)]
        public decimal EnergyPrice { get; set; }

        /// <summary>
        /// Merit Order
        /// </summary>
        [Key(7)]
        public decimal MeritOrder { get; set; }

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        [Key(8)]
        public bool PartialQuantityAccepted { get; set; }

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        [Key(9)]
        public decimal ADJQuantity { get; set; }

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        [Key(10)]
        public decimal ADJEnergyPrice { get; set; }

        /// <summary>
        /// Transaction Reference
        /// </summary>
        [Key(11)]
        public string TransactionReference { get; set; }

        /// <summary>
        /// Grid Supply Point No
        /// </summary>
        [Key(12)]
        public string GridSupplyPoint { get; set; }

        /// <summary>
        /// Bilateral
        /// </summary>
        [Key(13)]
        public bool? Bilateral { get; set; }

        /// <summary>
        /// SubmittedAt
        /// </summary>
        [Key(14)]
        public LocalDateTime? SubmittedAt { get; set; }

        /// <summary>
        /// Timestamp for Xbid data
        /// </summary>
        [Key(15)]
        public LocalDateTime? Timestamp { get; set; }

        /// <summary>
        /// PrezzoUnitario for Xbid data
        /// </summary>
        [Key(16)]
        public decimal? PrezzoUnitario { get; set; }

        /// <summary>
        /// 15 Minute Period In Day
        /// </summary>
        [Key(17)]
        public int? Period { get; set; }

        /// <summary>
        /// MINIMUM ACCEPTANCE RATIO
        /// </summary>
        [Key(18)]
        public decimal? MinimumAcceptanceRatio { get; set; }
    }
}