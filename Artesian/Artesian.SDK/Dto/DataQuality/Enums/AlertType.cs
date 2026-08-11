namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Determines when a quality notification alert is fired.
    /// </summary>
    public enum AlertType
    {
        /// <summary>
        /// The alert is triggered immediately when a quality check detects a failure (real-time event-driven).
        /// </summary>
        OnEvent,

        /// <summary>
        /// The alert is triggered on a defined schedule (e.g., daily digest), aggregating check results over the period.
        /// </summary>
        Scheduled
    }
}
