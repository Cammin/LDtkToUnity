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
                AllowedRefsConverter.Singleton,
                EditorDisplayModeConverter.Singleton,
                EditorDisplayPosConverter.Singleton,
                TextLanguageModeConverter.Singleton,
                LevelFieldTypeConverter.Singleton,
                LimitBehaviorConverter.Singleton,
                LimitScopeConverter.Singleton,
                RenderModeConverter.Singleton,
                TileRenderModeConverter.Singleton,
                CheckerConverter.Singleton,
                TileModeConverter.Singleton,
                LayerTypeConverter.Singleton,
                EmbedAtlasConverter.Singleton,
                FlagConverter.Singleton,
                IdentifierStyleConverter.Singleton,
                ImageExportModeConverter.Singleton,
                BgPosConverter.Singleton,
                WorldLayoutConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}