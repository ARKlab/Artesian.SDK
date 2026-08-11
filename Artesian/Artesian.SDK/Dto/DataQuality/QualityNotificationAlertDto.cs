using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Defines a notification alert for data quality events.
    /// </summary>
    public static class QualityNotificationAlertDto
    {
        /// <summary>
        /// Write model for creating or updating a notification alert.
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>The server-assigned notification alert identifier.</summary>
            [Key(0)]
            public int Id { get; set; }

            /// <summary>The human-readable name of the alert.</summary>
            [Key(1)]
            public required string Name { get; set; }

            /// <summary>The trigger configuration that determines when notifications are sent.</summary>
            [Key(2)]
            public required TriggerConfigDto TriggerConfig { get; set; }

            /// <summary>The configured email notification recipients.</summary>
            [Key(3)]
            public List<MailNotificationDto> MailNotifications { get; set; } = new();

            /// <summary>The ETag used for optimistic concurrency control.</summary>
            [Key(4)]
            public string? ETag { get; set; }

            /// <summary>The version counter used to guard deferred alert schedule messages.</summary>
            [Key(5)]
            public int Version { get; set; }
        }

        /// <summary>
        /// Read model returned by GET operations.
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
        }

    }

    /// <summary>
    /// Abstract base class for notification alert trigger configurations.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(OnEventTriggerConfigDto))]
    [Union(1, typeof(ScheduleTriggerConfigDto))]
    [JsonConverter(typeof(TriggerConfigConverterSTJ))]
    public abstract class TriggerConfigDto
    {
        /// <summary>Gets the trigger type discriminator.</summary>
        [IgnoreMember]
        public abstract AlertType Type { get; }
    }

    /// <summary>
    /// Trigger configuration for event-driven alerts.
    /// </summary>
    [MessagePackObject]
    public class OnEventTriggerConfigDto : TriggerConfigDto
    {
        /// <summary>Gets the on-event trigger discriminator.</summary>
        [IgnoreMember]
        public override AlertType Type => AlertType.OnEvent;
    }

    /// <summary>
    /// Trigger configuration for scheduled alerts.
    /// </summary>
    [MessagePackObject]
    public class ScheduleTriggerConfigDto : TriggerConfigDto
    {
        /// <summary>Gets or sets the schedule used to send the alert digest.</summary>
        [Key("ScheduleDefinition")]
        public required ScheduleDefinitionDto ScheduleDefinition { get; set; }

        /// <summary>Gets the scheduled trigger discriminator.</summary>
        [IgnoreMember]
        public override AlertType Type => AlertType.Scheduled;
    }

    /// <summary>
    /// Email notification configuration for quality alerts.
    /// </summary>
    [MessagePackObject]
    public class MailNotificationDto
    {
        /// <summary>The email addresses that receive the notification.</summary>
        [Key(0)]
        public string[] Recipients { get; set; } = System.Array.Empty<string>();
    }
}
