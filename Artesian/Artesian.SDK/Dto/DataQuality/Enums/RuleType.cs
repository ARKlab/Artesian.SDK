namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Defines the type of a Data Quality rule, determining which validation logic and configuration is applied.
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// Validates that expected data records are present (completeness) and arrive within an acceptable delay (freshness).
        /// </summary>
        CompletenessAndFreshness,

        /// <summary>
        /// Detects anomalous data points using statistical models such as absolute bounds, moving averages, reference curves, or AI-based analysis.
        /// </summary>
        Outlier
    }
}
