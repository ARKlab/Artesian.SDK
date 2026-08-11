using MessagePack;
using NodaTime;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// API output wrapping a schedule occurrence with its materialized DQ change events.
    /// </summary>
    public static class AlertScheduleEventsDto
    {
        /// <summary>
        /// Read model containing the materialized data quality events for one schedule occurrence.
        /// </summary>
        [MessagePackObject]
        public class Output
        {
            /// <summary>
            /// The schedule occurrence timestamp for which the event set was materialized.
            /// </summary>
            [Key(0)]
            public Instant? ScheduleTime { get; set; }

            /// <summary>
            /// The data quality check change events for this schedule occurrence.
            /// </summary>
            [Key(1)]
            public DqCheckChangeEventDto.Output[] Events { get; set; } = System.Array.Empty<DqCheckChangeEventDto.Output>();
        }

    }
}
