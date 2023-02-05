using System;
using System.IO;
using UnityEditor;
using Utf8Json;

namespace LDtkUnity.Editor
{
    [Serializable]
    internal static class LDtkDependencyCache
    {
        private static string Key(string path) => $"{nameof(LDtkDependencyCache)}_{path}";
        internal sealed class DependencyRelocator : UnityEditor.AssetModificationProcessor
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
            return JsonSerializer.Deserialize<string[]>(json);
        }

        public static void Set(string path, string[] dependencies)
        {
            string key = Key(path);
            byte[] serialize = JsonSerializer.Serialize(dependencies);
            string json = System.Text.Encoding.UTF8.GetString(serialize);
            EditorPrefs.SetString(key, json);
        }
    }
}