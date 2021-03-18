using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class LimitScopeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LimitScope) || t == typeof(LimitScope?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "PerLayer":
                    return LimitScope.PerLayer;
                case "PerLevel":
                    return LimitScope.PerLevel;
                case "PerWorld":
                    return LimitScope.PerWorld;
            }
            throw new Exception("Cannot unmarshal type LimitScope");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LimitScope)untypedValue;
            switch (value)
            {
                case LimitScope.PerLayer:
                    serializer.Serialize(writer, "PerLayer");
                    return;
                case LimitScope.PerLevel:
                    serializer.Serialize(writer, "PerLevel");
                    return;
                case LimitScope.PerWorld:
                    serializer.Serialize(writer, "PerWorld");
                    return;
            }
            throw new Exception("Cannot marshal type LimitScope");
        }

        public static readonly LimitScopeConverter Singleton = new LimitScopeConverter();
    }
}