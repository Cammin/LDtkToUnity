using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class EditorDisplayModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(EditorDisplayMode) || t == typeof(EditorDisplayMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "EntityTile":
                    return EditorDisplayMode.EntityTile;
                case "Hidden":
                    return EditorDisplayMode.Hidden;
                case "NameAndValue":
                    return EditorDisplayMode.NameAndValue;
                case "PointPath":
                    return EditorDisplayMode.PointPath;
                case "PointPathLoop":
                    return EditorDisplayMode.PointPathLoop;
                case "PointStar":
                    return EditorDisplayMode.PointStar;
                case "Points":
                    return EditorDisplayMode.Points;
                case "RadiusGrid":
                    return EditorDisplayMode.RadiusGrid;
                case "RadiusPx":
                    return EditorDisplayMode.RadiusPx;
                case "ValueOnly":
                    return EditorDisplayMode.ValueOnly;
            }
            throw new Exception("Cannot unmarshal type EditorDisplayMode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (EditorDisplayMode)untypedValue;
            switch (value)
            {
                case EditorDisplayMode.EntityTile:
                    serializer.Serialize(writer, "EntityTile");
                    return;
                case EditorDisplayMode.Hidden:
                    serializer.Serialize(writer, "Hidden");
                    return;
                case EditorDisplayMode.NameAndValue:
                    serializer.Serialize(writer, "NameAndValue");
                    return;
                case EditorDisplayMode.PointPath:
                    serializer.Serialize(writer, "PointPath");
                    return;
                case EditorDisplayMode.PointPathLoop:
                    serializer.Serialize(writer, "PointPathLoop");
                    return;
                case EditorDisplayMode.PointStar:
                    serializer.Serialize(writer, "PointStar");
                    return;
                case EditorDisplayMode.Points:
                    serializer.Serialize(writer, "Points");
                    return;
                case EditorDisplayMode.RadiusGrid:
                    serializer.Serialize(writer, "RadiusGrid");
                    return;
                case EditorDisplayMode.RadiusPx:
                    serializer.Serialize(writer, "RadiusPx");
                    return;
                case EditorDisplayMode.ValueOnly:
                    serializer.Serialize(writer, "ValueOnly");
                    return;
            }
            throw new Exception("Cannot marshal type EditorDisplayMode");
        }

        public static readonly EditorDisplayModeConverter Singleton = new EditorDisplayModeConverter();
    }
}