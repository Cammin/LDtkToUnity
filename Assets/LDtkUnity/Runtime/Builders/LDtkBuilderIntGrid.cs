using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Colliders;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.Builders
{
    public static class LDtkBuilderIntGrid
    {
        //only builds the collision boxes.
        public static void BuildIntGrid(Tilemap tilemap, LDtkIntGridTileCollection tiles, IEnumerable<LDtkDataIntGrid> intGrid, Vector2Int gridSize)
        {
            List<LDtkDataIntGrid> intGridTiles = intGrid.ToList();
            
            //Debug.Log($"LDtk: Trying to build IntGrid {tilemap.gameObject.name}");
            
            if (intGridTiles.NullOrEmpty())
            {
                Debug.LogWarning("IntGrid had no tiles. Something wrong?");
                return;
            }

            foreach (LDtkDataIntGrid intTile in intGridTiles)
            {
                if (intTile.v >= tiles.IncludedAssets.Count)
                {
                    Debug.LogError("Tile could not be gotton; IntGrid tile value is greater than the collection's size.");
                    continue;
                }
                
                Tile setTile = tiles.IncludedAssets[intTile.v].Asset;
                SetTile(tilemap, setTile, intTile, gridSize);
            }
        }

        private static void SetTile(Tilemap tilemap, TileBase setTile, LDtkDataIntGrid intTile, Vector2Int gridSize)
        {
            Vector2Int coordToSet = LDtkToolTileCoord.GetCellPositionFromCoordID(intTile.coordId, gridSize);
            tilemap.SetTile((Vector3Int) coordToSet, setTile);
        }
    }
}