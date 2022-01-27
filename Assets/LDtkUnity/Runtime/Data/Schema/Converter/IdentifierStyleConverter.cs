using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class IdentifierStyleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IdentifierStyle) || t == typeof(IdentifierStyle?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Capitalize":
                    return IdentifierStyle.Capitalize;
                case "Free":
                    return IdentifierStyle.Free;
                case "Lowercase":
                    return IdentifierStyle.Lowercase;
                case "Uppercase":
                    return IdentifierStyle.Uppercase;
            }
            throw new Exception("Cannot unmarshal type IdentifierStyle");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (IdentifierStyle)untypedValue;
            switch (value)
            {
                case IdentifierStyle.Capitalize:
                    serializer.Serialize(writer, "Capitalize");
                    return;
                case IdentifierStyle.Free:
                    serializer.Serialize(writer, "Free");
                    return;
                case IdentifierStyle.Lowercase:
                    serializer.Serialize(writer, "Lowercase");
                    return;
                case IdentifierStyle.Uppercase:
                    serializer.Serialize(writer, "Uppercase");
                    return;
            }
            throw new Exception("Cannot marshal type IdentifierStyle");
        }

        public static readonly IdentifierStyleConverter Singleton = new IdentifierStyleConverter();
    }
}