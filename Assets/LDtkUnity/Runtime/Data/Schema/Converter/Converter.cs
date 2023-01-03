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
                CheckerConverter.Singleton,
                TileModeConverter.Singleton,
                WhenConverter.Singleton,
                AllowedRefsConverter.Singleton,
                EditorDisplayModeConverter.Singleton,
                EditorDisplayPosConverter.Singleton,
                EditorLinkStyleConverter.Singleton,
                TextLanguageModeConverter.Singleton,
                LimitBehaviorConverter.Singleton,
                LimitScopeConverter.Singleton,
                RenderModeConverter.Singleton,
                TileRenderModeConverter.Singleton,
                TypeEnumConverter.Singleton,
                EmbedAtlasConverter.Singleton,
                BgPosConverter.Singleton,
                WorldLayoutConverter.Singleton,
                FlagConverter.Singleton,
                IdentifierStyleConverter.Singleton,
                ImageExportModeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}