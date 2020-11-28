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
            T template = AssetDatabase.LoadAssetAtPath<T>(PACKAGES + relativePath);
            if (template != null) return template;
            
            template = AssetDatabase.LoadAssetAtPath<T>(ASSETS + relativePath);
            if (template != null) return template;

            Debug.LogError($"LDtk: Could not load the asset {typeof(T).Name}");
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