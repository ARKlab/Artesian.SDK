using NodaTime;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor - Period is a value type

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Most Recent selection configuration
    /// </summary>
    public class MostRecentConfig
    {
        /// <summary>
        /// Start date for date range
        /// </summary>
        public LocalDateTime? DateStart { get; set; }
        /// <summary>
        /// End date for date range
        /// </summary>
        public LocalDateTime? DateEnd { get; set; }
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
