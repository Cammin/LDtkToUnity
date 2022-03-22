using System.IO;
using LDtkUnity;
using NUnit.Framework;

namespace Tests.EditMode.Tests
{
    public static class TestJsonLoader
    {
        private const string GENERIC_PROJECT_PATH = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
        
        public static string LoadTextAsset()
        {
            string jsonText = File.ReadAllText(GENERIC_PROJECT_PATH);
            Assert.NotNull(jsonText, "Unsuccessful read of json text");

            return jsonText;
        }
        
        public static LdtkJson DeserializeProject()
        {
            string jsonText = LoadTextAsset();

            LdtkJson project = LdtkJson.FromJson(jsonText);
            Assert.NotNull(project, "Failure to deserialize LDtk project");

            return project;
        }
        
        [Test]
        public static void TestLoadTextAsset()
        {
            LoadTextAsset();
        }
        
        [Test]
        public static void TestDeserializeProject()
        {
            DeserializeProject();
        }
    }
}