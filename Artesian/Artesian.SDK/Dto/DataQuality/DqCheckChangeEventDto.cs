// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// API output representation of a DQ check status change event.
    /// Maps from DqCheckChangeEvent.V1 with a combined <see cref="Output.RangeImpacted"/> range.
    /// </summary>
    public static class DqCheckChangeEventDto
    {
        /// <summary>
        /// Output model for data quality check change events.
        /// </summary>
        [MessagePackObject]
        public class Output
        {
            /// <summary>
            /// The identifier of the Market Data entity.
            /// </summary>
            [Key(0)]
            public int MarketDataId { get; set; }

            /// <summary>
            /// The identifier of the Data Quality Rule.
            /// </summary>
            [Key(1)]
            public int RuleId { get; set; }

            /// <summary>
            /// The identifier of the assignment binding the rule to the market data.
            /// </summary>
            [Key(2)]
            public int AssignmentId { get; set; }

            /// <summary>
            /// The version timestamp for versioned time series, or null for actual time series.
            /// </summary>
            [Key(3)]
            public LocalDateTime? Version { get; set; }

            /// <summary>
            /// The product identifier within the market data curve.
            /// </summary>
            [Key(4)]
            public string? Product { get; set; }

            /// <summary>
            /// The impacted time range (start inclusive, end exclusive).
            /// </summary>
            [Key(5)]
            public required LocalDateTimeRange RangeImpacted { get; set; }

            /// <summary>
            /// The new aggregated check status after the change.
            /// </summary>
            [Key(6)]
            public CheckAggregatedStatus NewStatus { get; set; }

            /// <summary>
            /// The previous aggregated check status before the change, or null if this is the first check.
            /// </summary>
            [Key(7)]
            public CheckAggregatedStatus? OldStatus { get; set; }

            /// <summary>
            /// The instant when this status change occurred.
            /// </summary>
            [Key(8)]
            public Instant Timestamp { get; set; }

            /// <summary>
            /// The human-readable name of the Data Quality Rule.
            /// </summary>
            [Key(9)]
            public string? RuleName { get; set; }

            /// <summary>
            /// The version number of the rule configuration at the time of the check.
            /// </summary>
            [Key(10)]
            public int RuleVersion { get; set; }

            /// <summary>
            /// The name of the Market Data entity.
            /// </summary>
            [Key(11)]
            public string? MarketDataName { get; set; }

            /// <summary>
            /// The provider name of the Market Data entity.
            /// </summary>
            [Key(12)]
            public string? Provider { get; set; }
        }
    }
}
