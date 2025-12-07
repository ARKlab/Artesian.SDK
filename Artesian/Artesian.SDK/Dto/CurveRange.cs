// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Curve Range
    /// </summary>
    [MessagePackObject]
    public class CurveRange
    {
        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        [Key(0)]
        public int MarketDataId { get; set; }

        /// <summary>
        /// The product for MAS
        /// </summary>
        [Key(1)]
        public string? Product { get; set; }

        /// <summary>
        /// The version date for Versioned 
        /// </summary>
        [Key(2)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// Last Update for this curve
        /// </summary>
        [Key(3)]
        public Instant LastUpdated { get; set; }

        /// <summary>
        /// Creation date for this curve 
        /// </summary>
        [Key(4)]
        public Instant Created { get; set; }

        /// <summary>
        /// Start date of range for this curve  
        /// </summary>
        [Key(5)]
        public LocalDate RangeStart { get; set; }

        /// <summary>
        /// End date of range for this curve  
        /// </summary>
        [Key(6)]
        public LocalDate RangeEnd { get; set; }
    }
}
