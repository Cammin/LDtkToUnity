using System.IO;
using LDtkUnity.Editor;
using NUnit.Framework;
using UnityEngine.Profiling;

namespace LDtkUnity.Tests
{
    public static class TestJsonLoader
    {
        public const string GENERIC_PROJECT_PATH = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
        private const string GRIDVANIA_PROJECT_PATH = "Assets/Samples/Samples/WorldMap_GridVania_layout.ldtk";

        private static string[] _paths = new[]
        {
            GENERIC_PROJECT_PATH,
            GRIDVANIA_PROJECT_PATH,
        };

        public static LdtkJson DeserializeProject()
        {
            return DeserializeLdtkJson(GENERIC_PROJECT_PATH);
        }

        
        [Test, TestCaseSource(nameof(_paths))]
        public static void TestLoadTextAsset(string path) => LoadTextAsset(GENERIC_PROJECT_PATH);
        
        public static string LoadTextAsset(string path)
        {
            string jsonText = File.ReadAllText(path);
            Assert.NotNull(jsonText, "Unsuccessful read of json text");

            return jsonText;
        }
        
        [Test, TestCaseSource(nameof(_paths))]
        public static void TestDeserializeLdtkJson(string path)
        {
            DeserializeLdtkJson(path);
        }
        
        public static LdtkJson DeserializeLdtkJson(string path)
        {
            string jsonText = LoadTextAsset(path);

            LDtkProfiler.BeginSample($"DeserializeProject/SystemTestJson/{Path.GetFileName(path)}");
            Profiler.BeginSample("SystemTextJson");
            LdtkJson project = LdtkJson.FromJson(jsonText);
            Profiler.EndSample();
            LDtkProfiler.EndSample();
            
            Assert.NotNull(project, "Failure to deserialize LDtk project");
            return project;
        }
    }
}