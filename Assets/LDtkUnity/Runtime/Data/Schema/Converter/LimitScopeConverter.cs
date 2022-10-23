using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class LimitScopeConverter : JsonConverter<LimitScope>
    {
        public override bool CanConvert(Type t) => t == typeof(LimitScope);

        public override LimitScope Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, LimitScope value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case LimitScope.PerLayer:
                    JsonSerializer.Serialize(writer, "PerLayer");
                    return;
                case LimitScope.PerLevel:
                    JsonSerializer.Serialize(writer, "PerLevel");
                    return;
                case LimitScope.PerWorld:
                    JsonSerializer.Serialize(writer, "PerWorld");
                    return;
            }
            throw new Exception("Cannot marshal type LimitScope");
        }

        public static readonly LimitScopeConverter Singleton = new LimitScopeConverter();
    }
}