// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Custom JSON converter for dictionaries with complex keys (e.g., NodaTime types)
    /// Serializes dictionaries as arrays of {Key, Value} objects to handle non-string keys
    /// </summary>
    /// <typeparam name="TKey">The type of dictionary keys</typeparam>
    /// <typeparam name="TValue">The type of dictionary values</typeparam>
    internal sealed class DictionaryJsonConverterSTJ<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
        where TKey : notnull
    {
        public override Dictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected StartArray token");

            var dictionary = new Dictionary<TKey, TValue>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    return dictionary;

                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Expected StartObject token");

                TKey? key = default;
                TValue? value = default;
                bool hasKey = false;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        if (!hasKey)
                            throw new JsonException("Dictionary entry missing 'Key' property");
                        
                        // Key should not be null after deserialization, but check to be safe
                        if (key == null)
                            throw new JsonException("Dictionary key cannot be null");

                        dictionary.Add(key, value!);
                        break;
                    }

                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var propertyName = reader.GetString();
                        reader.Read(); // Move to the value

                        if (propertyName == "Key")
                        {
                            key = JsonSerializer.Deserialize<TKey>(ref reader, options);
                            hasKey = true;
                        }
                        else if (propertyName == "Value")
                        {
                            value = JsonSerializer.Deserialize<TValue>(ref reader, options);
                        }
                    }
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var kvp in value)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Key");
                JsonSerializer.Serialize(writer, kvp.Key, options);

                writer.WritePropertyName("Value");
                JsonSerializer.Serialize(writer, kvp.Value, options);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }

    /// <summary>
    /// Factory for creating dictionary converters
    /// </summary>
    internal sealed class DictionaryJsonConverterSTJFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            var genericTypeDef = typeToConvert.GetGenericTypeDefinition();
            return genericTypeDef == typeof(Dictionary<,>) || genericTypeDef == typeof(IDictionary<,>);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var keyType = typeToConvert.GetGenericArguments()[0];
            var valueType = typeToConvert.GetGenericArguments()[1];

            // Use custom converter for all dictionaries to maintain compatibility with existing JSON format
            // This ensures Key/Value array format is used consistently, which is required for complex keys like NodaTime types
            var converterType = typeof(DictionaryJsonConverterSTJ<,>).MakeGenericType(keyType, valueType);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }
    }
}
