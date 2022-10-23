using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class CheckerConverter : JsonConverter<Checker>
    {
        public override bool CanConvert(Type t) => t == typeof(Checker);

        public override Checker Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "Horizontal":
                    return Checker.Horizontal;
                case "None":
                    return Checker.None;
                case "Vertical":
                    return Checker.Vertical;
            }
            throw new Exception("Cannot unmarshal type Checker");
        }

        public override void Write(Utf8JsonWriter writer, Checker value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Checker.Horizontal:
                    JsonSerializer.Serialize(writer, "Horizontal");
                    return;
                case Checker.None:
                    JsonSerializer.Serialize(writer, "None");
                    return;
                case Checker.Vertical:
                    JsonSerializer.Serialize(writer, "Vertical");
                    return;
            }
            throw new Exception("Cannot marshal type Checker");
        }

        public static readonly CheckerConverter Singleton = new CheckerConverter();
    }
}