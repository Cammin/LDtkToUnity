using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public class ComponentTilesTest
    {
        [Test]
        public void TestExpectedCoordsTileset()
        {
            //const string lvlName = "Level";
            
            string path = "Assets/Samples/Samples/Test_file_for_API_showing_all_features.ldtk";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            LDtkComponentProject project = prefab.GetComponent<LDtkComponentProject>();
            LDtkComponentLayer[] layers = project.Worlds[0].Levels[0].LayerInstances;

            foreach (LDtkComponentLayer layer in layers)
            {
                LDtkComponentLayerIntGridValues intGrid = layer.IntGrid;
                
                if (layer.Identifier == "IntGrid_without_rules")
                {
                    Assert.NotNull(intGrid);
                    Assert.IsNull(layer.AutoLayerTiles);
                    Assert.IsNull(layer.GridTiles);

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

            /*Level level = project.UnityWorlds.First().Levels.FirstOrDefault();
            Assert.NotNull(level, "null level");

            //LayerInstance layer = level.LayerInstances.FirstOrDefault(p => p.IsIntGridLayer);
            //Assert.NotNull(layer);

            Rect levelBounds = level.UnityWorldSpaceBounds(WorldLayout.Free, (int)16);


            //Debug.Log(levelBounds);*/
        }

        private static void TestIntGridPosition(LDtkComponentLayerIntGridValues intGrid, Vector3Int pos, bool assertHas, int assertIs = 0, Vector3Int[] assertAllTilesExist = null)
        {
            LDtkDefinitionObjectIntGridValue valueObj = intGrid.GetValueDefinition(pos);
            int value = intGrid.GetValue(pos);
            
            Assert.AreEqual(assertIs, value);
            Debug.Log($"{assertIs} == {value}");
            
            if (assertHas)
            {
                Assert.NotNull(valueObj);
                Assert.AreNotEqual(0, value);
                Debug.Log($"{assertIs} != 0");
                
                Vector3Int[] positionsObj = intGrid.GetPositionsOfValueDefinition(valueObj);
                Vector3Int[] positions = intGrid.GetPositionsOfValue(value);
            
                Assert.AreEqual(valueObj.Value, value);
                Assert.True(positionsObj.SequenceEqual(positions));
                
                Assert.True(ArraysContainSameValues(positionsObj, assertAllTilesExist));
                Debug.Log($"ArraysContainSameValues!");
            }
            else
            {
                Assert.IsNull(valueObj);
                Assert.AreEqual(0, value);
                Debug.Log($"{value} == 0");
            }
        }
        
        public static bool ArraysContainSameValues<T>(T[] array1, T[] array2)
        {
            return array1.Length == array2.Length && array1.All(array2.Contains) && array2.All(array1.Contains);
        }
    }
}
