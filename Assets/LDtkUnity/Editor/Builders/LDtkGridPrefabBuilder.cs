using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor.Builders
{
    //class will be deprecated, so add these to the original level builder
    public class LDtkGridPrefabBuilder
    {
        public Tilemap BuildUnityTileset(string objName, Grid tilemapPrefab, int layerSortingOrder, int pixelsPerUnit, int layerGridSize)
        {
            //instantiating a prefab bloats the scene size, only instantiate
            Grid grid = Object.Instantiate(tilemapPrefab);
            
            grid.gameObject.name = objName;
            
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();

            //toDo validate this works as expected
            float size = (float) layerGridSize / pixelsPerUnit;
            grid.transform.localScale = Vector3.one * size;
            
            if (tilemap != null)
            {
                TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();

                if (renderer)
                {
                    renderer.sortingOrder = layerSortingOrder;
                }
                return tilemap;
            }
            
            Debug.LogError("Tilemap prefab does not have a Tilemap component in it's children", tilemapPrefab);
            return null;
        }


        
    }
}