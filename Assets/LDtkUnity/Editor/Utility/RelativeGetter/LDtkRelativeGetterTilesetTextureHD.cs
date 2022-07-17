using System.IO;
using UnityEngine;
using UnityEditor;

namespace LDtkUnity.Editor
{
    internal class LDtkRelativeGetterTilesetTextureHD : LDtkRelativeGetterTilesetTexture
    {
        
        public override Texture2D GetRelativeAsset(TilesetDefinition def, string relativeTo)
        {            
            return GetMainAsset<Texture2D>(GetRelPathHD(def, relativeTo));
        }

        public override T GetRelativeSubAsset<T>(TilesetDefinition def, string relativeTo)
        {
            var realPath = GetRelPathHD(def, relativeTo);
            return GetAsset<T>(realPath);
        }

        private T GetMainAsset<T>(string fullPath) where T : Object
        {
            //basic find
            Object loadedAsset = AssetDatabase.LoadMainAssetAtPath(fullPath);

            if (loadedAsset != null)
            {
                if (loadedAsset is T cast)
                {
                    return cast;
                }

                LDtkDebug.LogError($"An asset was successfully loaded but was not the right type {fullPath}");
                return null;
            }

            if (LOG)
            {
                LogFailIsAssetRelativeToAssetPathExists(fullPath);
            }
            return null;
        }

        private T GetAsset<T>(string fullPath) where T: Object
        {
            //basic find
            Object loadedAsset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (loadedAsset != null)
            {
                if (loadedAsset is T cast)
                {
                    return cast;
                }

                LDtkDebug.LogError($"An asset was successfully loaded but was not the right type {fullPath}");
                return null;
            }

            if (LOG)
            {
                Debug.Log($"Nao foi possivel caregar {fullPath}");
            }
            return null;
        }
        

        private string GetRelPathHD(TilesetDefinition definition, string relativeTo)
        {
            string relPath = relativeTo + "/" + GetRelPath(definition);

            if (relPath != null)
            {
                if (relPath.EndsWith("SD.png"))
                {
                    var hd = relPath.Replace("SD.png", "HD.png");
                    if (File.Exists(hd))
                    {
                        relPath = hd;
                    }
                }
            }
            
            return relPath;
        }
        
    }
}