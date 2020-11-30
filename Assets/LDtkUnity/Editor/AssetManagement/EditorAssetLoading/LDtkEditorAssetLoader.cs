using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.EditorAssetLoading
{
    public static class LDtkEditorAssetLoader
    {
        private const string ASSETS = "Assets/";
        private const string PACKAGES = "Packages/com.cammin.ldtkunity/";

        
        
        public static T Load<T>(string relativePath) where T : Object
        {
            return Load<T>(relativePath, out string fullPath);
        }
        public static T Load<T>(string relativePath, out string fullPath) where T : Object
        {
            fullPath = PACKAGES + relativePath;
            T template = AssetDatabase.LoadAssetAtPath<T>(fullPath);
            if (template != null) return template;
            
            fullPath = ASSETS + relativePath;
            template = AssetDatabase.LoadAssetAtPath<T>(fullPath);
            if (template != null) return template;

            Debug.LogError($"LDtk: Could not load the asset {typeof(T).Name} at path {fullPath}");
            return null;
        }
        

        public static bool AssetExists<T>(string relativePath) where T : Object
        {
            T template = AssetDatabase.LoadAssetAtPath<T>(PACKAGES + relativePath);
            if (template != null) return true;
            
            template = AssetDatabase.LoadAssetAtPath<T>(ASSETS + relativePath);
            if (template != null) return true;

            return false;
        }
    }
}