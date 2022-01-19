using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for AutoLayers and Tile layers in LDtk. Not IntGridValues.
    /// This only responsibility is for rendering art. That's all. //todo add tile animation here later
    /// </summary>
    [ExcludeFromDocs]
    [HelpURL(LDtkHelpURL.SO_ART_TILE)]
    public sealed class LDtkArtTile : TileBase
    {
        public Sprite _artSprite;
        public TileBase _animationOverride;
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _artSprite;
            tileData.colliderType = Tile.ColliderType.None;
            
            //make color full, the tilemap components themselves have the correct opacity set.
            tileData.color = Color.white;
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            return _animationOverride != null && _animationOverride.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }
    }
}