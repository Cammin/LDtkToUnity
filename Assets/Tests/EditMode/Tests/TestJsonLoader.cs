using System.IO;
using LDtkUnity.Editor;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine.Profiling;
using Utf8Json.Resolvers;
using Utf8Json.Unity;

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

        
        [Test, TestCaseSource(nameof(_paths))]
        public static void TestLoadTextAsset(string path) => LoadTextAsset(GENERIC_PROJECT_PATH);
        public static string LoadTextAsset(string path)
        {
            string jsonText = File.ReadAllText(path);
            Assert.NotNull(jsonText, "Unsuccessful read of json text");

            return jsonText;
        }
        
        [Test, TestCaseSource(nameof(_paths))]
        public static LdtkJson DeserializeLdtkJsonNewtonsoft(string path)
        {
            string jsonText = LoadTextAsset(path);
            LdtkJson project = LdtkJson.FromJson(jsonText);
            Assert.NotNull(project, "Failure to deserialize LDtk project");
            return project;
        }
        
        [Test, TestCaseSource(nameof(_paths))]
        public static void DeserializeLdtkJsonUtf8(string path)
        {
            string jsonText = LoadTextAsset(path);
            LdtkJson project = Utf8Json.JsonSerializer.Deserialize<LdtkJson>(jsonText);
            Assert.NotNull(project, "Failure to deserialize LDtk project");
        }
        
        [Test, TestCaseSource(nameof(_paths))]
        public static void DeserializeLdtkJsonNewtonsoftVsUtf8(string path)
        {
            string json = LoadTextAsset(path);

            LdtkJson project;
            
            LDtkProfiler.BeginSample($"DeserializeProjectNewtonsoftVsUtf8/{Path.GetFileName(path)}");
            
            Profiler.BeginSample("Newtonsoft");
            project = JsonConvert.DeserializeObject<LdtkJson>(json);
            Profiler.EndSample();
            
            Assert.NotNull(project, "Failure to deserialize LDtk project");
            
            Profiler.BeginSample("Utf8");
            project = Utf8Json.JsonSerializer.Deserialize<LdtkJson>(json);
            Profiler.EndSample();
            
            Assert.NotNull(project, "Failure to deserialize LDtk project");
            
            LDtkProfiler.EndSample();
            
        }
    }
}