using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class EditorDisplayModeConverter : JsonConverter<EditorDisplayMode>
    {
        public override bool CanConvert(Type t) => t == typeof(EditorDisplayMode);

        public override EditorDisplayMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "ArrayCountNoLabel":
                    return EditorDisplayMode.ArrayCountNoLabel;
                case "ArrayCountWithLabel":
                    return EditorDisplayMode.ArrayCountWithLabel;
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
                case "RefLinkBetweenCenters":
                    return EditorDisplayMode.RefLinkBetweenCenters;
                case "RefLinkBetweenPivots":
                    return EditorDisplayMode.RefLinkBetweenPivots;
                case "ValueOnly":
                    return EditorDisplayMode.ValueOnly;
            }
            throw new Exception("Cannot unmarshal type EditorDisplayMode");
        }

        public override void Write(Utf8JsonWriter writer, EditorDisplayMode value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case EditorDisplayMode.ArrayCountNoLabel:
                    JsonSerializer.Serialize(writer, "ArrayCountNoLabel");
                    return;
                case EditorDisplayMode.ArrayCountWithLabel:
                    JsonSerializer.Serialize(writer, "ArrayCountWithLabel");
                    return;
                case EditorDisplayMode.EntityTile:
                    JsonSerializer.Serialize(writer, "EntityTile");
                    return;
                case EditorDisplayMode.Hidden:
                    JsonSerializer.Serialize(writer, "Hidden");
                    return;
                case EditorDisplayMode.NameAndValue:
                    JsonSerializer.Serialize(writer, "NameAndValue");
                    return;
                case EditorDisplayMode.PointPath:
                    JsonSerializer.Serialize(writer, "PointPath");
                    return;
                case EditorDisplayMode.PointPathLoop:
                    JsonSerializer.Serialize(writer, "PointPathLoop");
                    return;
                case EditorDisplayMode.PointStar:
                    JsonSerializer.Serialize(writer, "PointStar");
                    return;
                case EditorDisplayMode.Points:
                    JsonSerializer.Serialize(writer, "Points");
                    return;
                case EditorDisplayMode.RadiusGrid:
                    JsonSerializer.Serialize(writer, "RadiusGrid");
                    return;
                case EditorDisplayMode.RadiusPx:
                    JsonSerializer.Serialize(writer, "RadiusPx");
                    return;
                case EditorDisplayMode.RefLinkBetweenCenters:
                    JsonSerializer.Serialize(writer, "RefLinkBetweenCenters");
                    return;
                case EditorDisplayMode.RefLinkBetweenPivots:
                    JsonSerializer.Serialize(writer, "RefLinkBetweenPivots");
                    return;
                case EditorDisplayMode.ValueOnly:
                    JsonSerializer.Serialize(writer, "ValueOnly");
                    return;
            }
            throw new Exception("Cannot marshal type EditorDisplayMode");
        }

        public static readonly EditorDisplayModeConverter Singleton = new EditorDisplayModeConverter();
    }
}