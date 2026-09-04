using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Trigger configuration for scheduled alerts.
    /// The notification is sent according to a defined schedule (e.g., daily digest), aggregating check results over the period.
    /// </summary>
    [MessagePackObject]
    public class ScheduleTriggerConfigDto : TriggerConfigDto
    {
        /// <summary>
        /// The schedule definition specifying when the alert digest should be sent (cron or custom reference).
        /// </summary>
        [Key("ScheduleDefinition")]
        public required ScheduleDefinitionDto ScheduleDefinition { get; set; }

        /// <summary>Gets the scheduled trigger discriminator.</summary>
        [IgnoreMember]
        public override AlertType Type => AlertType.Scheduled;
    }
}
