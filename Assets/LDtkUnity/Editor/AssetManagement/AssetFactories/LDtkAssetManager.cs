using System.IO;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories
{
    public static class LDtkAssetManager
    {
        public static T LoadLDtkAsset<T>(string folderPath, string identifier) where T : ScriptableObject, ILDtkAsset
        {
            string fullPath = $"{folderPath}/{identifier}.asset";
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        
        public static void SaveLDtkAsset<T>(T asset, string folderPath) where T : ScriptableObject, ILDtkAsset
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = $"{folderPath}/{asset.Identifier}.asset";

            if (AssetDatabase.LoadAssetAtPath<T>(fullPath))
            {
                Debug.Log($"Asset already exists: {asset.Identifier}");
                return;
            }
            
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


    }
}