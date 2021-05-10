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
        public const string PROP_USE_LDTK_DEFINITION_COLOR = nameof(_useLdtkDefinitionColor);
        public const string PROP_CUSTOM_COLOR = nameof(_customColor);
        public const string PROP_GAME_OBJECT = nameof(_gameObject);
        
        
        [SerializeField] protected Tile.ColliderType _colliderType;
        [SerializeField] protected Sprite _customPhysicsSprite;
        [SerializeField] protected bool _useLdtkDefinitionColor = true;
        [SerializeField] protected Color _customColor = Color.white;
        [SerializeField] protected Color _defColor;
        [SerializeField] protected GameObject _gameObject;
        
        public virtual void SetDefinitionData(IntGridValueDefinition def)
        {
            _defColor = def.UnityColor;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.colliderType = _colliderType;
            tileData.sprite = _customPhysicsSprite;
            tileData.color = _useLdtkDefinitionColor ? _defColor : _customColor;
            tileData.gameObject = _gameObject;
        }
    }
}