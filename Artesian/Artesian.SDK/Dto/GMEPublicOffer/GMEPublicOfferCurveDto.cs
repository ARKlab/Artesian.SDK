using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferCurve class
    /// </summary>
    [MessagePackObject]
    public class GMEPublicOfferCurveDto
    {
        /// <summary>
        /// The Status
        /// </summary>
        [Key(0)]
        public GME_Status Status { get; set; }

        /// <summary>
        /// The BAType
        /// </summary>
        [Key(1)]
        public GME_BAType BAType { get; set; }

        /// <summary>
        /// The Scope
        /// </summary>
        [Key(2)]
        public GME_Scope Scope { get; set; }

        /// <summary>
        /// The Date
        /// </summary>
        [Key(3)]
        public LocalDate Date { get; set; }

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(4)]
        public int Hour { get; set; }

        /// <summary>
        /// The Market
        /// </summary>
        [Key(5)]
        public GME_Market Market { get; set; }

        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(6)]
        public GME_Purpose Purpose { get; set; }

        /// <summary>
        /// The Zone
        /// </summary>
        [Key(7)]
        public GME_Zone Zone { get; set; }

        /// <summary>
        /// The Unit Type
        /// </summary>
        [Key(8)]
        public GME_UnitType UnitType { get; set; }

        /// <summary>
        /// The Generation Type
        /// </summary>
        [Key(9)]
        public GME_GenerationType GenerationType { get; set; }

        /// <summary>
        /// The Unit
        /// </summary>
        [Key(10)]
        public string Unit { get; set; }

        /// <summary>
        /// The Operator
        /// </summary>
        [Key(11)]
        public string Operator { get; set; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Key(12)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(13)]
        public decimal AwardedQuantity { get; set; }

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(14)]
        public decimal EnergyPrice { get; set; }

        /// <summary>
        /// Merit Order
        /// </summary>
        [Key(15)]
        public decimal MeritOrder { get; set; }

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        [Key(16)]
        public bool PartialQuantityAccepted { get; set; }

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        [Key(17)]
        public decimal ADJQuantity { get; set; }

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        [Key(18)]
        public decimal ADJEnergyPrice { get; set; }

        /// <summary>
        /// Quarter Value
        /// </summary>
        [Key(19)]
        public int? Quarter { get; set; }

        /// <summary>
        /// The Awarded Price
        /// </summary>
        [Key(20)]
        public decimal? AwardedPrice { get; set; }

        /// <summary>
        /// Transaction Reference
        /// </summary>
        [Key(21)]
        public string TransactionReference { get; set; }

        /// <summary>
        /// Grid Supply Point No
        /// </summary>
        [Key(22)]
        public string GridSupplyPoint { get; set; }

        /// <summary>
        /// Bilateral
        /// </summary>
        [Key(23)]
        public bool? Bilateral { get; set; }

        /// <summary>
        /// SubmittedAt
        /// </summary>
        [Key(24)]
        public LocalDateTime? SubmittedAt { get; set; }

    }
}