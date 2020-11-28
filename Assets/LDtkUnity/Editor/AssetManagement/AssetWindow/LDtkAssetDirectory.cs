using System.IO;
using UnityEditor;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow
{
    public static class LDtkAssetDirectory
    {
        private const string ASSETS = "Assets/";
        
        public static void CreateDirectoryIfNotValidFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(ASSETS + folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}