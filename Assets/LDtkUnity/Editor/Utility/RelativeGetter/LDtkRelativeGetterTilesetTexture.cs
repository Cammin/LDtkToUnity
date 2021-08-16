using System.IO;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkRelativeGetterTilesetTexture : LDtkRelativeGetter<TilesetDefinition, Texture2D>
    {
        protected override string GetRelPath(TilesetDefinition definition)
        {
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
                Debug.LogWarning($"LDtk: Aseprite files not supported for Tilesets (yet) for \"{name}\"");
                return true;
            }

            return false;
        }
    }
}