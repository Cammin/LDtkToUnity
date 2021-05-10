using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for AutoLayers and Tile layers in LDtk. Not IntGridValues.
    /// This only responsibility is for rendering art. That's all. 
    /// </summary>
    public sealed class LDtkArtTile : TileBase
    {
        public Sprite _artSprite;
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _artSprite;
            tileData.colliderType = Tile.ColliderType.None;
            
            //make color full, the tilemap components themselves have the correct opacity set.
            tileData.color = Color.white;
        }
    }
}