using MessagePack;

using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Defines when and how often a quality check should be executed.
    /// Combines a schedule definition (e.g., cron expression) with a maximum allowed delay.
    /// </summary>
    [MessagePackObject]
    public class ScheduleConfigDto
    {
        /// <summary>
        /// The schedule definition specifying the check execution pattern (cron expression or custom reference).
        /// </summary>
        [Key(0)]
        public required ScheduleDefinitionDto ScheduleDefinition { get; set; }

        /// <summary>
        /// The maximum acceptable delay between the expected data availability time and the actual check.
        /// Data arriving after this delay is flagged as "InLate".
        /// </summary>
        [Key(1)]
        public required Period MaxDelay { get; set; }
    }
}
