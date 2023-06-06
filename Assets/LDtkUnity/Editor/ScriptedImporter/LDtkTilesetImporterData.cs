using UnityEngine;
using Utf8Json;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// We're making this because it's harder to generate an asset and set it's importer's bonus metadata in the same pass. So we're writing our own text instead to provide that data
    /// </summary>
    internal sealed class LDtkTilesetImporterData
    {
        public int PixelsPerUnit;
        public RectInt[] Rects;
        public TilesetDefinition Def;

        public static LDtkTilesetImporterData FromJson(byte[] bytes)
        {
            return JsonSerializer.Deserialize<LDtkTilesetImporterData>(bytes);
        }
        public byte[] ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}