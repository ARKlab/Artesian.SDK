using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Abstract base class for schedule definitions used by quality checks and notification alerts.
    /// Concrete subtype: <see cref="CronScheduleDefinitionDto"/> for cron-based schedules.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(CronScheduleDefinitionDto))]
    [JsonConverter(typeof(ScheduleDefinitionConverterSTJ))]
    public abstract class ScheduleDefinitionDto
    {
        /// <summary>
        /// Discriminator property identifying the schedule definition type.
        /// </summary>
        [IgnoreMember]
        public abstract ScheduleDefinitionType Type { get; }
    }
}
