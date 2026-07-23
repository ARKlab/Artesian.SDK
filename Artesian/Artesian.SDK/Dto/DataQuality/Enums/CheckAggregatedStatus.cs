namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Represents the overall aggregated outcome of a data quality check execution.
    /// </summary>
    public enum CheckAggregatedStatus
    {
        /// <summary>
        /// All validated data points passed the quality check — no issues detected.
        /// </summary>
        OK,

        /// <summary>
        /// One or more data points failed the quality check (missing, late, or outlier detected).
        /// </summary>
        KO
    }
}
