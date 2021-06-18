using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkInternalUtility
    {
        private const string ASSETS = "Assets/LDtkUnity/";
        private const string PACKAGES = "Packages/com.cammin.ldtkunity/";
        
        public static T Load<T>(string pathFromRoot) where T : Object
        {
            //release package path
            if (ExistsInPackages(pathFromRoot))
            {
                string fullPath = PACKAGES + pathFromRoot;
                T template = AssetDatabase.LoadAssetAtPath<T>(fullPath);
                if (template != null)
                {
                    return template;
                }
            }

            //development environment path
            if (ExistsInAssets(pathFromRoot))
            {
                string fullPath = ASSETS + pathFromRoot;
                T template = AssetDatabase.LoadAssetAtPath<T>(fullPath);
                if (template != null)
                {
                    return template;
                }
            }

            Debug.LogError($"LDtk: Could not load the asset {typeof(T).Name} at path {ASSETS + pathFromRoot} or {PACKAGES + pathFromRoot}");
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