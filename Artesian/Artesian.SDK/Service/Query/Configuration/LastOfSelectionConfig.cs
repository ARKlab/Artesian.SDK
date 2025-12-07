// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor - Period is a value type

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Last of selection configuration
    /// </summary>
    public class LastOfSelectionConfig
    {
        /// <summary>
        /// Start date for date range
        /// </summary>
        public LocalDate? DateStart { get; set; }
        /// <summary>
        /// End date for date range
        /// </summary>
        public LocalDate? DateEnd { get; set; }
        /// <summary>
        /// Period
        /// </summary>
        public Period Period { get; set; }
        /// <summary>
        /// Period start for period range
        /// </summary>
        public Period PeriodFrom { get; set; }
        /// <summary>
        /// Period start for period range
        /// </summary>
        public Period PeriodTo { get; set; }
    }
}
