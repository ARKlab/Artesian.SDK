// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// CurveRange-like summary per data quality assignment.
    /// Provides a checksummary view with range metadata, modeled after CurveRangeV2.
    /// </summary>
    [MessagePackObject]
    public class CheckResultCheckSummaryDto
    {
        /// <summary>
        /// The enriched assignment (expanded with OutputEnriched + Rule).
        /// </summary>
        [Key(0)]
        public MarketDataQualityRuleAssignmentDto.Output? Assignment { get; set; }

        /// <summary>
        /// Timestamp of the last quality check execution.
        /// </summary>
        [Key(1)]
        public Instant LastCheckTime { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [Key(2)]
        public string? Product { get; set; }

        /// <summary>
        /// Version timestamp (null for non-versioned time series).
        /// </summary>
        [Key(3)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// Last time the check result was updated.
        /// </summary>
        [Key(4)]
        public Instant LastUpdated { get; set; }

        /// <summary>
        /// Time when the check result was created.
        /// </summary>
        [Key(5)]
        public Instant Created { get; set; }

        /// <summary>
        /// Start of the checked data range.
        /// </summary>
        [Key(6)]
        public LocalDate RangeStart { get; set; }

        /// <summary>
        /// End of the checked data range.
        /// </summary>
        [Key(7)]
        public LocalDate RangeEnd { get; set; }

        /// <summary>
        /// Exact start of the checked range (with time component).
        /// </summary>
        [Key(8)]
        public LocalDateTime? RangeExactStart { get; set; }

        /// <summary>
        /// Exact end of the checked range (with time component).
        /// </summary>
        [Key(9)]
        public LocalDateTime? RangeExactEnd { get; set; }

        /// <summary>
        /// Aggregated quality status (OK = no issues, KO = failures detected).
        /// </summary>
        [Key(10)]
        public CheckAggregatedStatus AggregatedStatus { get; set; }

        /// <summary>
        /// Version-from boundary for versioned time series (null for actuals).
        /// </summary>
        [Key(11)]
        public LocalDateTime? VersionFrom { get; set; }
    }
}
