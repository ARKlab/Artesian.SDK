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

            if (data.OverrideId == Guid.Empty)
                throw new ArgumentException("UpsertCurveDataOverride OverrideId must be valorized", nameof(data));
        }
    }
}
