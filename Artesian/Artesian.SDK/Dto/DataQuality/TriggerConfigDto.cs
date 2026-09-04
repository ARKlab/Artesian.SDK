using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.DataQuality.Serialize;

using MessagePack;

using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Abstract base class for notification alert trigger configurations.
    /// Determines when a quality notification alert is fired.
    /// Concrete subtypes: <see cref="OnEventTriggerConfigDto"/> for immediate event-driven triggers,
    /// <see cref="ScheduleTriggerConfigDto"/> for scheduled digest triggers.
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(OnEventTriggerConfigDto))]
    [Union(1, typeof(ScheduleTriggerConfigDto))]
    [JsonConverter(typeof(TriggerConfigConverterSTJ))]
    public abstract class TriggerConfigDto
    {
        /// <summary>
        /// Discriminator indicating the alert trigger type.
        /// </summary>
        [IgnoreMember]
        public abstract AlertType Type { get; }
    }
}
