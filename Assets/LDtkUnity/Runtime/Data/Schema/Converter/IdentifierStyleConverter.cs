using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class IdentifierStyleConverter : JsonConverter<IdentifierStyle>
    {
        public override bool CanConvert(Type t) => t == typeof(IdentifierStyle);

        public override IdentifierStyle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, IdentifierStyle value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case IdentifierStyle.Capitalize:
                    JsonSerializer.Serialize(writer, "Capitalize");
                    return;
                case IdentifierStyle.Free:
                    JsonSerializer.Serialize(writer, "Free");
                    return;
                case IdentifierStyle.Lowercase:
                    JsonSerializer.Serialize(writer, "Lowercase");
                    return;
                case IdentifierStyle.Uppercase:
                    JsonSerializer.Serialize(writer, "Uppercase");
                    return;
            }
            throw new Exception("Cannot marshal type IdentifierStyle");
        }

        public static readonly IdentifierStyleConverter Singleton = new IdentifierStyleConverter();
    }
}