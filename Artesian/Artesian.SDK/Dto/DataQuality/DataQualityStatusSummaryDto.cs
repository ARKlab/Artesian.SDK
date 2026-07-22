// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Provides an at-a-glance quality status summary for a specific Market Data entity.
    /// This is an enrichment of the MarketData concept, giving immediate information about its quality state.
    /// </summary>
    [MessagePackObject]
    public class DataQualityStatusSummaryDto
    {
        [Key(0)]
        public Instant? LastCheckTime { get; set; }
        /// <summary>
        /// The overall aggregated quality status across all active rule assignments (OK if all pass, KO if any fail).
        /// </summary>
        [Key(1)]
        public CheckAggregatedStatus OverallStatus { get; set; }

        /// <summary>
        /// The number of currently active quality rule assignments bound to this Market Data.
        /// </summary>
        [Key(2)]
        public int ActiveRulesCount { get; set; }

        /// <summary>
        /// The number of active rule assignments whose last check resulted in a failure (KO).
        /// </summary>
        [Key(3)]
        public int FailedRulesCount { get; set; }

        /// <summary>
        /// The start of the validated data range for this Market Data (analogous to <c>MarketDataCurveSummaryDto.DataRangeStart</c>).
        /// Null if no check results are available yet.
        /// </summary>
        [Key(4)]
        public LocalDate? From { get; set; }

        /// <summary>
        /// The end of the validated data range for this Market Data (analogous to <c>MarketDataCurveSummaryDto.DataRangeEnd</c>).
        /// Null if no check results are available yet.
        /// </summary>
        [Key(5)]
        public LocalDate? To { get; set; }
    }
}
