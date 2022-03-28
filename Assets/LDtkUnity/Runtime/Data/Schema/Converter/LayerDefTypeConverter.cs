using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class LayerDefTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LayerDefType) || t == typeof(LayerDefType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AutoLayer":
                    return LayerDefType.AutoLayer;
                case "Entities":
                    return LayerDefType.Entities;
                case "IntGrid":
                    return LayerDefType.IntGrid;
                case "Tiles":
                    return LayerDefType.Tiles;
            }
            throw new Exception("Cannot unmarshal type LayerDefType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LayerDefType)untypedValue;
            switch (value)
            {
                case LayerDefType.AutoLayer:
                    serializer.Serialize(writer, "AutoLayer");
                    return;
                case LayerDefType.Entities:
                    serializer.Serialize(writer, "Entities");
                    return;
                case LayerDefType.IntGrid:
                    serializer.Serialize(writer, "IntGrid");
                    return;
                case LayerDefType.Tiles:
                    serializer.Serialize(writer, "Tiles");
                    return;
            }
            throw new Exception("Cannot marshal type LayerDefType");
        }

        public static readonly LayerDefTypeConverter Singleton = new LayerDefTypeConverter();
    }
}