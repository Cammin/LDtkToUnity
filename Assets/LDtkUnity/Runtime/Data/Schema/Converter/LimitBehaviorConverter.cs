using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LDtkUnity
{
    internal class LimitBehaviorConverter : JsonConverter<LimitBehavior>
    {
        public override bool CanConvert(Type t) => t == typeof(LimitBehavior);

        public override LimitBehavior Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
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

        public override void Write(Utf8JsonWriter writer, LimitBehavior value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case LimitBehavior.DiscardOldOnes:
                    JsonSerializer.Serialize(writer, "DiscardOldOnes");
                    return;
                case LimitBehavior.MoveLastOne:
                    JsonSerializer.Serialize(writer, "MoveLastOne");
                    return;
                case LimitBehavior.PreventAdding:
                    JsonSerializer.Serialize(writer, "PreventAdding");
                    return;
            }
            throw new Exception("Cannot marshal type LimitBehavior");
        }

        public static readonly LimitBehaviorConverter Singleton = new LimitBehaviorConverter();
    }
}