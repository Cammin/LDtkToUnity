using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkRelativeGetterTilesetTexture : LDtkRelativeGetter<TilesetDefinition, Texture2D>
    {
        protected override string GetRelPath(TilesetDefinition definition)
        {
            return definition.RelPath;
        }
    }
}