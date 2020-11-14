using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Colliders;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Settings
{
    [CreateAssetMenu(fileName = nameof(LDtkProjectAssets), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkProjectAssets), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkProjectAssets : ScriptableObject
    {
        [SerializeField] private LDtkIntGridValueAssetCollection _collisionValues = null;
        
        [SerializeField] private LDtkEntityAssetCollection _entityInstances = null;
        [SerializeField] private LDtkTilesetAssetCollection _tilesets = null;
        [Space]
        [SerializeField] private Grid _tilemapPrefab = null;

        public LDtkIntGridValueAssetCollection CollisionValues => _collisionValues;
        public LDtkEntityAssetCollection EntityInstances => _entityInstances;
        public LDtkTilesetAssetCollection Tilesets => _tilesets;
        public Grid TilemapPrefab => _tilemapPrefab;
    }
}