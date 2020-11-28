using System.IO;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories
{
    public static class LDtkAssetSaver
    {
        public static void SaveAsset<T>(T asset) where T : ScriptableObject, ILDtkAsset
        {
            string meshRootDirectory = $"Assets/Resources/LDtkProject";
            if (Directory.Exists(meshRootDirectory) == false)
            {
                Directory.CreateDirectory(meshRootDirectory);
            }

            string meshFilePath = $"{meshRootDirectory}/{asset.Identifier}.asset";
            
            AssetDatabase.CreateAsset(asset, meshFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}