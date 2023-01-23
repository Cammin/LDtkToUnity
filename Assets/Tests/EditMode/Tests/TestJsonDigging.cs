using System.Collections.Generic;
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
        public const string LEVELS_VANIA_PROJECT_PATH = "Assets/Tests/Misc/OtherTechTests/LevelsVania.ldtk";
        
        public static string[] Paths = new[]
        {
            GENERIC_PROJECT_PATH,
            AUTO_LAYERS1_PATH,
            GRIDVANIA_PROJECT_PATH,
            LEVELS_VANIA_PROJECT_PATH,
            "Assets/Tests/Misc/OtherTechTests/Basic_PropertyNameExploit.ldtk",
        };
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetJsonVersion(string path)
        {
            string value = null;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetJsonVersion)}/{path}");
            bool success = LDtkJsonDigger.GetJsonVersion(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetJsonVersion was {value}");
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetDefaultGridSize(string path)
        {
            int value = 0;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetDefaultGridSize)}/{path}");
            bool success = LDtkJsonDigger.GetDefaultGridSize(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetDefaultGridSize was {value}");
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetIsExternalLevels(string path)
        {
            bool value = false;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetIsExternalLevels)}/{path}");
            bool success = LDtkJsonDigger.GetIsExternalLevels(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetIsExternalLevels was {value}");
        }
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedTilesetSprites(string path)
        {
            Dictionary<string, HashSet<int>> result = new Dictionary<string, HashSet<int>>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedTilesetSprites)}/{path}");
            bool success = LDtkJsonDigger.GetUsedTilesetSprites(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedTilesetSprites was {result}");
        }     
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedFieldTiles(string path)
        {
            List<FieldInstance> result = new List<FieldInstance>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedFieldTiles)}/{path}");
            bool success = LDtkJsonDigger.GetUsedFieldTiles(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedFieldTiles was {result}");
        }        
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedSeparateLevelBackgrounds(string path)
        {
            string result = null;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedSeparateLevelBackgrounds)}/{path}");
            bool success = LDtkJsonDigger.GetUsedSeparateLevelBackgrounds(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedSeparateLevelBackgrounds was {result}");
        }        
        
        //this function costs a lot of performance in particular
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedProjectLevelBackgrounds(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedProjectLevelBackgrounds)}/{path}");
            bool success = LDtkJsonDigger.GetUsedProjectLevelBackgrounds(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedProjectLevelBackgrounds was {result}");
        }        
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedIntGridValues(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedIntGridValues)}/{path}");
            bool success = LDtkJsonDigger.GetUsedIntGridValues(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedIntGridValues was {result}");
        }       
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetUsedEntities(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedEntities)}/{path}");
            bool success = LDtkJsonDigger.GetUsedEntities(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedEntities was {result}");
        }        
        
        [Test, TestCaseSource(nameof(Paths))]
        public static void GetTilesetRelPaths(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetTilesetRelPaths)}/{path}");
            bool success = LDtkJsonDigger.GetTilesetRelPaths(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetTilesetRelPaths was {result}");
        }
    }
}