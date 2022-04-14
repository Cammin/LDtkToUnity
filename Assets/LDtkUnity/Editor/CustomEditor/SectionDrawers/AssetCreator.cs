using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class AssetCreator
    {
        public delegate T ObjectCreation<out T>();

        public static T CreateAssetButton<T>(Rect buttonRect, GUIContent content, string assetName, ObjectCreation<T> c) where T : Object
        {
            if (!GUI.Button(buttonRect, content, EditorStyles.miniButton))
            {
                return null;
            }
            
            return CreateAsset(assetName, c);
        }

        public static T CreateAsset<T>(string assetName, ObjectCreation<T> c) where T : Object
        {
            string selectedPath = GetSelectedPathOrFallback();
            string path = $"{selectedPath}{assetName}";
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            string uniqueFileName = Path.GetFileNameWithoutExtension(uniquePath);

            T obj = c.Invoke();
            obj.name = uniqueFileName;

            AssetDatabase.CreateAsset(obj, uniquePath);
            AssetDatabase.ImportAsset(uniquePath, ImportAssetOptions.Default);
            EditorGUIUtility.PingObject(obj);
            return obj;
        }

        private static string GetSelectedPathOrFallback()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path + "/";
        }
    }
}