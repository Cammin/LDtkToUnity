using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class TypeEnumConverter : JsonConverter<TypeEnum>
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum);

        public override TypeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, TypeEnum value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case TypeEnum.AutoLayer:
                    JsonSerializer.Serialize(writer, "AutoLayer");
                    return;
                case TypeEnum.Entities:
                    JsonSerializer.Serialize(writer, "Entities");
                    return;
                case TypeEnum.IntGrid:
                    JsonSerializer.Serialize(writer, "IntGrid");
                    return;
                case TypeEnum.Tiles:
                    JsonSerializer.Serialize(writer, "Tiles");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}