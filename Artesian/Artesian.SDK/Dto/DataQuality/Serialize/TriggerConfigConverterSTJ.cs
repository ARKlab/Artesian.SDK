using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Serialize;

using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    sealed class TriggerConfigConverterSTJ : JsonPolymorphicConverter<TriggerConfigDto, AlertType>
    {
        public TriggerConfigConverterSTJ()
            : base(nameof(TriggerConfigDto.Type))
        {
        }

        protected override Type GetType(AlertType discriminatorValue)
        {
            return discriminatorValue switch
            {
                AlertType.OnEvent => typeof(OnEventTriggerConfigDto),
                AlertType.Scheduled => typeof(ScheduleTriggerConfigDto),
                _ => throw new InvalidOperationException($"Can't deserialize TriggerConfigDto. AlertType '{discriminatorValue}' is not valid.")
            };
        }
    }
}
