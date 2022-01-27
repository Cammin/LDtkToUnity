using System;
using Newtonsoft.Json;

namespace LDtkUnity
{
    internal class TextLanguageModeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TextLanguageMode) || t == typeof(TextLanguageMode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "LangC":
                    return TextLanguageMode.LangC;
                case "LangHaxe":
                    return TextLanguageMode.LangHaxe;
                case "LangJS":
                    return TextLanguageMode.LangJs;
                case "LangJson":
                    return TextLanguageMode.LangJson;
                case "LangLua":
                    return TextLanguageMode.LangLua;
                case "LangMarkdown":
                    return TextLanguageMode.LangMarkdown;
                case "LangPython":
                    return TextLanguageMode.LangPython;
                case "LangRuby":
                    return TextLanguageMode.LangRuby;
                case "LangXml":
                    return TextLanguageMode.LangXml;
            }
            throw new Exception("Cannot unmarshal type TextLanguageMode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TextLanguageMode)untypedValue;
            switch (value)
            {
                case TextLanguageMode.LangC:
                    serializer.Serialize(writer, "LangC");
                    return;
                case TextLanguageMode.LangHaxe:
                    serializer.Serialize(writer, "LangHaxe");
                    return;
                case TextLanguageMode.LangJs:
                    serializer.Serialize(writer, "LangJS");
                    return;
                case TextLanguageMode.LangJson:
                    serializer.Serialize(writer, "LangJson");
                    return;
                case TextLanguageMode.LangLua:
                    serializer.Serialize(writer, "LangLua");
                    return;
                case TextLanguageMode.LangMarkdown:
                    serializer.Serialize(writer, "LangMarkdown");
                    return;
                case TextLanguageMode.LangPython:
                    serializer.Serialize(writer, "LangPython");
                    return;
                case TextLanguageMode.LangRuby:
                    serializer.Serialize(writer, "LangRuby");
                    return;
                case TextLanguageMode.LangXml:
                    serializer.Serialize(writer, "LangXml");
                    return;
            }
            throw new Exception("Cannot marshal type TextLanguageMode");
        }

        public static readonly TextLanguageModeConverter Singleton = new TextLanguageModeConverter();
    }
}