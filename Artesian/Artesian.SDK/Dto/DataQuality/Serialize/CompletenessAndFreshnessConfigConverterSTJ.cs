using Artesian.SDK.Dto.Serialize;

using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// System.Text.Json polymorphic converter for <see cref="CompletenessAndFreshnessConfigDto"/>.
    /// Discriminates by the <see cref="MarketDataTypeV2"/> property to resolve the concrete subtype:
    /// <see cref="ActualCompletenessAndFreshnessConfigDto"/> for actual time series,
    /// <see cref="VersionedCompletenessAndFreshnessConfigDto"/> for versioned time series.
    /// </summary>
    sealed class CompletenessAndFreshnessConfigConverterSTJ : JsonPolymorphicConverter<CompletenessAndFreshnessConfigDto, MarketDataTypeV2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompletenessAndFreshnessConfigConverterSTJ"/> class.
        /// </summary>
        public CompletenessAndFreshnessConfigConverterSTJ()
            : base(nameof(CompletenessAndFreshnessConfigDto.MarketDataType))
        {
        }

        /// <summary>
        /// Resolves the concrete DTO type associated with the provided market data type discriminator.
        /// </summary>
        /// <param name="discriminatorValue">The market data type discriminator value.</param>
        /// <returns>The concrete type to deserialize.</returns>
        protected override Type GetType(MarketDataTypeV2 discriminatorValue)
        {
            return discriminatorValue switch
            {
                MarketDataTypeV2.ActualTimeSerie => typeof(ActualCompletenessAndFreshnessConfigDto),
                MarketDataTypeV2.VersionedTimeSerie => typeof(VersionedCompletenessAndFreshnessConfigDto),
                _ => throw new InvalidOperationException($"Can't deserialize CompletenessAndFreshnessConfigDto. MarketDataTypeV2 '{discriminatorValue}' is not valid.")
            };
        }
    }
}
