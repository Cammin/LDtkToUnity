using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [Serializable]
    public static class LDtkDependencyCache
    {
        private static string Key(string path) => $"{nameof(LDtkDependencyCache)}_{path}";
        public class DependencyRelocator : AssetModificationProcessor
        {
            private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
            {
                if (!IsFileValid(sourcePath))
                {
                    return AssetMoveResult.DidNotMove;
                }
                
                string srcKey = Key(sourcePath);
                if (EditorPrefs.HasKey(srcKey))
                {
                    string srcJson = EditorPrefs.GetString(srcKey);
                    EditorPrefs.DeleteKey(srcKey);

                    string destKey = Key(destinationPath);
                    EditorPrefs.SetString(destKey, srcJson);
                }

                return AssetMoveResult.DidNotMove;
            }

            private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
            {
                if (IsFileValid(assetPath))
                {
                    string srcKey = Key(assetPath);
                    if (EditorPrefs.HasKey(srcKey))
                    {
                        EditorPrefs.DeleteKey(srcKey);
                    }
                }

                return AssetDeleteResult.DidNotDelete;
            }

            private static bool IsFileValid(string assetPath)
            {
                string ext = Path.GetExtension(assetPath);
                return ext == ".ldtk" || ext == ".ldtkl";
            }
        }

        public static string[] Load(string path)
        {
            string key = Key(path);
            if (!EditorPrefs.HasKey(key))
            {
                return Array.Empty<string>();
            }
            
            string json = EditorPrefs.GetString(key, string.Empty);
            return JsonConvert.DeserializeObject<string[]>(json);
        }

        public static void Set(string path, string[] dependencies)
        {
            string key = Key(path);
            string json = JsonConvert.SerializeObject(dependencies);
            EditorPrefs.SetString(key, json);
        }
    }
}