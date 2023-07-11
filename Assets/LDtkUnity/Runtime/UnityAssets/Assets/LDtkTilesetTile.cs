using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for AutoLayers and Tile layers in LDtk. It can have collision shapes configured from the tileset file's sprite editor.
    /// </summary>
    [HelpURL(LDtkHelpURL.SO_ART_TILE)]
    public sealed class LDtkTilesetTile : TileBase
    {
        public Sprite _artSprite;
        public Tile.ColliderType _type;
        
        //todo add tile animation here later. through sprite editor custom module?
        public TileBase _animationOverride;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.colliderType = _type;
            tileData.sprite = _artSprite;
            
            //make color full, the tilemap components themselves have the actual requested opacity set.
            tileData.color = Color.white;
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            return _animationOverride != null && _animationOverride.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }
    }
}