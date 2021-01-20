using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class BgPosConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(BgPos) || t == typeof(BgPos?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Contain":
                    return BgPos.Contain;
                case "Cover":
                    return BgPos.Cover;
                case "CoverDirty":
                    return BgPos.CoverDirty;
                case "Unscaled":
                    return BgPos.Unscaled;
            }
            throw new Exception("Cannot unmarshal type BgPos");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (BgPos)untypedValue;
            switch (value)
            {
                case BgPos.Contain:
                    serializer.Serialize(writer, "Contain");
                    return;
                case BgPos.Cover:
                    serializer.Serialize(writer, "Cover");
                    return;
                case BgPos.CoverDirty:
                    serializer.Serialize(writer, "CoverDirty");
                    return;
                case BgPos.Unscaled:
                    serializer.Serialize(writer, "Unscaled");
                    return;
            }
            throw new Exception("Cannot marshal type BgPos");
        }

        public static readonly BgPosConverter Singleton = new BgPosConverter();
    }
}