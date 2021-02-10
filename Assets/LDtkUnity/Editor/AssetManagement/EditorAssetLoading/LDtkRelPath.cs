using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkRelPath
    {
        public static Object GetAssetRelativeToAsset(Object asset, string relPath)
        {
            string projectFilePath = AssetDatabase.GetAssetPath(asset);
            projectFilePath = Path.GetFileNameWithoutExtension(projectFilePath);
            string assetsPath = $"{projectFilePath}\\{relPath}";
            Debug.Log($"Trying to get: {assetsPath}");

            Object relAsset = AssetDatabase.LoadAssetAtPath<Object>(assetsPath);
            if (relAsset != null)
            {
                Debug.Log($"Got it!");
                Debug.Log(relAsset.name);
                //Property
                return relAsset;
            }
            
            Debug.Log("Was null");
            return null;
        }
    }
}