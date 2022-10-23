using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class FlagConverter : JsonConverter<Flag>
    {
        public override bool CanConvert(Type t) => t == typeof(Flag);

        public override Flag Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            switch (value)
            {
                case "DiscardPreCsvIntGrid":
                    return Flag.DiscardPreCsvIntGrid;
                case "ExportPreCsvIntGridFormat":
                    return Flag.ExportPreCsvIntGridFormat;
                case "IgnoreBackupSuggest":
                    return Flag.IgnoreBackupSuggest;
                case "MultiWorlds":
                    return Flag.MultiWorlds;
                case "PrependIndexToLevelFileNames":
                    return Flag.PrependIndexToLevelFileNames;
                case "UseMultilinesType":
                    return Flag.UseMultilinesType;
            }
            throw new Exception("Cannot unmarshal type Flag");
        }

        public override void Write(Utf8JsonWriter writer, Flag value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case Flag.DiscardPreCsvIntGrid:
                    JsonSerializer.Serialize(writer, "DiscardPreCsvIntGrid");
                    return;
                case Flag.ExportPreCsvIntGridFormat:
                    JsonSerializer.Serialize(writer, "ExportPreCsvIntGridFormat");
                    return;
                case Flag.IgnoreBackupSuggest:
                    JsonSerializer.Serialize(writer, "IgnoreBackupSuggest");
                    return;
                case Flag.MultiWorlds:
                    JsonSerializer.Serialize(writer, "MultiWorlds");
                    return;
                case Flag.PrependIndexToLevelFileNames:
                    JsonSerializer.Serialize(writer, "PrependIndexToLevelFileNames");
                    return;
                case Flag.UseMultilinesType:
                    JsonSerializer.Serialize(writer, "UseMultilinesType");
                    return;
            }
            throw new Exception("Cannot marshal type Flag");
        }

        public static readonly FlagConverter Singleton = new FlagConverter();
    }
}