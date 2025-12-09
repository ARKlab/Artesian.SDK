using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferCurve class
    /// </summary>
    [MessagePackObject]
    public record GMEPublicOfferCurveDto
    {
        /// <summary>
        /// The Status
        /// </summary>
        [Key(0)]
        public required Status Status { get; init; }

        /// <summary>
        /// The BAType
        /// </summary>
        [Key(1)]
        public required BAType BAType { get; init; }

        /// <summary>
        /// The Scope
        /// </summary>
        [Key(2)]
        public required Scope Scope { get; init; }

        /// <summary>
        /// The Date
        /// </summary>
        [Key(3)]
        public LocalDate? Date { get; init; }

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(4)]
        public int? Hour { get; init; }

        /// <summary>
        /// The Market
        /// </summary>
        [Key(5)]
        public required Market Market { get; init; }

        /// <summary>
        /// The Purpose
        /// </summary>
        [Key(6)]
        public required Purpose Purpose { get; init; }

        /// <summary>
        /// The Zone
        /// </summary>
        [Key(7)]
        public required Zone Zone { get; init; }

        /// <summary>
        /// The Unit Type
        /// </summary>
        [Key(8)]
        public required UnitType UnitType { get; init; }

        /// <summary>
        /// The Generation Type
        /// </summary>
        [Key(9)]
        public required GenerationType GenerationType { get; init; }

        /// <summary>
        /// The Unit
        /// </summary>
        /// <remarks>can be NULL for XBID, never NULL otherwise</remarks>
        [Key(10)]
        public string? Unit { get; init; }

        /// <summary>
        /// The Operator
        /// </summary>
        [Key(11)]
        public string? Operator { get; init; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Key(12)]
        public decimal? Quantity { get; init; }

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(13)]
        public decimal? AwardedQuantity { get; init; }

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(14)]
        public decimal? EnergyPrice { get; init; }

        /// <summary>
        /// Merit Order
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(15)]
        public decimal? MeritOrder { get; init; }

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(16)]
        public bool? PartialQuantityAccepted { get; init; }

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(17)]
        public decimal? ADJQuantity { get; init; }

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(18)]
        public decimal? ADJEnergyPrice { get; init; }

        /// <summary>
        /// Quarter Value
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(19)]
        public int? Quarter { get; init; }

        /// <summary>
        /// The Awarded Price
        /// </summary>
        /// <remarks>always NULL for XBID, never NULL otherwise</remarks>
        [Key(20)]
        public decimal? AwardedPrice { get; init; }

        /// <summary>
        /// Transaction Reference
        /// </summary>
        [Key(21)]
        public string? TransactionReference { get; init; }

        /// <summary>
        /// Grid Supply Point No
        /// </summary>
        [Key(22)]
        public string? GridSupplyPoint { get; init; }

        /// <summary>
        /// Bilateral
        /// </summary>
        [Key(23)]
        public bool? Bilateral { get; init; }

        /// <summary>
        /// SubmittedAt
        /// </summary>
        [Key(24)]
        public LocalDateTime? SubmittedAt { get; init; }

        /// <summary>
        /// Timestamp
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(25)]
        public LocalDateTime? Timestamp { get; init; }

        /// <summary>
        /// PrezzoUnitario
        /// </summary>
        /// <remarks>Added for XBID, NULL otherwise</remarks>
        [Key(26)]
        public decimal? PrezzoUnitario { get; init; }

        /// <summary>
        /// Granularity of the data
        /// </summary>
        [Key(27)]
        public string? Granularity { get; init; }

        /// <summary>
        /// Offer type
        /// </summary>
        [Key(28)]
        public string? OfferType { get; init; }

        /// <summary>
        /// Block ID
        /// </summary>
        [Key(29)]
        public string? BlockId { get; init; }

        /// <summary>
        /// Period
        /// </summary>
        [Key(30)]
        public int? Period { get; init; }

        /// <summary>
        /// Minimum Acceptance Ratio
        /// </summary>
        [Key(31)]
        public decimal? MinimumAcceptanceRatio { get; init; }

        /// <summary>
        /// PRODOTTO
        /// </summary>
        [Key(32)]
        public string? Prodotto { get; init; }

    }
}