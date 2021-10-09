using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class TestNativeExportWindow : EditorWindow
    {
        [SerializeField] private GameObject test;
        
        [MenuItem("LDtkUnity/OpenNativeExportWindow")]
        static void CreateWindow()
        {
            TestNativeExportWindow testNativeExportWindow = CreateInstance<TestNativeExportWindow>();
            testNativeExportWindow.Show();
        }

        private void OnGUI()
        {
            
            test = (GameObject)EditorGUILayout.ObjectField(test, typeof(GameObject), false);
            if (test == null)
            {
                return;
            }
            
            if (GUILayout.Button("Export"))
            {
                //this.
                new LDtkNativePrefabFactory().ExportNativePrefab(test);
            }
        }
    }
}