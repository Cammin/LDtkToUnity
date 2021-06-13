using System.IO;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkRelativeGetterTilesetTexture : LDtkRelativeGetter<TilesetDefinition, Texture2D>
    {
        protected override string GetRelPath(TilesetDefinition definition)
        {
            if (IsAsepriteAsset(definition.RelPath))
            {
                return null;
            }
            
            return definition.RelPath;
        }
        
        private bool IsAsepriteAsset(string path)
        {
            //if we hit an aseprite path instead.
            string ext = Path.GetExtension(path);
            if (ext == ".ase" || ext == ".aseprite")
            {
                string name = Path.GetFileName(path);
                Debug.LogError($"LDtk: Aseprite files not supported for Tilesets ({name})");
                return true;
            }

            return false;
        }
    }
}