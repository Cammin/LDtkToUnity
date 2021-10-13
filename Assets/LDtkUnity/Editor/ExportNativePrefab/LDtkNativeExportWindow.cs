using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkNativeExportWindow : EditorWindow
    {
        [SerializeField] private GameObject _project;
        [SerializeField] private string _exportPath;
        
        private SerializedObject _serializedObject;
        private SerializedProperty _projectProp;
        private SerializedProperty _pathProp;
        private PathDrawer _pathDrawer;
        private LDtkNativePrefabAssets _assetManager;

        public static void CreateWindowWithContext(GameObject ctx)
        {
            LDtkNativeExportWindow window = ConstructWindow();
            window._project = ctx;
            window.Show();
        }

        private static LDtkNativeExportWindow ConstructWindow()
        {
            LDtkNativeExportWindow window = CreateInstance<LDtkNativeExportWindow>();
            window.titleContent = new GUIContent()
            {
                text = "LDtk Export",
                image = LDtkIconUtility.GetUnityIcon("Prefab On")
            };
            window.minSize = new Vector2(250, 150);
            
            
            
            return window;
        }
        
        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _projectProp = _serializedObject.FindProperty(nameof(_project));
            _pathProp = _serializedObject.FindProperty(nameof(_exportPath));

            

            

        }

        private void OnGUI()
        {
            
            
            GUIContent content = new GUIContent("Export Path");

            GUIContent headerContent = new GUIContent()
            {
                text = "Export Native Prefab",
                tooltip = "If you feel like uninstalling the LDtkToUnity package but want to maintain the LDtk project you Unity, you can export a prefab and assets to a folder."
            };
            
            
            EditorGUILayout.LabelField(headerContent, EditorStyles.boldLabel);
            //EditorGUILayout.LabelField("", style);
            EditorGUILayout.PropertyField(_projectProp);
            
            

            GameObject obj = (GameObject)_projectProp.objectReferenceValue;
            if (obj == null || !AssetDatabase.Contains(obj))
            {
                return;
            }

            string pathToObject = AssetDatabase.GetAssetPath(obj);
            _pathDrawer = new PathDrawer(content, _pathProp, pathToObject, "The path to export the prefab and related assets to.");
            _pathDrawer.DrawFolderField();
            
            if (GUILayout.Button("Export"))
            {
                //first try creating dupe assets
                LDtkArtifactAssets artifactAssets = AssetDatabase.LoadAssetAtPath<LDtkArtifactAssets>(pathToObject);
                _assetManager = new LDtkNativePrefabAssets(artifactAssets, _exportPath);
                _assetManager.GenerateAssets();
                
                //then create prefab and replace all formaer prefab references with new dupes
                LDtkNativePrefabFactory prefabFactory = new LDtkNativePrefabFactory();
                GameObject nativePrefab = prefabFactory.ExportNativePrefab(obj);

                string prefabPath = _exportPath + nativePrefab.name + ".prefab";
                
                if (PrefabUtility.SaveAsPrefabAsset(nativePrefab, prefabPath))
                {
                    Debug.Log($"Exported prefab to {prefabPath}");
                    
                    //destroy the instance in the scene now that we made the prefab
                    Destroy(nativePrefab);
                }
                else
                {
                    Debug.LogError($"Failed to export prefab to {prefabPath}");
                }
            }
        }
    }
}