using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Configuration for Completeness and Freshness rules applied to versioned time series.
    /// Extends the base completeness config with version-specific tolerances to validate that
    /// expected versions are published within an acceptable window.
    /// </summary>
    [MessagePackObject]
    public class VersionedCompletenessAndFreshnessConfigDto : CompletenessAndFreshnessConfigDto
    {
        /// <summary>
        /// The minimum expected version publication offset relative to the reference time.
        /// Defines the lower bound of the acceptable version window.
        /// </summary>
        [Key("VersionToleranceFrom")]
        public required Period VersionToleranceFrom { get; set; }

        /// <summary>
        /// The maximum expected version publication offset relative to the reference time.
        /// Defines the upper bound of the acceptable version window.
        /// </summary>
        [Key("VersionToleranceTo")]
        public required Period VersionToleranceTo { get; set; }

        /// <summary>
        /// Optional precision level for truncating the cron reference time before applying
        /// <see cref="VersionToleranceFrom"/> and <see cref="VersionToleranceTo"/> offsets.
        /// When set, the reference time is zeroed to the start of the chosen unit before version
        /// window bounds are computed. Periods must not contain non-zero components smaller than
        /// the chosen precision. When <c>null</c>, no truncation occurs.
        /// </summary>
        [Key("VersionPrecision")]
        public PeriodPrecision? VersionPrecision { get; set; }
    }
}
