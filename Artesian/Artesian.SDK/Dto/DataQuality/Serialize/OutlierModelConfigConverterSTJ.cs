using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Serialize;
using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// System.Text.Json polymorphic converter for <see cref="OutlierModelConfigDto"/>.
    /// Discriminates by the <see cref="OutlierModel"/> property to resolve the concrete subtype.
    /// </summary>
    public class OutlierModelConfigConverterSTJ : JsonPolymorphicConverter<OutlierModelConfigDto, OutlierModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutlierModelConfigConverterSTJ"/> class.
        /// </summary>
        public OutlierModelConfigConverterSTJ()
            : base(nameof(OutlierModelConfigDto.Model))
        {
        }

        /// <summary>
        /// Resolves the concrete DTO type associated with the provided market data type discriminator.
        /// </summary>
        /// <param name="discriminatorValue">The market data type discriminator value.</param>
        /// <returns>The concrete type to deserialize.</returns>
        protected override Type GetType(OutlierModel discriminatorValue)
        {
            return discriminatorValue switch
            {
                OutlierModel.AbsoluteBound => typeof(OutlierAbsoluteBoundConfigDto),
                OutlierModel.RefCurve => typeof(OutlierRefCurveConfigDto),
                _ => throw new InvalidOperationException($"Can't deserialize OutlierModelConfigDto. OutlierModel '{discriminatorValue}' is not valid.")
            };
        }
    }
}
