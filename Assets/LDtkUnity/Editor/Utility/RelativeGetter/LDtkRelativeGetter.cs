using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkRelativeGetter<TData, TAsset> where TAsset : Object
    {
        protected virtual bool LOG { get; } = true;
        protected abstract string GetRelPath(TData definition);

        public delegate TAsset LoadAction(string path);
        
        /// <summary>
        /// Will return null with no errors if the load failed unless for a special occasion
        /// </summary>
        public virtual TAsset GetRelativeAsset(TData def, string relativeTo, LoadAction loadAction = null)
        {
            return GetAssetRelativeToAssetPath(relativeTo, GetRelPath(def), loadAction);
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
        
        public string GetPathRelativeToPath(string assetPath, string relPath)
        {
            if (relPath == null)
            {
                return null;
            }
            
            string directory = Path.GetDirectoryName(assetPath);
            string assetsPath = directory + '/' + relPath;
            assetsPath = LDtkPathUtility.CleanPath(assetsPath);
            return assetsPath;
        }

        private TAsset GetAssetRelativeToAssetPath(string assetPath, string relPath, LoadAction loadAction)
        {
            Profiler.BeginSample("GetAssetRelativeToAssetPath");
            string fullPath = GetPathRelativeToPath(assetPath, relPath);
                
            //basic find
            Object loadedAsset;
            if (loadAction != null)
            {
                loadedAsset = loadAction.Invoke(fullPath);
            }
            else
            {
                loadedAsset = AssetDatabase.LoadMainAssetAtPath(fullPath);
            }

            if (loadedAsset != null)
            {
                if (loadedAsset is TAsset cast)
                {
                    Profiler.EndSample();
                    return cast;
                }
                    
                Profiler.EndSample();
                LDtkDebug.LogError($"An asset was successfully loaded but was not the right type \"{typeof(TAsset).Name}\" at \"{fullPath}\"");
                return null;
            }

            if (LOG)
            {
                LogFailIsAssetRelativeToAssetPathExists(fullPath);
            }
            Profiler.EndSample();
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
            string path = Path.Combine(Application.dataPath, assetsPath);
            if (File.Exists(path))
            {
                LDtkDebug.LogError($"File DOES exist but could not load the asset at \"{assetsPath}\". " +
                               $"Is the asset imported yet, or is the path invalid, or is the imported asset type correct, or is the asset located in the Unity Project?");
                return;
            }

            LDtkDebug.LogError($"Could not find an asset in the path \"{assetsPath}\". " +
                           $"Is the asset also locatable by LDtk?");
        }
    }
}