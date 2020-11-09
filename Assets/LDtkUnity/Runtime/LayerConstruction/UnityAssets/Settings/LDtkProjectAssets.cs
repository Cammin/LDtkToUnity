using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Settings
{
    [CreateAssetMenu(fileName = nameof(LDtkProjectAssets), menuName = LDtkSOTool.SO_PATH + nameof(LDtkProjectAssets), order = LDtkSOTool.SO_ORDER)]
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