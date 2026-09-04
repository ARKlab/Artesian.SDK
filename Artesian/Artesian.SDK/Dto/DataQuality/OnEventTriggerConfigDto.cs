using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{

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
}
