// ReSharper disable InconsistentNaming

using LDtkUnity.Providers;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.Data
{
    public static class LDtkDataEntityTileExtensions
    {
        public static TilesetDefinition Definition(this LDtkDataEntityTile data) => LDtkProviderUid.GetUidData<TilesetDefinition>(data.tilesetUid);
        public static Rect SourceRect(this LDtkDataEntityTile data) => data.srcRect.ToRect();
    }
}