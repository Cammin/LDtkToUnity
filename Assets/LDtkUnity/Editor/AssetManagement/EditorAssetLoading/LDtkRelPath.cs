using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkRelPath
    {
        public static T GetAssetRelativeToAsset<T>(Object asset, string relPath) where T : Object
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string directoryName = Path.GetDirectoryName(assetPath);
            
            string assetsPath = $"{directoryName}/{relPath}";
            assetsPath = SimplifyPathWithDoubleDots(assetsPath);
            assetsPath = assetsPath.Replace("\\", "/");

            return AssetDatabase.LoadAssetAtPath<T>(assetsPath);
        }

        private static string SimplifyPathWithDoubleDots(string inputPath)
        {
            string fullPath = Path.GetFullPath(inputPath);
            return "Assets" + fullPath.Substring(Application.dataPath.Length);
        }
    }
}