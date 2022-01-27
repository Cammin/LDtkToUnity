using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class FlagConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Flag) || t == typeof(Flag?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ExportPreCsvIntGridFormat":
                    return Flag.ExportPreCsvIntGridFormat;
                case "IgnoreBackupSuggest":
                    return Flag.IgnoreBackupSuggest;
                case "MultiWorlds":
                    return Flag.MultiWorlds;
                case "PrependIndexToLevelFileNames":
                    return Flag.PrependIndexToLevelFileNames;
            }
            throw new Exception("Cannot unmarshal type Flag");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Flag)untypedValue;
            switch (value)
            {
                case Flag.ExportPreCsvIntGridFormat:
                    serializer.Serialize(writer, "ExportPreCsvIntGridFormat");
                    return;
                case Flag.IgnoreBackupSuggest:
                    serializer.Serialize(writer, "IgnoreBackupSuggest");
                    return;
                case Flag.MultiWorlds:
                    serializer.Serialize(writer, "MultiWorlds");
                    return;
                case Flag.PrependIndexToLevelFileNames:
                    serializer.Serialize(writer, "PrependIndexToLevelFileNames");
                    return;
            }
            throw new Exception("Cannot marshal type Flag");
        }

        public static readonly FlagConverter Singleton = new FlagConverter();
    }
}