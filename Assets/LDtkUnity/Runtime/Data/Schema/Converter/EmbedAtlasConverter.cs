using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class EmbedAtlasConverter : JsonConverter<EmbedAtlas>
    {
        public override bool CanConvert(Type t) => t == typeof(EmbedAtlas);

        public override EmbedAtlas Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == "LdtkIcons")
            {
                return EmbedAtlas.LdtkIcons;
            }
            throw new Exception("Cannot unmarshal type EmbedAtlas");
        }

        public override void Write(Utf8JsonWriter writer, EmbedAtlas value, JsonSerializerOptions options)
        {
            if (value == EmbedAtlas.LdtkIcons)
            {
                JsonSerializer.Serialize(writer, "LdtkIcons");
                return;
            }
            throw new Exception("Cannot marshal type EmbedAtlas");
        }

        public static readonly EmbedAtlasConverter Singleton = new EmbedAtlasConverter();
    }
}