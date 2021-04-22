using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used in tilemaps for LDtk.
    /// </summary>
    public class LDtkTile : TileBase
    {
        public const string PROP_COLLIDER_TYPE = nameof(_colliderType);
        public const string PROP_CUSTOM_PHYSICS_SPRITE = nameof(_customPhysicsSprite);
        
        [SerializeField] private Tile.ColliderType _colliderType;
        [SerializeField] private Sprite _customPhysicsSprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _customPhysicsSprite;
            tileData.colliderType = _colliderType;
            
            //make color full, the tilemaps themselves have the correct opacity set.
            tileData.color = Color.white;
        }
    }
}