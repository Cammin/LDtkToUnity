using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class ImageExportModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ImageExportMode) || t == typeof(ImageExportMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
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

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ImageExportMode)untypedValue;
            switch (value)
            {
                case ImageExportMode.LayersAndLevels:
                    serializer.Serialize(writer, "LayersAndLevels");
                    return;
                case ImageExportMode.None:
                    serializer.Serialize(writer, "None");
                    return;
                case ImageExportMode.OneImagePerLayer:
                    serializer.Serialize(writer, "OneImagePerLayer");
                    return;
                case ImageExportMode.OneImagePerLevel:
                    serializer.Serialize(writer, "OneImagePerLevel");
                    return;
            }
            throw new Exception("Cannot marshal type ImageExportMode");
        }

        public static readonly ImageExportModeConverter Singleton = new ImageExportModeConverter();
    }
}