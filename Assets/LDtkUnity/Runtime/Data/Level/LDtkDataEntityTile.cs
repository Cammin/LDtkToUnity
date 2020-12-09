// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataEntityTileExtensions
    {
        public static LDtkDefinitionTileset Definition(this LDtkDataEntityTile data) => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(data.tilesetUid);
        public static Rect SourceRect(this LDtkDataEntityTile data) => data.srcRect.ToRect();
    }
}