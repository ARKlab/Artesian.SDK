using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.DerivedCfg.Serialize
{
    internal abstract class JsonPolymorphicConverter<TBase, TDiscriminatorEnum> : JsonConverter<TBase> where TDiscriminatorEnum : struct, Enum
    {
        private readonly string _discriminatorPropertyName;

        protected JsonPolymorphicConverter(string discriminatorPropertyName)
        {
            _discriminatorPropertyName = discriminatorPropertyName;
        }

        protected abstract Type GetType(TDiscriminatorEnum discriminatorValue);

        public override TBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            string propertyName = options.PropertyNamingPolicy?.ConvertName(_discriminatorPropertyName) ?? _discriminatorPropertyName;
            if (!jsonDocument.RootElement.TryGetProperty(propertyName, out var value))
            {
                throw new JsonException();
            }

            Type type = null;
            TDiscriminatorEnum result;
            if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var value2) && Enum.IsDefined(typeof(TDiscriminatorEnum), value2))
            {
                type = GetType((TDiscriminatorEnum)Enum.ToObject(typeof(TDiscriminatorEnum), value2));
            }
            else if (value.ValueKind == JsonValueKind.String && Enum.TryParse<TDiscriminatorEnum>(value.GetString(), ignoreCase: true, out result))
            {
                type = GetType(result);
            }

            if (type == null)
            {
                throw new JsonException();
            }

            return (TBase)JsonSerializer.Deserialize(jsonDocument.RootElement.GetRawText(), type, options);
        }

        public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}