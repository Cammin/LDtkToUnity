using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkAssetUtil
    {
        /// <summary>
        /// automatically make the directory if it doesnt exist,
        /// 
        /// //make sure to call these when done with making assets:
        ///AssetDatabase.SaveAssets();
        ///AssetDatabase.Refresh();
        /// </summary>
        public static void SaveAsset<T>(string directory, T asset, string extension = ".asset") where T : Object
        {
            LDtkPathUtil.CreateDirectoryIfNotValidFolder(directory);

            string fullPath = $"{directory}/{asset.name}{extension}";

            //If already exists, destroy old asset before we re-save a new one
            if (AssetDatabase.LoadAssetAtPath<T>(fullPath))
            {
                AssetDatabase.DeleteAsset(fullPath);
            }
            
            AssetDatabase.CreateAsset(asset, fullPath);
        }

        public static void WriteText(string path, string content)
        {
            using StreamWriter streamWriter = new StreamWriter(path);
            streamWriter.Write(content);
        }
    }
}