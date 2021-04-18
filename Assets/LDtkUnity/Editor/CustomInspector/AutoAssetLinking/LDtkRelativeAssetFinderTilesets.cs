using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkRelativeAssetFinderTilesets : LDtkRelativeAssetFinder<TilesetDefinition, Texture2D>
    {
        protected override string GetRelPath(TilesetDefinition definition)
        {
            return definition.RelPath;
        }
    }
}