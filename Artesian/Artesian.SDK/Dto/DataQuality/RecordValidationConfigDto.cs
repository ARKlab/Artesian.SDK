using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Defines the expected data presence window for a quality check.
    /// Specifies which time range of records should be validated and the granularity profile or calendar for determining expected slots.
    /// </summary>
    [MessagePackObject]
    public class RecordValidationConfigDto
    {
        /// <summary>
        /// The start offset (relative to the check execution time) defining where the expected data window begins.
        /// </summary>
        [Key(0)]
        public required Period RecordRangeFrom { get; set; }

        /// <summary>
        /// The end offset (relative to the check execution time) defining where the expected data window ends.
        /// </summary>
        [Key(1)]
        public required Period RecordRangeTo { get; set; }

        /// <summary>
        /// Optional precision level for truncating the cron reference time before applying Period offsets.
        /// When set, the reference time is zeroed to the start of the chosen unit (e.g. <c>Day</c> → midnight)
        /// before <see cref="RecordRangeFrom"/> and <see cref="RecordRangeTo"/> are added.
        /// Periods must not contain non-zero components smaller than the chosen precision.
        /// When <c>null</c>, no truncation occurs and behavior is identical to previous versions.
        /// </summary>
        [Key(2)]
        public PeriodPrecision? Precision { get; set; }
    }
}