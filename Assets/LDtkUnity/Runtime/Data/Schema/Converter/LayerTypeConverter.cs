using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class LayerTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LayerType) || t == typeof(LayerType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AutoLayer":
                    return LayerType.AutoLayer;
                case "Entities":
                    return LayerType.Entities;
                case "IntGrid":
                    return LayerType.IntGrid;
                case "Tiles":
                    return LayerType.Tiles;
            }
            throw new Exception("Cannot unmarshal type LayerType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LayerType)untypedValue;
            switch (value)
            {
                case LayerType.AutoLayer:
                    serializer.Serialize(writer, "AutoLayer");
                    return;
                case LayerType.Entities:
                    serializer.Serialize(writer, "Entities");
                    return;
                case LayerType.IntGrid:
                    serializer.Serialize(writer, "IntGrid");
                    return;
                case LayerType.Tiles:
                    serializer.Serialize(writer, "Tiles");
                    return;
            }
            throw new Exception("Cannot marshal type LayerType");
        }

        public static readonly LayerTypeConverter Singleton = new LayerTypeConverter();
    }
}