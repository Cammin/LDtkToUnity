
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkPathUtil
    {
        /// <summary>
        /// //this directory is a folder that stands aside the project asset.
        /// Folder uses the same name as the original object
        /// </summary>
        public static string SiblingDirectoryOfAsset(Object obj)
        {
            string objAssetPath = AssetDatabase.GetAssetPath(obj);
            string objAssetDirectory = Path.GetDirectoryName(objAssetPath);
            
            string directory = $"{objAssetDirectory}/{obj.name}";

            return directory.Replace('\\', '/');
        }

        public static void CreateDirectoryIfNotValidFolder(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}