// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Lightweight DTO pairing a Data Quality Rule with its aggregated DQ status summary.
    /// Used by the DqRule DQ Status Summary endpoint.
    /// </summary>
    [MessagePackObject]
    public class DqRuleDqStatusSummaryDto
    {
        /// <summary>
        /// The Data Quality Rule identifier.
        /// </summary>
        [Key(0)]
        public int RuleId { get; set; }

        /// <summary>
        /// The aggregated DQ status summary for this rule (across all assigned Market Data, or filtered by a specific Market Data).
        /// </summary>
        [Key(1)]
        public DataQualityStatusSummaryDto? StatusSummary { get; set; }
    }
}
