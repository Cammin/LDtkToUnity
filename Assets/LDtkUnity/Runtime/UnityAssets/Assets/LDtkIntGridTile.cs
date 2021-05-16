using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

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

        private Color _nextLDtkColor = Color.white;

        public void SetNextLDtkColor(Color color)
        {
            _nextLDtkColor = color;
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.flags = TileFlags.LockAll;
            
            tileData.colliderType = _colliderType;
            tileData.sprite = GetSprite();
            tileData.gameObject = _gameObject;

            Color color = Color.black;
            Debug.Log(color);

            tileData.color = color;
        }

        private Color GetColor()
        {
            return _nextLDtkColor;
        }
        
        private Sprite GetSprite()
        {
            switch (_colliderType)
            {
                case Tile.ColliderType.Sprite:
                    return _customPhysicsSprite;
                case Tile.ColliderType.Grid:
                    return LDtkResourcesLoader.LoadDefaultTileSprite();
                default:
                    return null;
            }
        }
    }
}