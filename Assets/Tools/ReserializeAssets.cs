using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class ReserializeAssets
    {
        [MenuItem("LDtkUnity/Reserialize/All", false, 10)]
        private static void ForceReserializeAssets()
        {
            AssetDatabase.ForceReserializeAssets();
        }
        [MenuItem("LDtkUnity/Reserialize/LDtkUnity", false, 10)]
        private static void ForceReserializeLDtkAssets()
        {
            ForceReserializeFolder("Assets/LDtkUnity");
        }
        [MenuItem("LDtkUnity/Reserialize/Samples", false, 10)]
        private static void ForceReserializeSamples()
        {
            ForceReserializeFolder("Assets/Samples");
        }

        private static void ForceReserializeFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("Folder path cannot be null or empty.");
                return;
            }

            // 1. Get all asset GUIDs in the folder and its subfolders
            string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });
            string[] paths = new string[guids.Length];

            // 2. Convert GUIDs to individual file paths
            for (int i = 0; i < guids.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            // 3. Force reserialize the asset paths
            AssetDatabase.ForceReserializeAssets(paths);
        
            Debug.Log($"Force-reserialized {paths.Length} assets in '{folderPath}'.");
        }
    }
}
