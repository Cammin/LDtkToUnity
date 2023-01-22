using System.IO;
using LDtkUnity.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Tests
{
    public static class TestJsonDigging
    {
        public const string GENERIC_PROJECT_PATH = "Assets/Tests/Misc/OtherTechTests/Basic.ldtk";
        public const string AUTO_LAYERS1_PATH = "Assets/Samples/Samples/AutoLayers_1_basic.ldtk";
        public const string GRIDVANIA_PROJECT_PATH = "Assets/Samples/Samples/WorldMap_GridVania_layout.ldtk";
        
        
        public static string[] Paths = new[]
        {
            GENERIC_PROJECT_PATH,
            AUTO_LAYERS1_PATH,
            GRIDVANIA_PROJECT_PATH,
        };
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void TestGetJsonVersion(string path)
        {
            string value = null;
            bool success = LDtkJsonDigger.GetJsonVersion(path, ref value);
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"JsonVersion was {value}");
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void TestGetDefaultGridSize(string path)
        {
            int value = 0;
            bool success = LDtkJsonDigger.GetDefaultGridSize(path, ref value);
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"DefaultGridSize was {value}");
        }
    }
}