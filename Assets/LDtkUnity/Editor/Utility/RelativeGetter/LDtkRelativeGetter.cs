using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkRelativeGetter<TData, TAsset> where TAsset : Object
    {
        protected abstract string GetRelPath(TData definition);

        public TAsset GetRelativeAsset(TData def, Object relativeTo)
        {
            if (!AssetDatabase.Contains(relativeTo))
            {
                Debug.LogError("LDtk: input object is not in the AssetDatabase");
                return null;
            }
            
            string assetPath = AssetDatabase.GetAssetPath(relativeTo);
            
            return GetRelativeAsset(def, assetPath);
        }
        public TAsset GetRelativeAsset(TData def, string assetPath)
        {
            return GetAssetRelativeToAssetPath<TAsset>(assetPath, GetRelPath(def));
        }

        private T GetAssetRelativeToAssetPath<T>(string assetPath, string relPath) where T : Object
        {
            string directory = Path.GetDirectoryName(assetPath);
            
            string assetsPath = $"{directory}/{relPath}";
            assetsPath = LDtkPathUtility.CleanPath(assetsPath);
            
            //basic find
            T assetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetsPath);
            if (assetAtPath != null)
            {
                return assetAtPath;
            }
            
            //if the asset is null, try an asset database refresh (the refresh costs time so try try only if it was unsuccessful)
            /*AssetDatabase.Refresh();
            assetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetsPath);
            if (assetAtPath != null)
            {
                return assetAtPath;
            }*/
            
            //try a reimport as it may fix it
            /*if (File.Exists(assetsPath))
            {
                AssetDatabase.ImportAsset(assetsPath);
                assetAtPath = AssetDatabase.LoadAssetAtPath<T>(assetsPath);
                if (assetAtPath != null)
                {
                    return assetAtPath;
                }
            }*/
            
            //if we couldn't load it but the file indeed exists, spit a different error
            if (File.Exists(assetsPath))
            {
                Debug.LogError($"LDtk: File exists but could not load an asset at \"{assetPath}\". " +
                               $"Is the asset imported correctly?");
                return null;
            }

            Debug.LogError($"LDtk: Could not find an asset in the path relative to \"{assetPath}\": \"{relPath}\". " +
                           $"Is the asset also locatable by LDtk, and is the asset located in the Unity Project?");
            return null;
        }
    }
}