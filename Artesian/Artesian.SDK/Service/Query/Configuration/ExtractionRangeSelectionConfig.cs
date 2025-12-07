// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor - Period is a value type

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
        public LocalDate DateStart{ get; set; }
        /// <summary>
        /// End date for Date Range for extraction
        /// </summary>
        public LocalDate DateEnd { get; set; }
        /// <summary>
        /// Period for extraction
        /// </summary>
        public Period Period { get; set; }
        /// <summary>
        /// Period start range for extraction
        /// </summary>
        public Period PeriodFrom { get; set; }
        /// <summary>
        /// Period end range for extraction
        /// </summary>
        public Period PeriodTo { get; set; }
        /// <summary>
        /// Relative Interval for extraction
        /// </summary>
        public RelativeInterval? Interval { get; set; }
    }
}
