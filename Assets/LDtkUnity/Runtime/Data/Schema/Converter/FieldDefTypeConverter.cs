using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class FieldDefTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FieldDefType) || t == typeof(FieldDefType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            //HACK
            string[] strings = value.Split('(');
            value = strings[0];
            switch (value)
            {
                case "F_Bool":
                    return FieldDefType.FBool;
                case "F_Color":
                    return FieldDefType.FColor;
                case "F_EntityRef":
                    return FieldDefType.FEntityRef;
                case "F_Enum":
                    return FieldDefType.FEnum;
                case "F_Float":
                    return FieldDefType.FFloat;
                case "F_Int":
                    return FieldDefType.FInt;
                case "F_Path":
                    return FieldDefType.FPath;
                case "F_Point":
                    return FieldDefType.FPoint;
                case "F_String":
                    return FieldDefType.FString;
                case "F_Text":
                    return FieldDefType.FText;
                case "F_Tile":
                    return FieldDefType.FTile;
            }
            throw new Exception("Cannot unmarshal type FieldDefType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FieldDefType)untypedValue;
            switch (value)
            {
                case FieldDefType.FBool:
                    serializer.Serialize(writer, "F_Bool");
                    return;
                case FieldDefType.FColor:
                    serializer.Serialize(writer, "F_Color");
                    return;
                case FieldDefType.FEntityRef:
                    serializer.Serialize(writer, "F_EntityRef");
                    return;
                case FieldDefType.FEnum:
                    serializer.Serialize(writer, "F_Enum");
                    return;
                case FieldDefType.FFloat:
                    serializer.Serialize(writer, "F_Float");
                    return;
                case FieldDefType.FInt:
                    serializer.Serialize(writer, "F_Int");
                    return;
                case FieldDefType.FPath:
                    serializer.Serialize(writer, "F_Path");
                    return;
                case FieldDefType.FPoint:
                    serializer.Serialize(writer, "F_Point");
                    return;
                case FieldDefType.FString:
                    serializer.Serialize(writer, "F_String");
                    return;
                case FieldDefType.FText:
                    serializer.Serialize(writer, "F_Text");
                    return;
                case FieldDefType.FTile:
                    serializer.Serialize(writer, "F_Tile");
                    return;
            }
            throw new Exception("Cannot marshal type FieldDefType");
        }

        public static readonly FieldDefTypeConverter Singleton = new FieldDefTypeConverter();
    }
}