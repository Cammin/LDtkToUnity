using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkInternalUtility
    {
        public const string ASSETS = "Assets/LDtkUnity";
        public const string PACKAGES = "Packages/com.cammin.ldtkunity";

        /// <summary>
        /// Loading in this way so that editor files can be loaded correctly no matter the path in development or in the packages folder
        /// </summary>
        public static T Load<T>(string pathFromRoot) where T : Object
        {
            //release package path
            T packagesLoad = TryLoad(PACKAGES);
            if (packagesLoad)
            {
                return packagesLoad;
            }
            
            //development environment path
            T assetsLoad = TryLoad(ASSETS);
            if (assetsLoad)
            {
                return assetsLoad;
            }

            T TryLoad(string start)
            {
                string fullPath = Path.Combine(start, pathFromRoot);
                return AssetDatabase.LoadAssetAtPath<T>(fullPath);
            }

            LDtkDebug.LogError($"Could not load the asset {typeof(T).Name} at path {ASSETS + pathFromRoot} or {PACKAGES + pathFromRoot}");
            return null;
        }

        public static bool Exists(string pathFromRoot)
        {
            return ExistsInPackages(pathFromRoot) || ExistsInAssets(pathFromRoot);
        }

        private static bool ExistsInAssets(string pathFromRoot)
        {
            return File.Exists(ASSETS + pathFromRoot);
        }

        private static bool ExistsInPackages(string pathFromRoot)
        {
            return File.Exists(PACKAGES + pathFromRoot);
        }
    }
}