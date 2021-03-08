using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkAssetUtil
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

            //Destroy old asset before we re-save a new one
            if (AssetDatabase.LoadAssetAtPath<T>(fullPath))
            {
                AssetDatabase.DeleteAsset(fullPath);
            }
            
            AssetDatabase.CreateAsset(asset, fullPath);
        }
    }
}