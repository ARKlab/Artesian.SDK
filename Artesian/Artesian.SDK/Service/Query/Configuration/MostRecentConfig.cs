using NodaTime;

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
        public Period Period { get; set; } = null!;

        /// <summary>
        /// Period start for period range
        /// </summary>
        public Period PeriodFrom { get; set; } = null!;

        /// <summary>
        /// Period start for period range
        /// </summary>
        public Period PeriodTo { get; set; } = null!;

    }
}
