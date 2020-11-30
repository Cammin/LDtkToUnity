using System.IO;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories
{
    public static class LDtkAssetManager
    {
        public static T LoadLDtkAsset<T>(string folderPath, string identifier) where T : Object
        {
            string fullPath = $"{folderPath}/{identifier}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        
        public static bool SaveAsset<T>(T asset, string folderPath) where T : Object
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = $"{folderPath}/{asset.name}.asset";

            if (AssetDatabase.LoadAssetAtPath<T>(fullPath))
            {
                Debug.Log($"Asset already exists: {asset.name}");
                return false;
            }
            
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
        }
    }
}