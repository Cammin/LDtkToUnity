using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class EditorLinkStyleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(EditorLinkStyle) || t == typeof(EditorLinkStyle?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ArrowsLine":
                    return EditorLinkStyle.ArrowsLine;
                case "CurvedArrow":
                    return EditorLinkStyle.CurvedArrow;
                case "DashedLine":
                    return EditorLinkStyle.DashedLine;
                case "StraightArrow":
                    return EditorLinkStyle.StraightArrow;
                case "ZigZag":
                    return EditorLinkStyle.ZigZag;
            }
            throw new Exception("Cannot unmarshal type EditorLinkStyle");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (EditorLinkStyle)untypedValue;
            switch (value)
            {
                case EditorLinkStyle.ArrowsLine:
                    serializer.Serialize(writer, "ArrowsLine");
                    return;
                case EditorLinkStyle.CurvedArrow:
                    serializer.Serialize(writer, "CurvedArrow");
                    return;
                case EditorLinkStyle.DashedLine:
                    serializer.Serialize(writer, "DashedLine");
                    return;
                case EditorLinkStyle.StraightArrow:
                    serializer.Serialize(writer, "StraightArrow");
                    return;
                case EditorLinkStyle.ZigZag:
                    serializer.Serialize(writer, "ZigZag");
                    return;
            }
            throw new Exception("Cannot marshal type EditorLinkStyle");
        }

        public static readonly EditorLinkStyleConverter Singleton = new EditorLinkStyleConverter();
    }
}