using Artesian.SDK.Dto.Override.Enum;

using MessagePack;

using System;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Payload for upserting an override or fallback correction.
    /// Inherits all curve data from <see cref="UpsertCurveData"/> and adds the
    /// override-specific metadata required to describe the correction.
    /// </summary>
    [MessagePackObject]
    public record UpsertCurveDataOverride : UpsertCurveData
    {
        /// <summary>
        /// Whether the write is an <see cref="OverrideKind.Override"/> or an <see cref="OverrideKind.Fallback"/>.
        /// </summary>
        [Required]
        [MessagePack.Key(12)]
        public OverrideKind Kind { get; set; }

        /// <summary>
        /// The id of an existing override/fallback to update/merge/replace. When null, a new entry is created.
        /// </summary>
        [MessagePack.Key(13)]
        public Guid? OverrideId { get; set; }

        /// <summary>
        /// When true, an overlapping override/fallback of the same kind is trimmed/replaced to stay
        /// consistent with this write; previously overwritten data on the overlap are lost.
        /// </summary>
        [MessagePack.Key(14)]
        public bool ReplaceExisting { get; set; }

        /// <summary>
        /// Optional free-text comment describing the reason for the override/fallback.
        /// </summary>
        [MessagePack.Key(15)]
        public string? Comment { get; set; }
    }

    internal static class UpsertCurveDataOverrideExt
    {
        public static void Validate(this UpsertCurveDataOverride data)
        {
            new UpsertCurveData
            {
                ID = data.ID,
                Version = data.Version,
                Timezone = data.Timezone,
                DownloadedAt = data.DownloadedAt,
                MarketAssessment = data.MarketAssessment,
                Rows = data.Rows,
                DeferCommandExecution = data.DeferCommandExecution,
                DeferDataGeneration = data.DeferDataGeneration,
                KeepNulls = data.KeepNulls,
                AuctionRows = data.AuctionRows,
                BidAsk = data.BidAsk,
                UpsertMode = data.UpsertMode,
            }.Validate();

            if (!Enum.IsDefined(typeof(OverrideKind), data.Kind))
                throw new ArgumentException("UpsertCurveDataOverride Kind must be a valid value", nameof(data));

            if (data.OverrideId.HasValue && data.ReplaceExisting)
                throw new ArgumentException("UpsertCurveDataOverride ReplaceExisting cannot be combined with OverrideId", nameof(data));

            if (data.OverrideId == Guid.Empty)
                throw new ArgumentException("UpsertCurveDataOverride OverrideId must be valorized", nameof(data));

            if (data.Comment?.Length > 4000)
                throw new ArgumentException("UpsertCurveDataOverride Comment cannot exceed 4000 characters", nameof(data));

            var hasRows = data.Rows != null && data.Rows.Count > 0;
            var hasMarketAssessment = data.MarketAssessment != null && data.MarketAssessment.Count > 0;
            var hasAuctionRows = data.AuctionRows != null && data.AuctionRows.Count > 0;
            var hasBidAsk = data.BidAsk != null && data.BidAsk.Count > 0;
            if (!hasRows && !hasMarketAssessment && !hasAuctionRows && !hasBidAsk)
                throw new ArgumentException("UpsertCurveDataOverride must contain at least one of Rows, MarketAssessment, AuctionRows, or BidAsk", nameof(data));

            if (data.UpsertMode.HasValue && data.UpsertMode.Value != UpsertMode.Merge)
                throw new ArgumentException("UpsertCurveDataOverride UpsertMode must be null or Merge", nameof(data));
        }
    }
}
