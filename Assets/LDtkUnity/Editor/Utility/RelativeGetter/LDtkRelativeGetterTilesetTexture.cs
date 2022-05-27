using System.IO;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkRelativeGetterTilesetTexture : LDtkRelativeGetter<TilesetDefinition, Texture2D>
    {
        protected override string GetRelPath(TilesetDefinition definition)
        {
            if (definition.IsEmbedAtlas)
            {
                //is the internal icons, we don't load it.
                //todo eventually think about how we can make this work
                return null;
            }

            string relPath = definition.RelPath;
            if (IsAsepriteAsset(relPath))
            {
                return null;
            }
            
            return relPath;
        }
        
        private bool IsAsepriteAsset(string path)
        {
            //if we hit an aseprite path instead.
            string ext = Path.GetExtension(path);
            if (ext == ".ase" || ext == ".aseprite")
            {
                string name = Path.GetFileName(path);
                LDtkDebug.LogWarning($"Aseprite files not supported for Tilesets (yet) for \"{name}\"");
                return true;
            }

            return false;
        }
    }
}