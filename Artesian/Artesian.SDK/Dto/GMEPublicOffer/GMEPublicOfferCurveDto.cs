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
        public Status Status { get; set; }

        /// <summary>
        /// The BAType
        /// </summary>
        [Key(1)]
        public BAType BAType { get; set; }

        /// <summary>
        /// The Scope
        /// </summary>
        [Key(2)]
        public Scope Scope { get; set; }

        /// <summary>
        /// The Date
        /// </summary>
        [Key(3)]
        public LocalDate? Date { get; set; }

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(4)]
        public int? Hour { get; set; }

        /// <summary>
        /// The Market
        /// </summary>
        [Key(5)]
        public Market Market { get; set; }

        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(6)]
        public Purpose Purpose { get; set; }

        /// <summary>
        /// The Zone
        /// </summary>
        [Key(7)]
        public Zone Zone { get; set; }

        /// <summary>
        /// The Unit Type
        /// </summary>
        [Key(8)]
        public UnitType UnitType { get; set; }

        /// <summary>
        /// The Generation Type
        /// </summary>
        [Key(9)]
        public GenerationType GenerationType { get; set; }

        /// <summary>
        /// The Unit
        /// </summary>
        /// <remarks>can be NULL for XBID, never NULL otherwise</remarks>
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
        public decimal? Quantity { get; set; }

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(13)]
        public decimal? AwardedQuantity { get; set; }

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(14)]
        public decimal? EnergyPrice { get; set; }

        /// <summary>
        /// Merit Order
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(15)]
        public decimal? MeritOrder { get; set; }

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(16)]
        public bool? PartialQuantityAccepted { get; set; }

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(17)]
        public decimal? ADJQuantity { get; set; }

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(18)]
        public decimal? ADJEnergyPrice { get; set; }

        /// <summary>
        /// Quarter Value
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(19)]
        public int? Quarter { get; set; }

        /// <summary>
        /// The Awarded Price
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
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

        /// <summary>
        /// Timestamp
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(25)]
        public LocalDateTime? Timestamp { get; set; }

        /// <summary>
        /// PrezzoUnitario
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(26)]
        public decimal? PrezzoUnitario { get; set; }

        /// <summary>
        /// Granularity of the data
        /// </summary>
        [Key(27)]
        public string Granularity { get; set; }

        /// <summary>
        /// Offer type
        /// </summary>
        [Key(28)]
        public string OfferType { get; set; }

        /// <summary>
        /// Block ID
        /// </summary>
        [Key(29)]
        public string BlockId { get; set; }

        /// <summary>
        /// Period
        /// </summary>
        [Key(30)]
        public int? Period { get; set; }

        /// <summary>
        /// Minimum Acceptance Ratio
        /// </summary>
        [Key(31)]
        public decimal? MinimumAcceptanceRatio { get; set; }

        /// <summary>
        /// PRODOTTO
        /// </summary>
        [Key(32)]
        public string Prodotto { get; set; }

    }
}