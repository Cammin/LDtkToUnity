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
                LimitBehaviorConverter.Singleton,
                RenderModeConverter.Singleton,
                TileRenderModeConverter.Singleton,
                TypeEnumConverter.Singleton,
                BgPosConverter.Singleton,
                WorldLayoutConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}