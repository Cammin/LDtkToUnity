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

                    Vector3Int[] allValidItems = new Vector3Int[]
                    {
                        new Vector3Int(3,28,0),
                    };
                    
                    //test what doesnt exist
                    TestIntGridPosition(intGrid, new Vector3Int(0,0,0), false);
                    TestIntGridPosition(intGrid, new Vector3Int(1,0,0), false);
                    TestIntGridPosition(intGrid, new Vector3Int(1000,9999,0), false);
                    
                    //test what exists
                    
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

        private static void TestIntGridPosition(LDtkComponentLayerIntGridValues intGrid, Vector3Int pos, bool assertHas, int assertIs = -1, Vector3Int[] assertAllTilesExist = null)
        {
            LDtkDefinitionObjectIntGridValue valueObj = intGrid.GetValueDefinition(pos);
            int value = intGrid.GetValue(pos);
            
            Assert.AreEqual(assertIs, value);
            
            if (assertHas)
            {
                Assert.NotNull(valueObj);
                Assert.AreNotEqual(-1, value);
                
                Vector3Int[] positionsObj = intGrid.GetPositionsOfValueDefinition(valueObj);
                Vector3Int[] positions = intGrid.GetPositionsOfValue(value);
            
                Assert.AreEqual(valueObj.Value, value);
                Assert.True(positionsObj.SequenceEqual(positions));
                
                Assert.True(ArraysContainSameValues(positionsObj, assertAllTilesExist));
            }
            else
            {
                Assert.IsNull(valueObj);
            }
        }
        
        public static bool ArraysContainSameValues<T>(T[] array1, T[] array2)
        {
            return array1.Length == array2.Length && array1.All(array2.Contains) && array2.All(array1.Contains);
        }
    }
}
