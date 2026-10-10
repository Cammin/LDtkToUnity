using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        [MenuItem("LDtkUnity/Reserialize/Selection")]
        private static void ReserializeSelected()
        {
            HashSet<string> pathsToProcess = new HashSet<string>();

            foreach (var obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue;

                if (Directory.Exists(path))
                {
                    pathsToProcess.UnionWith(GetPathsInFolderRecursive(path));
                }
                else
                {
                    pathsToProcess.Add(path);
                }
            }

            //make exceptions!
            //to System.Threading.Tasks.Extensions.dll.meta and Utf8Json.dll.meta
            pathsToProcess.ExceptWith(new[] { "Assets/System.Threading.Tasks.Extensions.dll.meta", "Assets/Utf8Json.dll.meta" });
            
            if (pathsToProcess.Count > 0)
            {
                AssetDatabase.ForceReserializeAssets(pathsToProcess);
                Debug.Log($"Reserialized {pathsToProcess.Count} assets");
            }
        }

        private static List<string> GetPathsInFolderRecursive(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return new List<string>();
            }
            
            return AssetDatabase.FindAssets("", new[] { path })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(p => !Directory.Exists(p))
                .ToList();
        }
    }
}
