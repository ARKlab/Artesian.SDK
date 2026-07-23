using Artesian.SDK.Dto.DataQuality.Enums;
using Artesian.SDK.Dto.Serialize;

using System;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// System.Text.Json polymorphic converter for <see cref="DataQualityRuleConfigDto"/>.
    /// Discriminates by the <see cref="RuleType"/> property to resolve the concrete configuration subtype.
    /// For <see cref="RuleType.CompletenessAndFreshness"/>, secondary discrimination by MarketDataType is handled by
    /// <see cref="CompletenessAndFreshnessConfigConverterSTJ"/>.
    /// </summary>
    public class DataQualityRuleConfigConverterSTJ : JsonPolymorphicConverter<DataQualityRuleConfigDto, RuleType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataQualityRuleConfigConverterSTJ"/> class.
        /// </summary>
        public DataQualityRuleConfigConverterSTJ()
            : base(nameof(DataQualityRuleConfigDto.Type))
        {
        }

        /// <summary>
        /// Resolves the concrete DTO type associated with the provided rule type discriminator.
        /// </summary>
        /// <param name="discriminatorValue">The rule type discriminator value.</param>
        /// <returns>The concrete type to deserialize.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the discriminator is unsupported.</exception>
        protected override Type GetType(RuleType discriminatorValue)
        {
            return discriminatorValue switch
            {
                RuleType.CompletenessAndFreshness => typeof(CompletenessAndFreshnessConfigDto),
                RuleType.Outlier => typeof(OutlierConfigDto),
                _ => throw new InvalidOperationException($"Can't deserialize DataQualityRuleConfigDto. RuleType '{discriminatorValue}' is not valid.")
            };
        }
    }
}