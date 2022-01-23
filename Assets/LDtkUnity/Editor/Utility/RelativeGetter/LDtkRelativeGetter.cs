using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkRelativeGetter<TData, TAsset> where TAsset : Object
    {
        protected virtual bool LOG { get; } = true;

        protected abstract string GetRelPath(TData definition);

        /*public TAsset GetRelativeAsset(TData def, Object relativeTo)
        {
            if (!AssetDatabase.Contains(relativeTo))
            {
                Debug.LogError("LDtk: input object is not in the AssetDatabase");
                return null;
            }
            
            string assetPath = AssetDatabase.GetAssetPath(relativeTo);
            
            return GetRelativeAsset(def, assetPath);
        }*/

        public TAsset GetRelativeAsset(TData def, string relativeTo)
        {
            return GetAssetRelativeToAssetPath<TAsset>(relativeTo, GetRelPath(def));
        }
        
        public string GetPath(TData def, string relativeTo)
        {
            return GetPathRelativeToPath(relativeTo, GetRelPath(def));
        }
        
        public string ReadRelativeText(TData def, string relativeTo)
        {
            string path = GetPathRelativeToPath(relativeTo, GetRelPath(def));
            return File.ReadAllText(path);
        }
        
        private string GetPathRelativeToPath(string assetPath, string relPath)
        {
            if (relPath == null)
            {
                return null;
            }
            
            string directory = Path.GetDirectoryName(assetPath);
            string assetsPath = $"{directory}/{relPath}";
            assetsPath = LDtkPathUtility.CleanPath(assetsPath);
            return assetsPath;
        }

        private T GetAssetRelativeToAssetPath<T>(string assetPath, string relPath) where T : Object
        {
            string fullPath = GetPathRelativeToPath(assetPath, relPath);
                
            //basic find
            T assetAtPath = (T)AssetDatabase.LoadMainAssetAtPath(fullPath);
            if (assetAtPath != null)
            {
                return assetAtPath;
            }

            if (LOG)
            {
                LogFailIsAssetRelativeToAssetPathExists(fullPath);
            }
            return null;
        }

        private static void LogFailIsAssetRelativeToAssetPathExists(string assetsPath)
        {
            //if it was an aseprite path
            if (string.IsNullOrEmpty(assetsPath))
            {
                return;
            }

            //if we couldn't load it but the file indeed exists, spit a different error
            if (!File.Exists($"{Application.dataPath}/{assetsPath}"))
            {
                Debug.LogError($"LDtk: File DOES exist but could not load the asset at \"{assetsPath}\". " +
                               $"Is the asset imported yet, or is the path invalid, or is the imported asset type correct?");
                return;
            }

            Debug.LogError($"LDtk: Could not find an asset in the path \"{assetsPath}\". " +
                           $"Is the asset also locatable by LDtk, and is the asset located in the Unity Project?");
        }
    }
}