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
            /// <summary>
            /// The unique identifier of this notification alert, assigned by the server on creation.
            /// </summary>
            [Key(0)]
            public int Id { get; set; }

            /// <summary>
            /// A human-readable name for this alert (e.g., "Weather station daily digest").
            /// </summary>
            [Key(1)]
            public required string Name { get; set; }

            /// <summary>
            /// The trigger configuration determining when notifications are sent.
            /// Can be <see cref="OnEventTriggerConfigDto"/> for immediate alerts or <see cref="ScheduleTriggerConfigDto"/> for scheduled digests.
            /// </summary>
            [Key(2)]
            public required TriggerConfigDto TriggerConfig { get; set; }

            /// <summary>The configured email notification recipients.</summary>
            [Key(3)]
            public List<MailNotificationDto> MailNotifications { get; set; } = new();

            /// <summary>
            /// The entity tag for optimistic concurrency control.
            /// </summary>
            [Key(4)]
            public string? ETag { get; set; }

            /// <summary>
            /// Monotonically increasing version counter, incremented on each update.
            /// Used as a guard for deferred alert schedule messages.
            /// </summary>
            [Key(5)]
            public int Version { get; set; }
        }

        /// <summary>
        /// Read model returned by GET operations. Extends <see cref="Input"/> with the server-assigned identifier.
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
        }
    }
}
