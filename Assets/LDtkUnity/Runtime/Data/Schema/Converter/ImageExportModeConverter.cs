using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class ImageExportModeConverter : JsonConverter<ImageExportMode>
    {
        public override bool CanConvert(Type t) => t == typeof(ImageExportMode);

        public override ImageExportMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "LayersAndLevels":
                    return ImageExportMode.LayersAndLevels;
                case "None":
                    return ImageExportMode.None;
                case "OneImagePerLayer":
                    return ImageExportMode.OneImagePerLayer;
                case "OneImagePerLevel":
                    return ImageExportMode.OneImagePerLevel;
            }
            throw new Exception("Cannot unmarshal type ImageExportMode");
        }

        public override void Write(Utf8JsonWriter writer, ImageExportMode value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case ImageExportMode.LayersAndLevels:
                    JsonSerializer.Serialize(writer, "LayersAndLevels");
                    return;
                case ImageExportMode.None:
                    JsonSerializer.Serialize(writer, "None");
                    return;
                case ImageExportMode.OneImagePerLayer:
                    JsonSerializer.Serialize(writer, "OneImagePerLayer");
                    return;
                case ImageExportMode.OneImagePerLevel:
                    JsonSerializer.Serialize(writer, "OneImagePerLevel");
                    return;
            }
            throw new Exception("Cannot marshal type ImageExportMode");
        }

        public static readonly ImageExportModeConverter Singleton = new ImageExportModeConverter();
    }
}