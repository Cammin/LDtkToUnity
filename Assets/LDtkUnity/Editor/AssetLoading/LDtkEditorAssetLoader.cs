using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetLoading
{
    public static class LDtkEditorAssetLoader
    {
        private const string ASSETS = "Assets/";
        private const string PACKAGES = "Packages/com.cammin.ldtkunity/";

        public static bool IsValidFolder(string folderPath)
        {
            return AssetDatabase.IsValidFolder(ASSETS + folderPath);
        }
        public static void CreateDirectory(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }
        
        public static T Load<T>(string relativePath) where T : Object
        {
            T template = AssetDatabase.LoadAssetAtPath<T>(PACKAGES + relativePath);
            if (template != null) return template;
            
            template = AssetDatabase.LoadAssetAtPath<T>(ASSETS + relativePath);
            if (template != null) return template;

            Debug.LogError($"LDtk: Could not load the Editor asset {typeof(T).Name}");
            return null;
        }
    }
}