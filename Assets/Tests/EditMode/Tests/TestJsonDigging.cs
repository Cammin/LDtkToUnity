using System.Collections.Generic;
using System.IO;
using System.Linq;
using LDtkUnity.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Tests
{
    public static class TestJsonDigging
    {
        public static string[] Projects = new[]
        {
            "Assets/Samples/Samples/AutoLayers_1_basic.ldtk",
            "Assets/Samples/Samples/AutoLayers_2_stamps.ldtk",
            "Assets/Samples/Samples/AutoLayers_3_Mosaic.ldtk",
            "Assets/Samples/Samples/AutoLayers_4_Assistant.ldtk",
            "Assets/Samples/Samples/AutoLayers_5_Advanced.ldtk",
            "Assets/Samples/Samples/AutoLayers_6_OptionalRules.ldtk",
            "Assets/Samples/Samples/Entities.ldtk",
            "Assets/Samples/Samples/SeparateLevelFiles.ldtk",
            "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk",
            "Assets/Samples/Samples/Typical_2D_platformer_example.ldtk",
            "Assets/Samples/Samples/Typical_TopDown_example.ldtk",
            "Assets/Samples/Samples/WorldMap_Free_layout.ldtk",
            "Assets/Samples/Samples/WorldMap_GridVania_layout.ldtk",
            
            "Assets/Tests/Misc/OtherTechTests/Basic.ldtk",
            "Assets/Tests/Misc/OtherTechTests/Basic_PropertyNameExploit.ldtk",
            "Assets/Tests/Misc/OtherTechTests/Basic_Tileset.ldtk",
            "Assets/Tests/Misc/OtherTechTests/Basic_Tileset_GridSize.ldtk",
            "Assets/Tests/Misc/OtherTechTests/Basic_Tileset_GridTiles.ldtk",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania.ldtk",
            "Assets/Tests/Misc/OtherTechTests/TestAllFields.ldtk",
            "Assets/Tests/Misc/OtherTechTests/TestAllFieldsLevels.ldtk",
            "Assets/Tests/Misc/OtherTechTests/TestPivot.ldtk",
            
            "Assets/Tests/Misc/ProperPPU/ProperPixelsPerUnit.ldtk",
        };

        public static string[] Levels = new[]
        {
            "Assets/Samples/Samples/SeparateLevelFiles/World_Level_0.ldtkl",
            "Assets/Samples/Samples/SeparateLevelFiles/World_Level_1.ldtkl",
            "Assets/Samples/Samples/SeparateLevelFiles/World_Level_2.ldtkl",
            
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Boss_room.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Cross_roads.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Entrance.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Exit.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Flooded_rooms.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Garden.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Hidden_cave.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Large_water.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Long_hallway.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Ossuary.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Pit.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Save.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Sewers1.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Sewers2.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Sewers_trash.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Shop.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Shop_entrance.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Shortcut_passage.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/The_ponds.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/The_tower.ldtkl",
            "Assets/Tests/Misc/OtherTechTests/LevelsVania/Water_supply.ldtkl",

        };
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        public static void GetJsonVersion(string path)
        {
            string value = null;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetJsonVersion)}/{path}");
            bool success = LDtkJsonDigger.GetJsonVersion(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetJsonVersion was {value}");
        }
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        public static void GetDefaultGridSize(string path)
        {
            int value = 0;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetDefaultGridSize)}/{path}");
            bool success = LDtkJsonDigger.GetDefaultGridSize(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetDefaultGridSize was {value}");
        }
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        public static void GetIsExternalLevels(string path)
        {
            bool value = false;
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetIsExternalLevels)}/{path}");
            bool success = LDtkJsonDigger.GetIsExternalLevels(path, ref value);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetIsExternalLevels was {value}");
        }
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        [TestCaseSource(nameof(Levels))]
        public static void GetUsedTilesetSprites(string path)
        {
            Dictionary<string, HashSet<int>> result = new Dictionary<string, HashSet<int>>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedTilesetSprites)}/{path}");
            bool success = LDtkJsonDigger.GetUsedTilesetSprites(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            
            IEnumerable<string> lines = result.Select(kvp => $"{kvp.Key}: [{string.Join(", ", kvp.Value)}]");
            Debug.Log($"GetUsedTilesetSprites was {result.Count}: \n{string.Join(",\n", lines)}");
        }     
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        [TestCaseSource(nameof(Levels))]
        public static void GetUsedFieldTiles(string path)
        {
            List<FieldInstance> result = new List<FieldInstance>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedFieldTiles)}/{path}");
            bool success = LDtkJsonDigger.GetUsedFieldTiles(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            
            string[] lines = result.Select(field =>
            {
                if (field.Value is TilesetRectangle rect)
                {
                    return $"{field.Identifier}: [{rect.ToString()}]";
                }
                if (field.Value is TilesetRectangle[] rects)
                {
                    return $"{field.Identifier}: [{string.Join(", ", rects.Select(p => p.ToString()))}]";
                }
                Assert.Fail();
                return "null";
            }).ToArray();
            Debug.Log($"GetUsedFieldTiles was {result.Count}: \n{string.Join(",\n", lines)}");
        }

        //this function costs a lot of performance in particular
        [Test]
        [TestCaseSource(nameof(Projects))]
        [TestCaseSource(nameof(Levels))]
        public static void GetUsedBackgrounds(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedBackgrounds)}/{path}");
            bool success = LDtkJsonDigger.GetUsedBackgrounds(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedProjectLevelBackgrounds was {result.Count}: {string.Join(", ", result)}");
        }        
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        [TestCaseSource(nameof(Levels))]
        public static void GetUsedIntGridValues(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedIntGridValues)}/{path}");
            bool success = LDtkJsonDigger.GetUsedIntGridValues(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedIntGridValues was {result.Count}: {string.Join(", ", result)}");
        }       
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        [TestCaseSource(nameof(Levels))]
        public static void GetUsedEntities(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetUsedEntities)}/{path}");
            bool success = LDtkJsonDigger.GetUsedEntities(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetUsedEntities was {result.Count}: {string.Join(", ", result)}");
        }        
        
        [Test]
        [TestCaseSource(nameof(Projects))]
        public static void GetTilesetRelPaths(string path)
        {
            HashSet<string> result = new HashSet<string>();
            
            LDtkProfiler.BeginSample($"{nameof(TestJsonDigging)}/{nameof(GetTilesetRelPaths)}/{path}");
            bool success = LDtkJsonDigger.GetTilesetRelPaths(path, ref result);
            LDtkProfiler.EndSample();
            
            Assert.IsTrue(success, "not successful");
            Debug.Log($"GetTilesetRelPaths was {result.Count}: {string.Join(", ", result)}");
        }
    }
}