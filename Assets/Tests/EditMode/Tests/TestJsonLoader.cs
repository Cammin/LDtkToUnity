using System.IO;
using NUnit.Framework;

namespace LDtkUnity.Tests
{
    public static class TestJsonLoader
    {
        public const string GENERIC_PROJECT_PATH = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
        public const string GRIDVANIA_PROJECT_PATH = "Assets/Samples/Samples/WorldMap_GridVania_layout.ldtk";
        
        
        public static string[] Paths = new[]
        {
            GENERIC_PROJECT_PATH,
            GRIDVANIA_PROJECT_PATH,
        };
        
        
        public static string LoadTextAsset(string path)
        {
            string jsonText = File.ReadAllText(path);
            Assert.NotNull(jsonText, "Unsuccessful read of json text");

            return jsonText;
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void TestLoadTextAsset(string path) => LoadTextAsset(path);
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void DeserializeLdtkJsonUtf8(string path)
        {
            string jsonText = LoadTextAsset(path);
            LdtkJson project = Utf8Json.JsonSerializer.Deserialize<LdtkJson>(jsonText);
            Assert.NotNull(project, "Failure to deserialize LDtk project");
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void DeserializeLdtkJsonUtf8Profile(string path)
        {
            string json = LoadTextAsset(path);

            LdtkJson project;
            
            LDtkProfiler.BeginWriting($"DeserializeProjectUtf8/{Path.GetFileName(path)}");

            LDtkProfiler.BeginSample("Utf8");
            project = Utf8Json.JsonSerializer.Deserialize<LdtkJson>(json);
            LDtkProfiler.EndSample();
            
            Assert.NotNull(project, "Failure to deserialize LDtk project");
            
            LDtkProfiler.EndWriting();
            
        }
    }
}