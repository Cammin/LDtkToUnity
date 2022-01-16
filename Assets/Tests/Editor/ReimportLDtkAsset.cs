using System;
using LDtkUnity;
using LDtkUnity.Editor;
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
            TryImport(typeof(LDtkProjectFile));
            TryImport(typeof(LDtkLevelFile));

            void TryImport(Type type)
            {
                foreach (string guid in AssetDatabase.FindAssets($"t:{type.Name}"))
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (assetPath == null)
                    {
                        continue;
                    }

                    AssetDatabase.ImportAsset(assetPath);
                }
            }
        }
        
        [MenuItem("LDtkUnity/Export Native Prefab")]
        private static void CreateWindow()
        {
            LDtkNativeExportWindow.CreateWindowWithContext(null);
        }
    }
}
