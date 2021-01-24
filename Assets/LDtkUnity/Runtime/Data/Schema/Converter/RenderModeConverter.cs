using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class RenderModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(RenderMode) || t == typeof(RenderMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Cross":
                    return RenderMode.Cross;
                case "Ellipse":
                    return RenderMode.Ellipse;
                case "Rectangle":
                    return RenderMode.Rectangle;
                case "Tile":
                    return RenderMode.Tile;
            }
            throw new Exception("Cannot unmarshal type RenderMode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (RenderMode)untypedValue;
            switch (value)
            {
                case RenderMode.Cross:
                    serializer.Serialize(writer, "Cross");
                    return;
                case RenderMode.Ellipse:
                    serializer.Serialize(writer, "Ellipse");
                    return;
                case RenderMode.Rectangle:
                    serializer.Serialize(writer, "Rectangle");
                    return;
                case RenderMode.Tile:
                    serializer.Serialize(writer, "Tile");
                    return;
            }
            throw new Exception("Cannot marshal type RenderMode");
        }

        public static readonly RenderModeConverter Singleton = new RenderModeConverter();
    }
}