// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Represents the binding between a Market Data entity and a Data Quality Rule.
    /// Uses the Input/Output pattern: <see cref="Input"/> for write operations, <see cref="Output"/> for read operations with enriched navigation.
    /// </summary>
    public static class MarketDataQualityRuleAssignmentDto
    {
        /// <summary>
        /// Write model for creating or updating a Market Data / Rule assignment.
        /// Contains only the foreign-key identifiers and concurrency token.
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The unique identifier of this assignment, assigned by the server on creation.
            /// </summary>
            [Key(0)]
            public int Id { get; set; }

            /// <summary>
            /// The identifier of the Market Data entity to which the quality rule is assigned.
            /// </summary>
            [Key(1)]
            public int MarketDataId { get; set; }

            /// <summary>
            /// The identifier of the Data Quality Rule applied to the Market Data.
            /// </summary>
            [Key(2)]
            public int DataQualityRuleId { get; set; }

            /// <summary>
            /// The entity tag for optimistic concurrency control.
            /// </summary>
            [Key(3)]
            public string? ETag { get; set; }
        }

        /// <summary>
        /// Read model returned by GET operations. Extends <see cref="Input"/> with expanded navigation properties
        /// for the associated Market Data and Data Quality Rule.
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
            /// <summary>
            /// The enriched Market Data entity associated with this assignment.
            /// </summary>
            [Key(4)]
            public MarketDataEntity.OutputEnriched? MarketData { get; set; }

            /// <summary>
            /// The Data Quality Rule definition associated with this assignment, including its aggregated status.
            /// </summary>
            [Key(5)]
            public DataQualityRuleDto.Output? DataQualityRule { get; set; }

            /// <summary>
            /// The lookback date from which data quality checks are evaluated for this assignment.
            /// </summary>
            [Key(6)]
            public Instant? LookbackDate { get; set; }

            /// <summary>
            /// The version number for concurrency tracking.
            /// </summary>
            [Key(7)]
            public int Version { get; set; }
        }
    }
}
