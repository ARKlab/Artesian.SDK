using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Artesian.SDK.Dto
{
    class DerivedCfgBaseConverter : JsonCreationConverter<DerivedCfgBase>
    {
        protected override DerivedCfgBase Create(Type objectType, JObject jObject)
        {

            if (jObject.TryGetValue(nameof(DerivedCfgBase.DerivedAlgorithm), StringComparison.InvariantCultureIgnoreCase, out var token))
            {
                if (token.ToObject<DerivedAlgorithm>() == DerivedAlgorithm.MUV)
                {
                    return new DerivedCfgMuv();
                }
                else if (token.ToObject<DerivedAlgorithm>() == DerivedAlgorithm.Sum)
                {
                    throw new NotImplementedException($@"Not yet impletemented DerivedAlgorithm {DerivedAlgorithm.Sum}");
                }
                else if (token.ToObject<DerivedAlgorithm>() == DerivedAlgorithm.Coalesce)
                {
                    return new DerivedCfgCoalesce();
                }

                throw new InvalidOperationException("Can't deserialize DerivedCfgBase. DerivedAlgorithm field not valid.");
            }

            throw new InvalidOperationException("Can't deserialize DerivedCfgBase. DerivedAlgorithm field not found.");
        }
    }

    /// <summary>
    /// JsonCreationConverter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        /// <summary>
        /// Can convert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Can write
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read Json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
