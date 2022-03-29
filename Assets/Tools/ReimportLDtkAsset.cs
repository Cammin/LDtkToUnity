using System;
using LDtkUnity;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.Editor
{
    public static class TestReimportLDtkAsset
    {
        [MenuItem("LDtkUnity/Reimport From Scene &r", false, 10)]
        private static void ReimportAsset()
        {
            LDtkComponentProject instance = Object.FindObjectOfType<LDtkComponentProject>();
            if (!instance)
            {
                Debug.LogWarning("No Instance");
                return;
            }

            Object asset = PrefabUtility.GetCorrespondingObjectFromSource(instance);
            if (!asset)
            {
                Debug.LogWarning("No asset");
                return;
            }


            string path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("LDtkUnity/Reimport All %&r", false, 10)]
        private static void ReimportAll()
        {
            //projects, then levels.
            TryImport(typeof(DefaultAsset), "ldtk");
            TryImport(typeof(LDtkProjectFile));

            TryImport(typeof(DefaultAsset), "ldtkl");
            TryImport(typeof(LDtkLevelFile));

            void TryImport(Type type, string ext = null)
            {
                foreach (string guid in AssetDatabase.FindAssets($"t:{type.Name}"))
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (assetPath == null)
                    {
                        continue;
                    }

                    if (ext != null && !assetPath.EndsWith(ext))
                    {
                        continue;
                    }

                    AssetDatabase.ImportAsset(assetPath);
                }
            }
        }
    }
}
