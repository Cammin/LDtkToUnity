using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LDtkUnity
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                EditorDisplayModeConverter.Singleton,
                EditorDisplayPosConverter.Singleton,
                TextLangageModeConverter.Singleton,
                LimitBehaviorConverter.Singleton,
                LimitScopeConverter.Singleton,
                RenderModeConverter.Singleton,
                TileRenderModeConverter.Singleton,
                CheckerConverter.Singleton,
                TileModeConverter.Singleton,
                TypeEnumConverter.Singleton,
                FlagConverter.Singleton,
                BgPosConverter.Singleton,
                WorldLayoutConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}