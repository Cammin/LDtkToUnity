using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class EditorDisplayPosConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(EditorDisplayPos) || t == typeof(EditorDisplayPos?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Above":
                    return EditorDisplayPos.Above;
                case "Beneath":
                    return EditorDisplayPos.Beneath;
                case "Center":
                    return EditorDisplayPos.Center;
            }
            throw new Exception("Cannot unmarshal type EditorDisplayPos");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (EditorDisplayPos)untypedValue;
            switch (value)
            {
                case EditorDisplayPos.Above:
                    serializer.Serialize(writer, "Above");
                    return;
                case EditorDisplayPos.Beneath:
                    serializer.Serialize(writer, "Beneath");
                    return;
                case EditorDisplayPos.Center:
                    serializer.Serialize(writer, "Center");
                    return;
            }
            throw new Exception("Cannot marshal type EditorDisplayPos");
        }

        public static readonly EditorDisplayPosConverter Singleton = new EditorDisplayPosConverter();
    }
}