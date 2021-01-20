using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class WorldLayoutConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(WorldLayout) || t == typeof(WorldLayout?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Free":
                    return WorldLayout.Free;
                case "GridVania":
                    return WorldLayout.GridVania;
                case "LinearHorizontal":
                    return WorldLayout.LinearHorizontal;
                case "LinearVertical":
                    return WorldLayout.LinearVertical;
            }
            throw new Exception("Cannot unmarshal type WorldLayout");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (WorldLayout)untypedValue;
            switch (value)
            {
                case WorldLayout.Free:
                    serializer.Serialize(writer, "Free");
                    return;
                case WorldLayout.GridVania:
                    serializer.Serialize(writer, "GridVania");
                    return;
                case WorldLayout.LinearHorizontal:
                    serializer.Serialize(writer, "LinearHorizontal");
                    return;
                case WorldLayout.LinearVertical:
                    serializer.Serialize(writer, "LinearVertical");
                    return;
            }
            throw new Exception("Cannot marshal type WorldLayout");
        }

        public static readonly WorldLayoutConverter Singleton = new WorldLayoutConverter();
    }
}