using System.IO;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkRelativeGetterTilesetTexture : LDtkRelativeGetter<TilesetDefinition, Texture2D>
    {
        public override Texture2D GetRelativeAsset(TilesetDefinition def, string relativeTo, LoadAction loadAction = null)
        {
            //What's cool about this is that if we only need visual icons in LDtk (no layers defined with using Internal icons), then no sprites are generated and instead are just the texture stored in the entity drawer component 
            if (def.IsEmbedAtlas)
            {
                Texture2D iconsTexture = LDtkProjectSettings.InternalIconsTexture;
                if (iconsTexture != null)
                {
                    //doing no load action for this one. should be fine 
                    return iconsTexture;
                }
                
                //LDtkDebug.LogWarning("The project uses the internal icons tileset but the texture is not assigned or found. Add it in Project Settings > LDtk To Unity.");
                return null;
            }
            
            return base.GetRelativeAsset(def, relativeTo, loadAction);
        }

        protected override string GetRelPath(TilesetDefinition definition)
        {
            if (definition.IsEmbedAtlas)
            {
                LDtkDebug.LogError("Getting RelPath for embedded atlas was unexpected.");
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