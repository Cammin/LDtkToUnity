using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class LimitBehaviorConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LimitBehavior) || t == typeof(LimitBehavior?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "DiscardOldOnes":
                    return LimitBehavior.DiscardOldOnes;
                case "MoveLastOne":
                    return LimitBehavior.MoveLastOne;
                case "PreventAdding":
                    return LimitBehavior.PreventAdding;
            }
            throw new Exception("Cannot unmarshal type LimitBehavior");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LimitBehavior)untypedValue;
            switch (value)
            {
                case LimitBehavior.DiscardOldOnes:
                    serializer.Serialize(writer, "DiscardOldOnes");
                    return;
                case LimitBehavior.MoveLastOne:
                    serializer.Serialize(writer, "MoveLastOne");
                    return;
                case LimitBehavior.PreventAdding:
                    serializer.Serialize(writer, "PreventAdding");
                    return;
            }
            throw new Exception("Cannot marshal type LimitBehavior");
        }

        public static readonly LimitBehaviorConverter Singleton = new LimitBehaviorConverter();
    }
}