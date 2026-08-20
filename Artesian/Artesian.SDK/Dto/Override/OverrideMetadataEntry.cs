using Artesian.SDK.Dto.Override.Enum;

using MessagePack;

using NodaTime;

using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Metadata record identifying an override/fallback applied to a Market Data.
    /// It describes the affected range, who created it and when, the <see cref="OverrideKind"/>,
    /// and an optional free-text comment. Returned by the override/fallback write endpoint.
    /// </summary>
    /// <remarks>
    /// This is the <em>current-state</em> descriptor of a correction; it is not a full audit trail
    /// (records can be replaced and deletes do not reinstate prior overrides).
    /// </remarks>
    [MessagePackObject]
    public record OverrideMetadataEntry
    {
        /// <summary>
        /// The unique identifier of the override/fallback metadata, assigned by the server on write.
        /// </summary>
        [Key(0)]
        public Guid? Id { get; set; }

        /// <summary>
        /// The identifier of the Market Data this override/fallback refers to (shared Market Data registry).
        /// </summary>
        [Key(1)]
        public int MarketDataId { get; set; }

        /// <summary>
        /// Whether this entry is an <see cref="OverrideKind.Override"/> or a <see cref="OverrideKind.Fallback"/>.
        /// </summary>
        [Key(2)]
        public OverrideKind Kind { get; set; }

        /// <summary>
        /// Version for versioned data; part of the curve-range PK link. Null for Actual/MAS.
        /// </summary>
        [Key(3)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// Product for MAS/BidAsk; part of the curve-range PK link. Empty for Actual/Versioned.
        /// </summary>
        [Key(4)]
        public string? Product { get; set; }

        /// <summary>
        /// Referenced market data ID; part of the curve-range PK link.
        /// </summary>
        [Key(5)]
        public int ReferencedMarketDataId { get; set; }

        /// <summary>
        /// Effective range start.
        /// </summary>
        [Key(6)]
        public LocalDateTime RangeExactStart { get; set; }

        /// <summary>
        /// Effective range end.
        /// </summary>
        [Key(7)]
        public LocalDateTime RangeExactEnd { get; set; }

        /// <summary>
        /// The principal (who) that created the override/fallback.
        /// </summary>
        [Key(8)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// The UTC timestamp (when) the override/fallback was created.
        /// </summary>
        [Key(9)]
        public Instant CreatedAt { get; set; }

        /// <summary>
        /// Optional free-text comment describing the reason for the override/fallback.
        /// </summary>
        [Key(10)]
        public string? Comment { get; set; }
    }
}
