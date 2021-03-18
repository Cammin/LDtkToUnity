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
                case "DiscardPreCsvIntGrid":
                    return Flag.DiscardPreCsvIntGrid;
                case "IgnoreBackupSuggest":
                    return Flag.IgnoreBackupSuggest;
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
                case Flag.DiscardPreCsvIntGrid:
                    serializer.Serialize(writer, "DiscardPreCsvIntGrid");
                    return;
                case Flag.IgnoreBackupSuggest:
                    serializer.Serialize(writer, "IgnoreBackupSuggest");
                    return;
            }
            throw new Exception("Cannot marshal type Flag");
        }

        public static readonly FlagConverter Singleton = new FlagConverter();
    }
}