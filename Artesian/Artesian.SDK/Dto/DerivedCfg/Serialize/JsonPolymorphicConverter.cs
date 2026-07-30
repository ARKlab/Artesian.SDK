using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto.Serialize
{
    /// <summary>
    /// Provides a base System.Text.Json converter for polymorphic deserialization
    /// based on a discriminator enum property.
    /// </summary>
    /// <typeparam name="TBase">The base type handled by the converter.</typeparam>
    /// <typeparam name="TDiscriminatorEnum">The discriminator enum type used to resolve concrete types.</typeparam>
    public abstract class JsonPolymorphicConverter<TBase, TDiscriminatorEnum> : JsonConverter<TBase>
        where TDiscriminatorEnum : struct, Enum
    {
        private readonly string _discriminatorPropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPolymorphicConverter{TBase, TDiscriminatorEnum}"/> class.
        /// </summary>
        /// <param name="discriminatorPropertyName">The name of the discriminator property in JSON payloads.</param>
        protected JsonPolymorphicConverter(string discriminatorPropertyName)
        {
            _discriminatorPropertyName = discriminatorPropertyName;
        }

        /// <summary>
        /// Resolves the concrete type associated with the provided discriminator value.
        /// </summary>
        /// <param name="discriminatorValue">The discriminator enum value read from JSON.</param>
        /// <returns>The concrete type to use for deserialization.</returns>
        protected abstract Type GetType(TDiscriminatorEnum discriminatorValue);

        /// <summary>
        /// Reads and deserializes a polymorphic JSON object into the target base type.
        /// </summary>
        /// <param name="reader">The UTF-8 JSON reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The deserialized instance of <typeparamref name="TBase"/>, or <see langword="null"/>.</returns>
        public override TBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var pn = options.PropertyNamingPolicy?.ConvertName(_discriminatorPropertyName) ?? _discriminatorPropertyName;

                if (!jsonDocument.RootElement.TryGetProperty(pn, out var typeProperty))
                {
                    throw new JsonException();
                }

                Type? type = null;

                if (typeProperty.ValueKind == JsonValueKind.Number
                    && typeProperty.TryGetInt32(out var enumInt)
                    && Enum.IsDefined(typeof(TDiscriminatorEnum), enumInt))
                {
                    type = GetType((TDiscriminatorEnum)Enum.ToObject(typeof(TDiscriminatorEnum), enumInt));
                }
                else if (typeProperty.ValueKind == JsonValueKind.String
                    && Enum.TryParse<TDiscriminatorEnum>(typeProperty.GetString(), true, out var enumVal))
                {
                    type = GetType(enumVal);
                }

                if (type == null)
                {
                    throw new JsonException();
                }

                var jsonObject = jsonDocument.RootElement.GetRawText();
                var result = (TBase?)JsonSerializer.Deserialize(jsonObject, type, options);

                return result;
            }
        }

        /// <summary>
        /// Writes a polymorphic value to JSON.
        /// </summary>
        /// <param name="writer">The UTF-8 JSON writer.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object?)value, options);
        }
    }
}
