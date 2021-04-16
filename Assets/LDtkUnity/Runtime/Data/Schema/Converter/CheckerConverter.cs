using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class CheckerConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Checker) || t == typeof(Checker?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
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

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Checker)untypedValue;
            switch (value)
            {
                case Checker.Horizontal:
                    serializer.Serialize(writer, "Horizontal");
                    return;
                case Checker.None:
                    serializer.Serialize(writer, "None");
                    return;
                case Checker.Vertical:
                    serializer.Serialize(writer, "Vertical");
                    return;
            }
            throw new Exception("Cannot marshal type Checker");
        }

        public static readonly CheckerConverter Singleton = new CheckerConverter();
    }
}