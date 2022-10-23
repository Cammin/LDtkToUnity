using System.Text.Json;

namespace LDtkUnity
{
    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                CheckerConverter.Singleton,
                TileModeConverter.Singleton,
                AllowedRefsConverter.Singleton,
                EditorDisplayModeConverter.Singleton,
                EditorDisplayPosConverter.Singleton,
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
            },
        };
    }
}