using LDtkUnity.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Builders
{
    public static class LDtkUnityTilesetBuilder
    {
        public static bool ValidateTilemapPrefabRequirements(Grid tilemapPrefab)
        {
            if (tilemapPrefab == null)
            {
                Debug.LogError("LDtk: Tilemap prefab is null");
                return false;
            }
            
            foreach (Transform child in tilemapPrefab.transform)
            {
                Tilemap tilemap = child.GetComponent<Tilemap>();
                if (tilemap == null)
                {
                    continue;
                }

                if (!tilemap.GetComponent<TilemapRenderer>())
                {
                    Debug.LogError("LDtk: Tilemap prefab does not contain a TilemapRenderer component as a child", tilemapPrefab);
                    return false;
                }
                
                if (!tilemap.GetComponent<TilemapCollider2D>())
                {
                    Debug.LogError("LDtk: Tilemap prefab does not contain a TilemapCollider2D component as a child", tilemapPrefab);
                    return false;
                }

                return true;
            }

            Debug.LogError("LDtk: Tilemap prefab does not contain a Tilemap component as a child", tilemapPrefab);
            return false;
        }
        
        public static Tilemap BuildUnityTileset(string objName, Grid tilemapPrefab, int layerSortingOrder, int pixelsPerUnit, int layerGridSize)
        {
            Grid grid = InstantiateTilemap(tilemapPrefab, objName);
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();

            //toDo validate this works as expected
            float size = (float) layerGridSize / pixelsPerUnit;
            grid.transform.localScale = Vector3.one * size;
            
            if (tilemap != null)
            {
                TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
                renderer.sortingOrder = layerSortingOrder;

                return tilemap;
            }
            
            Debug.LogError("Tilemap prefab does not have a Tilemap component in it's children", tilemapPrefab);
            return null;
        }
        
        private static Grid InstantiateTilemap(Grid prefab, string objName)
        {
            Grid grid = LDtkPrefabFactory.Instantiate(prefab);
            grid.gameObject.name = objName;
            return grid;
        }

        public static void SetTilesetOpacity(Tilemap tilemap, double alpha)
        {
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemap.SetTileFlags(pos, TileFlags.None);
                    
                    Color newColor = tilemap.GetColor(pos);
                    newColor.a = (float)alpha;
                    tilemap.SetColor(pos, newColor);

                    //Debug.Log(newColor);
                }
            }
        }
    }
}