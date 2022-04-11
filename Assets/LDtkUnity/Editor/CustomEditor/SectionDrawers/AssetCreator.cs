using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class AssetCreator
    {
        public delegate T ObjectCreation<out T>();

        public static T CreateAssetButton<T>(GUIContent content, string assetName, ObjectCreation<T> c) where T : Object
        {
            const float height = 2;
            
            Rect buttonRect = EditorGUILayout.GetControlRect(false, height);
            buttonRect.height = EditorGUIUtility.singleLineHeight;
            
            const float width = 45;
            buttonRect.x = buttonRect.xMax - width;
            buttonRect.width = width;
            
            if (!GUI.Button(buttonRect, content, EditorStyles.miniButton))
            {
                return null;
            }
            
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