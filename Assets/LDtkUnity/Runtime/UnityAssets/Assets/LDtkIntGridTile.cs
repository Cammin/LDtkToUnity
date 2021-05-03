using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for IntGridValues in LDtk. Not AutoLayers and Tile layers.
    /// </summary>
    public class LDtkIntGridTile : TileBase
    {
        public Tile.ColliderType _colliderType;
        public Sprite _customPhysicsSprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _customPhysicsSprite;
            tileData.colliderType = _colliderType;
            
            //make color full, the tilemaps themselves have the correct opacity set.
            tileData.color = Color.white;
        }
        
        
    }
}