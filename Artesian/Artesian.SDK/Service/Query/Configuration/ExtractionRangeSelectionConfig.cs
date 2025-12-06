// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Extraction range selection configuration
    /// </summary>
    public class ExtractionRangeSelectionConfig
    {
        /// <summary>
        /// Start date for Date Range for extraction
        /// </summary>
        public LocalDate DateStart{ get; set; } = null!;

        /// <summary>
        /// End date for Date Range for extraction
        /// </summary>
        public LocalDate DateEnd { get; set; } = null!;

        /// <summary>
        /// Period for extraction
        /// </summary>
        public Period Period { get; set; } = null!;

        /// <summary>
        /// Period start range for extraction
        /// </summary>
        public Period PeriodFrom { get; set; } = null!;

        /// <summary>
        /// Period end range for extraction
        /// </summary>
        public Period PeriodTo { get; set; } = null!;

        /// <summary>
        /// Relative Interval for extraction
        /// </summary>
        public RelativeInterval Interval { get; set; } = null!;

    }
}
