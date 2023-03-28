// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Artesian.SDK.Service
{
    internal class DictionaryJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (IDictionary)value;

            writer.WriteStartArray();

            foreach (var key in dictionary.Keys)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Key");

                serializer.Serialize(writer, key);

                writer.WritePropertyName("Value");

                serializer.Serialize(writer, dictionary[key]);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!CanConvert(objectType))
                throw new NotSupportedException(string.Format("This converter is not for {0}.", objectType));

            var keyType = objectType.GetGenericArguments()[0];
            var valueType = objectType.GetGenericArguments()[1];
            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var result = (IDictionary)Activator.CreateInstance(dictionaryType);

            if (reader.TokenType == JsonToken.Null)
                return null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    return result;
                }

                if (reader.TokenType == JsonToken.StartObject)
                {
                    _addObjectToDictionary(reader, result, serializer, keyType, valueType);
                }
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(IDictionary<,>) || objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>));
        }

        private void _addObjectToDictionary(JsonReader reader, IDictionary result, JsonSerializer serializer, Type keyType, Type valueType)
        {
            object key = null;
            object value = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject && key != null)
                {
                    result.Add(key, value);
                    return;
                }

                var propertyName = reader.Value.ToString();
                if (propertyName == "Key")
                {
                    reader.Read();
                    key = serializer.Deserialize(reader, keyType);
                }
                else if (propertyName == "Value")
                {
                    reader.Read();
                    value = serializer.Deserialize(reader, valueType);
                }
            }
        }
    }

    internal static partial class DictionaryJsonConverterExtensions
    {
        public static JsonSerializerSettings ConfigureForDictionary(this JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            // Add our converters
            _addDefaultConverters(settings.Converters);

            // return to allow fluent chaining if desired
            return settings;
        }

        private static void _addDefaultConverters(IList<JsonConverter> converters)
        {
            if (!converters.OfType<DictionaryJsonConverter>().Any())
                converters.Add(new DictionaryJsonConverter());
        }

    }

}
