using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class BgPosConverter : JsonConverter<BgPos>
    {
        public override bool CanConvert(Type t) => t == typeof(BgPos);

        public override BgPos Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, BgPos value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case BgPos.Contain:
                    JsonSerializer.Serialize(writer, "Contain");
                    return;
                case BgPos.Cover:
                    JsonSerializer.Serialize(writer, "Cover");
                    return;
                case BgPos.CoverDirty:
                    JsonSerializer.Serialize(writer, "CoverDirty");
                    return;
                case BgPos.Unscaled:
                    JsonSerializer.Serialize(writer, "Unscaled");
                    return;
            }
            throw new Exception("Cannot marshal type BgPos");
        }

        public static readonly BgPosConverter Singleton = new BgPosConverter();
    }
}