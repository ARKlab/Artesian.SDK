// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;

using System.Collections.Generic;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// DTO pairing a Market Data entity with its aggregated DQ status summary,
    /// its full enriched Market Data entity and the list of rule assignments
    /// (carrying LookbackDate) for the Data Quality overview.
    /// Used by the MarketData DQ Status Summary endpoint.
    /// </summary>
    [MessagePackObject]
    public class MarketDataDqStatusSummaryDto
    {
        /// <summary>
        /// The Market Data entity identifier.
        /// </summary>
        [Key(0)]
        public int MarketDataId { get; set; }

        /// <summary>
        /// The aggregated DQ status summary for this Market Data under the queried rule.
        /// </summary>
        [Key(1)]
        public DataQualityStatusSummaryDto? StatusSummary { get; set; }

        /// <summary>
        /// The full enriched Market Data entity. Null if no rule assignment exists for this Market Data.
        /// </summary>
        [Key(2)]
        public MarketDataEntity.OutputEnriched? MarketData { get; set; }

        /// <summary>
        /// The Data Quality rule assignments bound to this Market Data (respecting the queried rule filter).
        /// Each assignment carries its LookbackDate, used to drive the Data Quality overview.
        /// </summary>
        [Key(3)]
        public IEnumerable<MarketDataQualityRuleAssignmentDto.Output>? Assignments { get; set; }
    }
}
