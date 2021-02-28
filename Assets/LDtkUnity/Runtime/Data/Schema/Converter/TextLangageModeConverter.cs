using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class TextLangageModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TextLangageMode) || t == typeof(TextLangageMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "LangC":
                    return TextLangageMode.LangC;
                case "LangHaxe":
                    return TextLangageMode.LangHaxe;
                case "LangJS":
                    return TextLangageMode.LangJs;
                case "LangJson":
                    return TextLangageMode.LangJson;
                case "LangLua":
                    return TextLangageMode.LangLua;
                case "LangMarkdown":
                    return TextLangageMode.LangMarkdown;
                case "LangPython":
                    return TextLangageMode.LangPython;
                case "LangRuby":
                    return TextLangageMode.LangRuby;
                case "LangXml":
                    return TextLangageMode.LangXml;
            }
            throw new Exception("Cannot unmarshal type TextLangageMode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TextLangageMode)untypedValue;
            switch (value)
            {
                case TextLangageMode.LangC:
                    serializer.Serialize(writer, "LangC");
                    return;
                case TextLangageMode.LangHaxe:
                    serializer.Serialize(writer, "LangHaxe");
                    return;
                case TextLangageMode.LangJs:
                    serializer.Serialize(writer, "LangJS");
                    return;
                case TextLangageMode.LangJson:
                    serializer.Serialize(writer, "LangJson");
                    return;
                case TextLangageMode.LangLua:
                    serializer.Serialize(writer, "LangLua");
                    return;
                case TextLangageMode.LangMarkdown:
                    serializer.Serialize(writer, "LangMarkdown");
                    return;
                case TextLangageMode.LangPython:
                    serializer.Serialize(writer, "LangPython");
                    return;
                case TextLangageMode.LangRuby:
                    serializer.Serialize(writer, "LangRuby");
                    return;
                case TextLangageMode.LangXml:
                    serializer.Serialize(writer, "LangXml");
                    return;
            }
            throw new Exception("Cannot marshal type TextLangageMode");
        }

        public static readonly TextLangageModeConverter Singleton = new TextLangageModeConverter();
    }
}