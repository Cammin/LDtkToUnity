using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class TileModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TileMode) || t == typeof(TileMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Single":
                    return TileMode.Single;
                case "Stamp":
                    return TileMode.Stamp;
            }
            throw new Exception("Cannot unmarshal type TileMode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TileMode)untypedValue;
            switch (value)
            {
                case TileMode.Single:
                    serializer.Serialize(writer, "Single");
                    return;
                case TileMode.Stamp:
                    serializer.Serialize(writer, "Stamp");
                    return;
            }
            throw new Exception("Cannot marshal type TileMode");
        }

        public static readonly TileModeConverter Singleton = new TileModeConverter();
    }
}