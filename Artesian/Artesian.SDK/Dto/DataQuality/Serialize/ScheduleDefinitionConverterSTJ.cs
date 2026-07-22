using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Serialize;

using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// Converts <see cref="ScheduleDefinitionDto"/> polymorphic JSON payloads
    /// to their concrete schedule definition types based on <see cref="ScheduleDefinitionType"/>.
    /// </summary>
    sealed class ScheduleDefinitionConverterSTJ : JsonPolymorphicConverter<ScheduleDefinitionDto, ScheduleDefinitionType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleDefinitionConverterSTJ"/> class.
        /// </summary>
        public ScheduleDefinitionConverterSTJ()
            : base(nameof(ScheduleDefinitionDto.Type))
        {
        }

        /// <summary>
        /// Resolves the concrete DTO type associated with the provided schedule definition discriminator.
        /// </summary>
        /// <param name="discriminatorValue">The schedule definition discriminator value.</param>
        /// <returns>The concrete type to deserialize.</returns>
        protected override Type GetType(ScheduleDefinitionType discriminatorValue)
        {
            return discriminatorValue switch
            {
                ScheduleDefinitionType.Cron => typeof(CronScheduleDefinitionDto),
                _ => throw new InvalidOperationException($"Can't deserialize ScheduleDefinitionDto. ScheduleDefinitionType '{discriminatorValue}' is not valid.")
            };
        }
    }
}
