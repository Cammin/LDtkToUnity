using System;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    internal class LevelFieldTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LevelFieldType) || t == typeof(LevelFieldType?);

        
        private class HackFixObj
        {
            
            [JsonProperty("id")]
            public string stringValue;

            [JsonProperty("params")] public long[] otherValue;
        }
        
        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            
            //todo this is a hack to fix the schema issue. wait until fix comes
            
            string value = default;
            if (reader.TokenType == JsonToken.String)
            {
                value = serializer.Deserialize<string>(reader);
            }
            else 
            if (reader.TokenType == JsonToken.StartObject)
            {
                HackFixObj hackFixObj = null;

                try
                {
                    hackFixObj = serializer.Deserialize<HackFixObj>(reader);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }

                value = hackFixObj.stringValue;
            }

            switch (value)
            {
                case "F_Bool":
                    return LevelFieldType.FBool;
                case "F_Color":
                    return LevelFieldType.FColor;
                case "F_EntityRef":
                    return LevelFieldType.FEntityRef;
                case "F_Enum":
                    return LevelFieldType.FEnum;
                case "F_Float":
                    return LevelFieldType.FFloat;
                case "F_Int":
                    return LevelFieldType.FInt;
                case "F_Path":
                    return LevelFieldType.FPath;
                case "F_Point":
                    return LevelFieldType.FPoint;
                case "F_String":
                    return LevelFieldType.FString;
                case "F_Text":
                    return LevelFieldType.FText;
                case "F_Tile":
                    return LevelFieldType.FTile;
            }

            Debug.LogError(value);
            return default;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LevelFieldType)untypedValue;
            switch (value)
            {
                case LevelFieldType.FBool:
                    serializer.Serialize(writer, "F_Bool");
                    return;
                case LevelFieldType.FColor:
                    serializer.Serialize(writer, "F_Color");
                    return;
                case LevelFieldType.FEntityRef:
                    serializer.Serialize(writer, "F_EntityRef");
                    return;
                case LevelFieldType.FEnum:
                    serializer.Serialize(writer, "F_Enum");
                    return;
                case LevelFieldType.FFloat:
                    serializer.Serialize(writer, "F_Float");
                    return;
                case LevelFieldType.FInt:
                    serializer.Serialize(writer, "F_Int");
                    return;
                case LevelFieldType.FPath:
                    serializer.Serialize(writer, "F_Path");
                    return;
                case LevelFieldType.FPoint:
                    serializer.Serialize(writer, "F_Point");
                    return;
                case LevelFieldType.FString:
                    serializer.Serialize(writer, "F_String");
                    return;
                case LevelFieldType.FText:
                    serializer.Serialize(writer, "F_Text");
                    return;
                case LevelFieldType.FTile:
                    serializer.Serialize(writer, "F_Tile");
                    return;
            }
            throw new Exception("Cannot marshal type LevelFieldType");
        }

        public static readonly LevelFieldTypeConverter Singleton = new LevelFieldTypeConverter();
    }
}