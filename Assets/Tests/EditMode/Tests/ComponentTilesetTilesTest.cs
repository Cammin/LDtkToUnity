using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public class ComponentTilesTest
    {
        [Test]
        public void EnsureComponentExistences()
        {
            LDtkComponentProject project = LoadProjectComponent();
            Assert.NotNull(project, "project component null");

            LDtkComponentWorld world = project.Worlds[0];
            Assert.NotNull(world, "world component null");
            
            LDtkComponentLevel level = world.Levels[0];
            Assert.NotNull(level, "level component null");
            
            LDtkComponentLayer[] layers = level.LayerInstances;
            Assert.NotNull(layers, "layers component null");

            foreach (LDtkComponentLayer layer in layers)
            {
                LDtkComponentLayerIntGridValues intGrid = layer.IntGrid;
                
                if (layer.Identifier == "IntGrid_without_rules")
                {
                    Assert.NotNull(intGrid);
                    Assert.IsNull(layer.AutoLayerTiles);
                    Assert.IsNull(layer.GridTiles);
                }
                if (layer.Identifier == "IntGrid_with_rules")
                {
                    Assert.NotNull(intGrid);
                    Assert.NotNull(layer.AutoLayerTiles);
                    Assert.IsNull(layer.GridTiles);
                }
                if (layer.Identifier == "PureAutoLayer")
                {
                    Assert.IsNull(intGrid);
                    Assert.NotNull(layer.AutoLayerTiles);
                    Assert.IsNull(layer.GridTiles);
                }
                if (layer.Identifier == "Tiles")
                {
                    Assert.IsNull(intGrid);
                    Assert.IsNull(layer.AutoLayerTiles);
                    Assert.NotNull(layer.GridTiles);
                }
                if (layer.Identifier == "IntGrid_8px_grid")
                {
                    Assert.NotNull(intGrid);
                    Assert.IsNull(layer.AutoLayerTiles);
                    Assert.IsNull(layer.GridTiles);
                }
            }
        }

        [Test]
        public void TestIntGridWithoutRules()
        {
            LDtkComponentProject project = LoadProjectComponent();
            Assert.NotNull(project, "project component null");

            LDtkComponentWorld world = project.Worlds[0];
            Assert.NotNull(world, "world component null");
            
            LDtkComponentLevel level = world.Levels[0];
            Assert.NotNull(level, "level component null");
            
            LDtkComponentLayer[] layers = level.LayerInstances;
            Assert.NotNull(layers, "layers component null");
            
            Assert.True(layers.Any(p => p.Identifier == "IntGrid_without_rules"));
            
            foreach (LDtkComponentLayer layer in layers)
            {
                LDtkComponentLayerIntGridValues intGrid = layer.IntGrid;
                
                if (layer.Identifier == "IntGrid_without_rules")
                {
                    Vector3Int[] allValidItems1 = new Vector3Int[]
                    {
                        new Vector3Int(3,28,0),
                        new Vector3Int(4,28,0),
                        new Vector3Int(5,28,0),
                        new Vector3Int(6,28,0),
                        new Vector3Int(7,28,0),
                        
                        new Vector3Int(8,28,0),
                        new Vector3Int(8,27,0),
                        new Vector3Int(8,26,0),
                        new Vector3Int(8,25,0),
                        new Vector3Int(8,24,0),
                        new Vector3Int(8,23,0),
                        
                        new Vector3Int(7,23,0),
                        new Vector3Int(6,23,0),
                        new Vector3Int(5,23,0),
                        new Vector3Int(4,23,0),
                        new Vector3Int(3,23,0),
                        
                        new Vector3Int(3,24,0),
                        new Vector3Int(3,25,0),
                        new Vector3Int(3,26,0),
                        new Vector3Int(3,27,0),
                    };
                    Vector3Int[] allValidItems2 = new Vector3Int[]
                    {
                        new Vector3Int(4,27,0),
                        new Vector3Int(5,27,0),
                        new Vector3Int(6,27,0),
                        new Vector3Int(7,27,0),
                        
                        new Vector3Int(4,26,0),
                        new Vector3Int(5,26,0),
                        new Vector3Int(6,26,0),
                        new Vector3Int(7,26,0),

                        new Vector3Int(4,25,0),
                        new Vector3Int(5,25,0),
                        new Vector3Int(6,25,0),
                        new Vector3Int(7,25,0),

                        new Vector3Int(4,24,0),
                        new Vector3Int(5,24,0),
                        new Vector3Int(6,24,0),
                        new Vector3Int(7,24,0),

                    };
                    Vector3Int[] allValidItems3 = new Vector3Int[]
                    {
                        new Vector3Int(19,28,0),
                        new Vector3Int(19,27,0),
                        new Vector3Int(19,26,0),
                        new Vector3Int(19,25,0),
                        new Vector3Int(19,24,0),
                        new Vector3Int(19,23,0),
                        
                        new Vector3Int(20,28,0),
                        new Vector3Int(20,27,0),
                        new Vector3Int(20,26,0),
                        new Vector3Int(20,25,0),
                        new Vector3Int(20,24,0),
                        new Vector3Int(20,23,0),
                        
                        new Vector3Int(21,28,0),
                        new Vector3Int(21,27,0),
                        new Vector3Int(21,26,0),
                        new Vector3Int(21,25,0),
                        new Vector3Int(21,24,0),
                        new Vector3Int(21,23,0),
                        
                        new Vector3Int(22,28,0),
                        new Vector3Int(22,27,0),
                        new Vector3Int(22,26,0),
                        new Vector3Int(22,25,0),
                        new Vector3Int(22,24,0),
                        new Vector3Int(22,23,0),
                        
                        new Vector3Int(23,28,0),
                        new Vector3Int(23,27,0),
                        new Vector3Int(23,26,0),
                        new Vector3Int(23,25,0),
                        new Vector3Int(23,24,0),
                        new Vector3Int(23,23,0),
                        
                        new Vector3Int(24,28,0),
                        new Vector3Int(24,27,0),
                        new Vector3Int(24,26,0),
                        new Vector3Int(24,25,0),
                        new Vector3Int(24,24,0),
                        new Vector3Int(24,23,0),
                    };
                    
                    //test what doesnt exist
                    TestIntGridPosition(intGrid, new Vector3Int(0,0,0), false);
                    TestIntGridPosition(intGrid, new Vector3Int(1,0,0), false);
                    TestIntGridPosition(intGrid, new Vector3Int(1000,9999,0), false);
                    
                    //test what exists
                    TestIntGridPosition(intGrid, new Vector3Int(3, 28, 0), true, 1, allValidItems1);
                    TestIntGridPosition(intGrid, new Vector3Int(5, 26, 0), true, 2, allValidItems2);
                    TestIntGridPosition(intGrid, new Vector3Int(20, 27, 0), true, 3, allValidItems3);
                }
            }
        }

        [Test]
        public void TestIntGridWithRules()
        {
            LDtkComponentProject project = LoadProjectComponent();
            LDtkComponentLayer[] layers = project.Worlds[0].Levels[0].LayerInstances;

            Assert.True(layers.Any(p => p.Identifier == "IntGrid_with_rules"));
            
            foreach (LDtkComponentLayer layer in layers)
            {
                LDtkComponentLayerIntGridValues intGrid = layer.IntGrid;
                
                if (layer.Identifier == "IntGrid_with_rules")
                {
                    Vector3Int[] allValidItems = new Vector3Int[]
                    {
                        new Vector3Int(11,28,0),
                        new Vector3Int(11,27,0),
                        new Vector3Int(11,26,0),
                        new Vector3Int(11,25,0),
                        new Vector3Int(11,24,0),
                        new Vector3Int(11,23,0),
                        
                        new Vector3Int(12,28,0),
                        new Vector3Int(12,27,0),
                        new Vector3Int(12,26,0),
                        new Vector3Int(12,25,0),
                        new Vector3Int(12,24,0),
                        new Vector3Int(12,23,0),
                        
                        new Vector3Int(13,28,0),
                        new Vector3Int(13,27,0),
                        new Vector3Int(13,26,0),
                        new Vector3Int(13,25,0),
                        new Vector3Int(13,24,0),
                        new Vector3Int(13,23,0),
                        
                        new Vector3Int(14,28,0),
                        new Vector3Int(14,27,0),
                        new Vector3Int(14,26,0),
                        new Vector3Int(14,25,0),
                        new Vector3Int(14,24,0),
                        new Vector3Int(14,23,0),
                        
                        new Vector3Int(15,28,0),
                        new Vector3Int(15,27,0),
                        new Vector3Int(15,26,0),
                        new Vector3Int(15,25,0),
                        new Vector3Int(15,24,0),
                        new Vector3Int(15,23,0),
                        
                        new Vector3Int(16,28,0),
                        new Vector3Int(16,27,0),
                        new Vector3Int(16,26,0),
                        new Vector3Int(16,25,0),
                        new Vector3Int(16,24,0),
                        new Vector3Int(16,23,0),
                    };
                    
                    TestIntGridPosition(intGrid, new Vector3Int(12,28,0), true, 1, allValidItems);
                    
                    TestTilesetTilePosition(layer.AutoLayerTiles, new Vector3Int(10,28,0), 0);
                    TestTilesetTilePosition(layer.AutoLayerTiles, new Vector3Int(11,28,0), 1);
                    TestTilesetTilePosition(layer.AutoLayerTiles, new Vector3Int(13,28,0), 2);
                }
            }
        }

        private static LDtkComponentProject LoadProjectComponent()
        {
            string path = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            LDtkComponentProject project = prefab.GetComponent<LDtkComponentProject>();
            return project;
        }

        private static void TestIntGridPosition(LDtkComponentLayerIntGridValues intGrid, Vector3Int pos, bool assertHas, int assertIs = 0, Vector3Int[] assertAllTilesExist = null)
        {
            Debug.Log($"TestIntGridPosition {pos}, shouldExist: {assertHas}");
            
            LDtkDefinitionObjectIntGridValue valueObj = intGrid.GetValueDefinition(pos);
            int value = intGrid.GetValue(pos);
            
            Debug.Log($"\tExpect value {assertIs}. {assertIs} == {value}");
            Assert.AreEqual(assertIs, value);
            
            if (assertHas)
            {
                Assert.NotNull(valueObj);
                Assert.AreNotEqual(0, value);
                Debug.Log($"\t{assertIs} != 0");
                
                Vector3Int[] positionsObj = intGrid.GetPositionsOfValueDefinition(valueObj);
                Vector3Int[] positions = intGrid.GetPositionsOfValue(value);
            
                Assert.AreEqual(valueObj.Value, value);
                Assert.True(positionsObj.SequenceEqual(positions));
                
                Assert.True(ArraysContainSameValues(positionsObj, assertAllTilesExist));
                Debug.Log($"\tArraysContainSameValues!");
            }
            else
            {
                Assert.IsNull(valueObj);
                Assert.AreEqual(0, value);
                Debug.Log($"\t{value} == 0");
            }
        }
        
        public static bool ArraysContainSameValues<T>(T[] array1, T[] array2)
        {
            return array1.Length == array2.Length && array1.All(array2.Contains) && array2.All(array1.Contains);
        }

        private static void TestTilesetTilePosition(LDtkComponentLayerTilesetTiles tilesComponent, Vector3Int pos, int assertTileCount = 0)
        {
            Debug.Log($"Test TilesetPosition {pos}");
            Vector3Int[] coords = tilesComponent.GetCoordinatesOfEnumValue<SomeEnum>();
            Debug.Log($"\tExpecting array empty. {coords.Length}");
            Assert.IsEmpty(coords);

            LDtkTilesetTile[] tiles = tilesComponent.GetTilesetTiles(pos);
            
            //Assert.IsTrue(tiles.Length == 2);

            int notNullCount = tiles.Count(p => p != null);
            Debug.Log($"\tGetTilesetTiles {tiles.Length}, where not-null count is {notNullCount}, expecting {assertTileCount}");
        }
    }
}
