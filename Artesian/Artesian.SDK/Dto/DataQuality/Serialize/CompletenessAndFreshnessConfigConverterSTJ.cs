using Artesian.SDK.Dto.Serialize;

using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// System.Text.Json polymorphic converter for <see cref="CompletenessAndFreshnessConfigDto"/>.
    /// Discriminates by the <see cref="MarketDataType"/> property to resolve the concrete subtype:
    /// <see cref="ActualCompletenessAndFreshnessConfigDto"/> for actual time series,
    /// <see cref="VersionedCompletenessAndFreshnessConfigDto"/> for versioned time series.
    /// </summary>
    sealed class CompletenessAndFreshnessConfigConverterSTJ : JsonPolymorphicConverter<CompletenessAndFreshnessConfigDto, MarketDataType>
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
        protected override Type GetType(MarketDataType discriminatorValue)
        {
            return discriminatorValue switch
            {
                MarketDataType.ActualTimeSerie => typeof(ActualCompletenessAndFreshnessConfigDto),
                MarketDataType.VersionedTimeSerie => typeof(VersionedCompletenessAndFreshnessConfigDto),
                _ => throw new InvalidOperationException($"Can't deserialize CompletenessAndFreshnessConfigDto. MarketDataType '{discriminatorValue}' is not valid.")
            };
        }
    }
}
