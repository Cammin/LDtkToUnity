using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for IntGridValues in LDtk. Not AutoLayers and Tile layers.
    /// Inherit from this for more custom functionality if required.
    /// </summary>
    [HelpURL(LDtkHelpURL.INT_GRID_TILE)]
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTile), menuName = LDtkToolScriptableObj.SO_ROOT + nameof(LDtkIntGridTile), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTile : TileBase
    {
        public const string PROP_COLLIDER_TYPE = nameof(_colliderType);
        public const string PROP_CUSTOM_PHYSICS_SPRITE = nameof(_customPhysicsSprite);
        public const string PROP_GAME_OBJECT = nameof(_gameObject);
        
        [SerializeField] protected Tile.ColliderType _colliderType;
        [SerializeField] protected Sprite _customPhysicsSprite;
        [SerializeField] protected GameObject _gameObject;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.flags = TileFlags.None;
            tileData.colliderType = GetColliderType();
            tileData.sprite = GetSprite();
            tileData.gameObject = _gameObject;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (go == null)
            {
                return true;
            }
            
            go.name = _gameObject.name;
            return true;
        }

        private Tile.ColliderType GetColliderType()
        {
            if (_colliderType == Tile.ColliderType.Sprite && _customPhysicsSprite == null)
            { 
                return Tile.ColliderType.None;
            }
            return _colliderType;
        }
        
        private Sprite GetSprite()
        {
            if (_colliderType == Tile.ColliderType.Sprite && _customPhysicsSprite != null)
            {
                return _customPhysicsSprite;
            }
            
            //previously tried implementing optional; renderings by returning a null sprite, but was causing dirty tilemaps
            return LDtkResourcesLoader.LoadDefaultTileSprite();
        }
    }
}