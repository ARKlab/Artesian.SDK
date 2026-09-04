
namespace Artesian.SDK.Dto.Override.Enum
{
    /// <summary>
    /// Discriminates the kind of user-provided correction stored alongside the original Market Data.
    /// </summary>
    public enum OverrideKind
    {
        /// <summary>
        /// A correction that overwrites the original value on a given range. The override value
        /// always takes precedence over the original value for the affected range.
        /// </summary>
        Override,

        /// <summary>
        /// A transient value used only while the original data does not pass the Data Quality check.
        /// As soon as the original data becomes valid, the fallback is no longer used.
        /// </summary>
        Fallback
    }
}
