using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AutoLayer":
                    return TypeEnum.AutoLayer;
                case "Entities":
                    return TypeEnum.Entities;
                case "IntGrid":
                    return TypeEnum.IntGrid;
                case "Tiles":
                    return TypeEnum.Tiles;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.AutoLayer:
                    serializer.Serialize(writer, "AutoLayer");
                    return;
                case TypeEnum.Entities:
                    serializer.Serialize(writer, "Entities");
                    return;
                case TypeEnum.IntGrid:
                    serializer.Serialize(writer, "IntGrid");
                    return;
                case TypeEnum.Tiles:
                    serializer.Serialize(writer, "Tiles");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}