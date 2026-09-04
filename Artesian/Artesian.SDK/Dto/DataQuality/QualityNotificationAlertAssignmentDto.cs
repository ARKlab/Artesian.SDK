using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Represents the binding between a notification alert and a Market Data entity.
    /// Uses the Input/Output pattern: <see cref="Input"/> for write operations, <see cref="Output"/> for read operations with enriched navigation.
    /// </summary>
    public static class QualityNotificationAlertAssignmentDto
    {
        /// <summary>
        /// Write model for creating or updating an alert / Market Data assignment.
        /// Contains only the foreign-key identifiers and concurrency token.
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The unique identifier of the assignment, assigned by the server on creation.
            /// </summary>
            [Key(0)]
            public int Id { get; set; }

            /// <summary>
            /// The identifier of the notification alert that monitors the Market Data.
            /// </summary>
            [Key(1)]
            public int AlertId { get; set; }

            /// <summary>
            /// The identifier of the Market Data entity monitored by the alert.
            /// </summary>
            [Key(2)]
            public int MarketDataId { get; set; }

            /// <summary>
            /// The entity tag for optimistic concurrency control.
            /// </summary>
            [Key(3)]
            public string? ETag { get; set; }
        }

        /// <summary>
        /// Read model returned by GET operations. Extends <see cref="Input"/> with expanded navigation properties
        /// for the associated Market Data and Alert.
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
            /// <summary>
            /// The enriched Market Data entity associated with this assignment.
            /// </summary>
            [Key(4)]
            public MarketDataEntity.OutputEnriched? MarketData { get; set; }

            /// <summary>
            /// The notification alert definition associated with this assignment.
            /// </summary>
            [Key(5)]
            public QualityNotificationAlertDto.Output? Alert { get; set; }
        }
    }
}
