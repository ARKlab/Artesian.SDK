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
        public Status Status { get; set; } = default;


        /// <summary>
        /// The BAType
        /// </summary>
        [Key(1)]
        public BAType BAType { get; set; } = default;


        /// <summary>
        /// The Scope
        /// </summary>
        [Key(2)]
        public Scope Scope { get; set; } = default;


        /// <summary>
        /// The Date
        /// </summary>
        [Key(3)]
        public LocalDate Date { get; set; } = default;

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(4)]
        public int? Hour { get; set; }

        /// <summary>
        /// The Market
        /// </summary>
        [Key(5)]
        public Market Market { get; set; } = default;


        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(6)]
        public Purpose Purpose { get; set; } = default;


        /// <summary>
        /// The Zone
        /// </summary>
        [Key(7)]
        public Zone Zone { get; set; } = default;


        /// <summary>
        /// The Unit Type
        /// </summary>
        [Key(8)]
        public UnitType UnitType { get; set; } = default;


        /// <summary>
        /// The Generation Type
        /// </summary>
        [Key(9)]
        public GenerationType GenerationType { get; set; } = default;


        /// <summary>
        /// The Unit
        /// </summary>
        /// <remarks>can be NULL for XBID, never NULL otherwise</remarks>
        [Key(10)]
        public string Unit { get; set; } = null!;


        /// <summary>
        /// The Operator
        /// </summary>
        [Key(11)]
        public string Operator { get; set; } = null!;


        /// <summary>
        /// The Quantity
        /// </summary>
        [Key(12)]
        public decimal Quantity { get; set; } = default;

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(13)]
        public decimal AwardedQuantity { get; set; } = default;

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(14)]
        public decimal EnergyPrice { get; set; } = default;

        /// <summary>
        /// Merit Order
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(15)]
        public decimal MeritOrder { get; set; } = default;

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(16)]
        public bool PartialQuantityAccepted { get; set; } = default;

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(17)]
        public decimal ADJQuantity { get; set; } = default;

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(18)]
        public decimal ADJEnergyPrice { get; set; } = default;

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
        public decimal AwardedPrice { get; set; } = default;

        /// <summary>
        /// Transaction Reference
        /// </summary>
        [Key(21)]
        public string TransactionReference { get; set; } = null!;


        /// <summary>
        /// Grid Supply Point No
        /// </summary>
        [Key(22)]
        public string GridSupplyPoint { get; set; } = null!;


        /// <summary>
        /// Bilateral
        /// </summary>
        [Key(23)]
        public bool Bilateral { get; set; } = default;

        /// <summary>
        /// SubmittedAt
        /// </summary>
        [Key(24)]
        public LocalDateTime SubmittedAt { get; set; } = default;

        /// <summary>
        /// Timestamp
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(25)]
        public LocalDateTime Timestamp { get; set; } = default;

        /// <summary>
        /// PrezzoUnitario
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(26)]
        public decimal PrezzoUnitario { get; set; } = default;

        /// <summary>
        /// Granularity of the data
        /// </summary>
        [Key(27)]
        public string Granularity { get; set; } = null!;


        /// <summary>
        /// Offer type
        /// </summary>
        [Key(28)]
        public string OfferType { get; set; } = null!;


        /// <summary>
        /// Block ID
        /// </summary>
        [Key(29)]
        public string BlockId { get; set; } = null!;


        /// <summary>
        /// Period
        /// </summary>
        [Key(30)]
        public int? Period { get; set; }

        /// <summary>
        /// Minimum Acceptance Ratio
        /// </summary>
        [Key(31)]
        public decimal MinimumAcceptanceRatio { get; set; } = default;

        /// <summary>
        /// PRODOTTO
        /// </summary>
        [Key(32)]
        public string Prodotto { get; set; } = null!;


    }
}