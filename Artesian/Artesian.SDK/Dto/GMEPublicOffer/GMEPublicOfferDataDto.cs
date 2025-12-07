using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// GMEPublicOfferDataDto class
    /// </summary>
    [MessagePackObject]
    public record GMEPublicOfferDataDto
    {

        /// <summary>
        /// The Date
        /// </summary>
        [Key(0)]
        public required LocalDate Date { get; init; }

        /// <summary>
        /// The Hour
        /// </summary>
        [Key(1)]
        public required int Hour { get; init; }

        /// <summary>
        /// Quarter Value
        /// </summary>
        [Key(2)]
        public required int Quarter { get; init; }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Key(3)]
        public required decimal Quantity { get; init; }

        /// <summary>
        /// The Awarded Quantity
        /// </summary>
        [Key(4)]
        public required decimal AwardedQuantity { get; init; }

        /// <summary>
        /// The Awarded Price
        /// </summary>
        [Key(5)]
        public required decimal AwardedPrice { get; init; }

        /// <summary>
        /// The Energy Price
        /// </summary>
        [Key(6)]
        public required decimal EnergyPrice { get; init; }

        /// <summary>
        /// Merit Order
        /// </summary>
        [Key(7)]
        public required decimal MeritOrder { get; init; }

        /// <summary>
        /// Partial Quantity Accepted
        /// </summary>
        [Key(8)]
        public required bool PartialQuantityAccepted { get; init; }

        /// <summary>
        /// Adjacent Quantity
        /// </summary>
        [Key(9)]
        public required decimal ADJQuantity { get; init; }

        /// <summary>
        /// Adjacent Energy Price
        /// </summary>
        [Key(10)]
        public required decimal ADJEnergyPrice { get; init; }

        /// <summary>
        /// Transaction Reference
        /// </summary>
        [Key(11)]
        public string? TransactionReference { get; init; }

        /// <summary>
        /// Grid Supply Point No
        /// </summary>
        [Key(12)]
        public string? GridSupplyPoint { get; init; }

        /// <summary>
        /// Bilateral
        /// </summary>
        [Key(13)]
        public bool? Bilateral { get; init; }

        /// <summary>
        /// SubmittedAt
        /// </summary>
        [Key(14)]
        public LocalDateTime? SubmittedAt { get; init; }

        /// <summary>
        /// Timestamp for Xbid data
        /// </summary>
        [Key(15)]
        public LocalDateTime? Timestamp { get; init; }

        /// <summary>
        /// PrezzoUnitario for Xbid data
        /// </summary>
        [Key(16)]
        public decimal? PrezzoUnitario { get; init; }

        /// <summary>
        /// 15 Minute Period In Day
        /// </summary>
        [Key(17)]
        public int? Period { get; init; }

        /// <summary>
        /// MINIMUM ACCEPTANCE RATIO
        /// </summary>
        [Key(18)]
        public decimal? MinimumAcceptanceRatio { get; init; }

        /// <summary>
        /// PRODOTTO
        /// </summary>
        [Key(19)]
        public string? Prodotto { get; init; }

    }
}