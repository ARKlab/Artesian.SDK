using Artesian.SDK.Dto.DataQuality.Enums;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DataQuality.Serialize
{
    /// <summary>
    /// System.Text.Json polymorphic converter for <see cref="DataQualityRuleConfigDto"/>.
    /// Discriminates by the <see cref="RuleType"/> property. Outlier configurations use MarketDataType
    /// to select the Market Assessment or BidAsk subtype.
    /// </summary>
    public class DataQualityRuleConfigConverterSTJ : JsonConverter<DataQualityRuleConfigDto>
    {
        /// <inheritdoc />
        public override DataQualityRuleConfigDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;
            if (!_tryGetProperty(root, nameof(DataQualityRuleConfigDto.Type), options, out var ruleTypeElement))
            {
                throw new JsonException($"Required discriminator '{nameof(DataQualityRuleConfigDto.Type)}' is missing.");
            }

            var ruleType = JsonSerializer.Deserialize<RuleType>(ruleTypeElement.GetRawText(), options);
            var configurationType = ruleType switch
            {
                RuleType.CompletenessAndFreshness => typeof(CompletenessAndFreshnessConfigDto),
                RuleType.Outlier => _getOutlierType(root, options),
                _ => throw new JsonException($"Unsupported Data Quality rule type '{ruleType}'.")
            };

            return (DataQualityRuleConfigDto?)JsonSerializer.Deserialize(root.GetRawText(), configurationType, options);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, DataQualityRuleConfigDto value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        private static Type _getOutlierType(JsonElement root, JsonSerializerOptions options)
        {
            if (!_tryGetProperty(root, nameof(OutlierConfigDto.MarketDataType), options, out var marketDataTypeElement))
            {
                return typeof(OutlierConfigDto);
            }

            var marketDataType = JsonSerializer.Deserialize<MarketDataType>(marketDataTypeElement.GetRawText(), options);
            return marketDataType switch
            {
                MarketDataType.MarketAssessment => typeof(MasOutlierConfigDto),
                MarketDataType.BidAsk => typeof(BidAskOutlierConfigDto),
                MarketDataType.ActualTimeSerie or MarketDataType.VersionedTimeSerie => typeof(OutlierConfigDto),
                _ => throw new JsonException($"Unsupported MarketDataType '{marketDataType}' for Outlier.")
            };
        }

        private static bool _tryGetProperty(JsonElement element, string propertyName, JsonSerializerOptions options, out JsonElement property)
        {
            var jsonPropertyName = options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
            if (element.TryGetProperty(jsonPropertyName, out property))
            {
                return true;
            }

            if (options.PropertyNameCaseInsensitive)
            {
                foreach (var candidate in element.EnumerateObject())
                {
                    if (string.Equals(candidate.Name, jsonPropertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        property = candidate.Value;
                        return true;
                    }
                }
            }

            property = default;
            return false;
        }
    }
}