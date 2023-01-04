using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class WhenConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(When) || t == typeof(When?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AfterLoad":
                    return When.AfterLoad;
                case "AfterSave":
                    return When.AfterSave;
                case "BeforeSave":
                    return When.BeforeSave;
                case "Manual":
                    return When.Manual;
            }
            throw new Exception("Cannot unmarshal type When");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (When)untypedValue;
            switch (value)
            {
                case When.AfterLoad:
                    serializer.Serialize(writer, "AfterLoad");
                    return;
                case When.AfterSave:
                    serializer.Serialize(writer, "AfterSave");
                    return;
                case When.BeforeSave:
                    serializer.Serialize(writer, "BeforeSave");
                    return;
                case When.Manual:
                    serializer.Serialize(writer, "Manual");
                    return;
            }
            throw new Exception("Cannot marshal type When");
        }

        public static readonly WhenConverter Singleton = new WhenConverter();
    }
}