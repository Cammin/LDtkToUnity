using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
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
        
        public static Tilemap BuildUnityTileset(Grid tilemapPrefab, Vector2 position, string objName, int layerSortingOrder)
        {
            Grid grid = InstantiateTilemap(tilemapPrefab, position, objName);
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();

            if (tilemap != null)
            {
                TilemapRenderer renderer = tilemap.GetComponent<TilemapRenderer>();
                renderer.sortingOrder = layerSortingOrder;

                return tilemap;
            }
            
            Debug.LogError("Tilemap prefab does not have a Tilemap component in it's children", tilemapPrefab);
            return null;
        }
        
        private static Grid InstantiateTilemap(Grid prefab, Vector2 position, string objName)
        {
            Grid grid = Object.Instantiate(prefab);
            grid.transform.position = position;
            grid.gameObject.name = objName;
            return grid;
        }

        public static void SetTilesetOpacity(Tilemap tilemap, float alpha)
        {
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemap.SetTileFlags(pos, TileFlags.None);
                    
                    Color newColor = tilemap.GetColor(pos);
                    newColor.a = alpha;
                    tilemap.SetColor(pos, newColor);

                    //Debug.Log(newColor);
                }
            }
        }
    }
}