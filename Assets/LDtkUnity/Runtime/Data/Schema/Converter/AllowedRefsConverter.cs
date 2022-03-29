using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class AllowedRefsConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AllowedRefs) || t == typeof(AllowedRefs?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Any":
                    return AllowedRefs.Any;
                case "OnlySame":
                    return AllowedRefs.OnlySame;
                case "OnlyTags":
                    return AllowedRefs.OnlyTags;
            }
            throw new Exception("Cannot unmarshal type AllowedRefs");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AllowedRefs)untypedValue;
            switch (value)
            {
                case AllowedRefs.Any:
                    serializer.Serialize(writer, "Any");
                    return;
                case AllowedRefs.OnlySame:
                    serializer.Serialize(writer, "OnlySame");
                    return;
                case AllowedRefs.OnlyTags:
                    serializer.Serialize(writer, "OnlyTags");
                    return;
            }
            throw new Exception("Cannot marshal type AllowedRefs");
        }

        public static readonly AllowedRefsConverter Singleton = new AllowedRefsConverter();
    }
}