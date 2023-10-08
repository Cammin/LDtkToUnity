using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for IntGridValues in LDtk. Not AutoLayers and Tile layers.<br/>
    /// Inherit from this for more custom functionality if required.
    /// </summary>
    [HelpURL(LDtkHelpURL.SO_INT_GRID_TILE)]
    public class LDtkIntGridTile : TileBase
    {
        internal const string PROPERTY_COLLIDER_TYPE = nameof(_colliderType);
        internal const string PROPERTY_CUSTOM_PHYSICS_SPRITE = nameof(_customPhysicsSprite);
        internal const string PROPERTY_TAG = nameof(_tilemapTag);
        internal const string PROPERTY_LAYERMASK = nameof(_tilemapLayerMask);
        internal const string PROPERTY_PHYSICS_MATERIAL = nameof(_physicsMaterial);
        internal const string PROPERTY_GAME_OBJECT = nameof(_gameObject);
        
        /// <value>
        /// The collider type used on the tilemap.
        /// </value>
        [SerializeField] protected Tile.ColliderType _colliderType = Tile.ColliderType.None;
        
        /// <value>
        /// The physics sprite used when <see cref="_colliderType"/> is set to Sprite.
        /// </value>
        [SerializeField] protected Sprite _customPhysicsSprite = null;

        /// <value>
        /// Sets the tag of this tile's tilemap.
        /// </value>
        [SerializeField, LDtkTag] protected string _tilemapTag = "Untagged";
        
        /// <value>
        /// Sets the layer mask of this tile's tilemap.
        /// </value>
        [SerializeField, LDtkLayerMask] protected int _tilemapLayerMask = 0;
        
        /// <value>
        /// The optional physics material to be applied for this specific tile in a tilemap.
        /// </value>
        [SerializeField] protected PhysicsMaterial2D _physicsMaterial = null;
        
        /// <value>
        /// The optional GameObject to be placed at the tile's position.
        /// </value>
        [SerializeField] protected GameObject _gameObject = null;

        internal string TilemapTag => _tilemapTag;
        internal int TilemapLayerMask => _tilemapLayerMask;
        internal PhysicsMaterial2D PhysicsMaterial => _physicsMaterial;
        internal Sprite PhysicsSprite => _customPhysicsSprite;
        internal Tile.ColliderType ColliderType => _colliderType;
        
        /// <summary>
        /// This TileBase inherited method for GetTileData.
        /// </summary>
        /// <param name="position">
        /// Position on the tilemap.
        /// </param>
        /// <param name="tilemap">
        /// The tilemap.
        /// </param>
        /// <param name="tileData">
        /// TileData to set.
        /// </param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.flags = TileFlags.None;
            tileData.colliderType = GetColliderType();
            tileData.sprite = GetSprite();
            tileData.gameObject = _gameObject;
            tileData.color = Color.white;
        }

        /// <summary>
        /// This TileBase inherited method for StartUp.
        /// </summary>
        /// <param name="position">
        /// Position on the tilemap.
        /// </param>
        /// <param name="tilemap">
        /// The tilemap.
        /// </param>
        /// <param name="go">
        /// The instantiated GameObject.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (go == null || _gameObject == null)
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