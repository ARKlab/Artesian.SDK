namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Defines the statistical model used for outlier detection in data quality rules.
    /// </summary>
    public enum OutlierModel
    {
        /// <summary>
        /// Uses fixed upper and lower bounds — a data point is an outlier if it falls outside the specified absolute range.
        /// </summary>
        AbsoluteBound,

        /// <summary>
        /// Compares data points against a reference market data curve, flagging values that deviate beyond a configurable tolerance percentage.
        /// </summary>
        RefCurve
    }
}
