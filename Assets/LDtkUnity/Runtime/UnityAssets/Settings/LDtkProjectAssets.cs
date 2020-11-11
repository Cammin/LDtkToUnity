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
        [SerializeField] private LDtkIntGridTileCollection _collisionTiles = null;
        [SerializeField] private LDtkEntityInstanceCollection _entityInstances = null;
        [SerializeField] private LDtkTilesetCollection _tilesets = null;
        [Space]
        [SerializeField] private Grid _collisionTilemapPrefab = null;
        //[SerializeField] private DimensionPreference _dimension = DimensionPreference.XY;

        //public DimensionPreference Dimension => _dimension;
        public LDtkIntGridTileCollection CollisionTiles => _collisionTiles;
        public LDtkEntityInstanceCollection EntityInstances => _entityInstances;
        public Grid CollisionTilemapPrefab => _collisionTilemapPrefab;

        public LDtkTilesetCollection Tilesets => _tilesets;
    }
}