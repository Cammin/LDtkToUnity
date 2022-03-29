using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class EmbedAtlasConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(EmbedAtlas) || t == typeof(EmbedAtlas?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "LdtkIcons")
            {
                return EmbedAtlas.LdtkIcons;
            }
            throw new Exception("Cannot unmarshal type EmbedAtlas");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (EmbedAtlas)untypedValue;
            if (value == EmbedAtlas.LdtkIcons)
            {
                serializer.Serialize(writer, "LdtkIcons");
                return;
            }
            throw new Exception("Cannot marshal type EmbedAtlas");
        }

        public static readonly EmbedAtlasConverter Singleton = new EmbedAtlasConverter();
    }
}