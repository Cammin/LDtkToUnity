using NUnit.Framework;
using UnityEditor;

namespace Tests.Editor
{
    public static class TestImport
    {
        [Test]
        public static void ImportProject()
        {
            string path = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
            AssetDatabase.ImportAsset(path);
        }
        
        [Test]
        public static void ImportLevel()
        {
            string path = "Assets/Samples/Samples/Test_file_for_API_showing_all_features/0000-Level_0.ldtkl";
            AssetDatabase.ImportAsset(path);
        }
    }
}