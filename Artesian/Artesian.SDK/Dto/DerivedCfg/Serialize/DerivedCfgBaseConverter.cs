using Artesian.SDK.Dto.DerivedCfg.Enums;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace Artesian.SDK.Dto.DerivedCfg.Serialize
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

    class DerivedCfgBaseConverterSTJ : JsonPolymorphicConverter<DerivedCfgBase, DerivedAlgorithm>
    {
        public DerivedCfgBaseConverterSTJ()
            : base(nameof(DerivedCfgBase.DerivedAlgorithm))
        {
        }

        protected override Type GetType(DerivedAlgorithm discriminatorValue)
        {
            return discriminatorValue switch
            {
                DerivedAlgorithm.MUV => typeof(DerivedCfgMuv),
                DerivedAlgorithm.Coalesce => typeof(DerivedCfgCoalesce),
                _ => null,
            };
        }
    }

    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

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
