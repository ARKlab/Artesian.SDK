using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// A schedule definition based on a cron expression, specifying recurring check times in a given time zone.
    /// </summary>
    [MessagePackObject]
    public class CronScheduleDefinitionDto : ScheduleDefinitionDto
    {
        /// <summary>
        /// The cron expression defining the schedule pattern (e.g., "0 8 * * *" for daily at 08:00).
        /// </summary>
        [Key("CronExpression")]
        public string? CronExpression { get; set; }

        /// <summary>
        /// The IANA time zone identifier in which the cron expression is evaluated (e.g., "Europe/Rome").
        /// </summary>
        [Key("TimeZone")]
        public string? TimeZone { get; set; }

        /// <inheritdoc />
        [IgnoreMember]
        public override ScheduleDefinitionType Type => ScheduleDefinitionType.Cron;
    }
}
