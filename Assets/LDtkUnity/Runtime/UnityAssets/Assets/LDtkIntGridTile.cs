using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    /// <summary>
    /// The tile used for IntGridValues in LDtk. Not AutoLayers and Tile layers.<br/>
    /// Inherit from this for more custom functionality if required.
    /// </summary>
    [HelpURL(LDtkHelpURL.SO_INT_GRID_TILE)]
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTile), menuName = LDtkToolScriptableObj.SO_ROOT + nameof(LDtkIntGridTile), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTile : TileBase
    {
        [ExcludeFromDocs] public const string PROP_COLLIDER_TYPE = nameof(_colliderType);
        [ExcludeFromDocs] public const string PROP_CUSTOM_PHYSICS_SPRITE = nameof(_customPhysicsSprite);
        [ExcludeFromDocs] public const string PROP_GAME_OBJECT = nameof(_gameObject);
        
        /// <summary>
        /// The collider type used on the tilemap.
        /// </summary>
        [SerializeField] protected Tile.ColliderType _colliderType;
        
        /// <summary>
        /// The physics sprite used when <see cref="_colliderType"/> is set to Sprite.
        /// </summary>
        [SerializeField] protected Sprite _customPhysicsSprite;
        
        /// <summary>
        /// The optional GameObject to be placed at the tile's position.
        /// </summary>
        [SerializeField] protected GameObject _gameObject;

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