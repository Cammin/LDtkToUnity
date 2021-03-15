using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

#pragma warning disable 0414

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.ASSET_PROJECT)]
    [CreateAssetMenu(fileName = nameof(LDtkProject), menuName = LDtkToolScriptableObj.SO_ROOT + "LDtk Project", order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkProject : ScriptableObject
    {
        public const string JSON = nameof(_jsonProject);
        public const string LEVEL = nameof(_levels);
        public const string INTGRID = nameof(_intGridValues);
        public const string INTGRID_TILES = nameof(_intGridValueTiles);
        public const string ENTITIES = nameof(_entities);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        public const string ENUM_ASSEMBLY = nameof(_enumAssembly);
        public const string TILESETS = nameof(_tilesets);
        public const string TILE_COLLECTIONS = nameof(_tileCollections);
        public const string TILEMAP_PREFAB = nameof(_tilemapPrefab);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        
        private const string GRID_PREFAB_PATH = "LDtkDefaultGrid";
        private const string MAGIC_DEFAULT_NULL_TILE = "DefaultNullTile";
        
        [SerializeField] private LDtkProjectFile _jsonProject = null;
        [SerializeField] private Grid _tilemapPrefab = null;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private int _pixelsPerUnit = 16;
        
        [SerializeField] private string _enumNamespace = string.Empty;
        [SerializeField] private LDtkTileCollection _intGridValueTiles = null;
        [SerializeField] private Object _enumAssembly = null;
        
        [SerializeField] private LDtkAsset[] _levels = null;
        [SerializeField] private LDtkAsset[] _intGridValues = null;
        [SerializeField] private LDtkAsset[] _entities = null;
        [SerializeField] private LDtkAsset[] _tilesets = null;
        [SerializeField] private LDtkAsset[] _tileCollections = null;


        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public LDtkProjectFile ProjectJson => _jsonProject;

        public LDtkLevelFile[] LevelAssets => _levels.Select(p => p.GetAsset<LDtkLevelFile>()).ToArray();

        public LDtkLevelFile GetLevel(string key) => GetAssetByIdentifier<LDtkLevelFile>(_levels, key);
        public GameObject GetEntity(string key) => GetAssetByIdentifier<GameObject>(_entities, key);
        public Texture2D GetTileset(string key) => GetAssetByIdentifier<Texture2D>(_tilesets, key);
        public LDtkTileCollection GetTileCollection(string identifier) => GetAssetByIdentifier<LDtkTileCollection>(_tileCollections, identifier);
        
        
        public Tile GetIntGridValue(string key)
        {
            if (LDtkProviderErrorIdentifiers.Contains(key))
            {
                //this is to help prevent too much log spam. only one mistake from the same identifier get is necessary.
                return default;
            }
            
            if (_intGridValueTiles == null)
            {
                Debug.LogError("Int grid Value Tile Collection is null. Assigned in project asset?", this);
                LDtkProviderErrorIdentifiers.Add(key);
                return null;
            }
            
            Sprite sprite = GetAssetByIdentifier<Sprite>(_intGridValues, key, true);
            
            if (sprite == null)
            {
                return null;
            }

            string nameKey = sprite != null ? sprite.name : MAGIC_DEFAULT_NULL_TILE;
            
            return _intGridValueTiles.GetByName(nameKey);
        }

        private T GetAssetByIdentifier<T>(IEnumerable<ILDtkAsset> input, string key, bool ignoreNullProblem = false) where T : Object
        {
            if (input == null)
            {
                Debug.LogError("LDtk: Tried getting an asset from LDtk project but the array was null. Is the project asset properly saved?", this);
                OnFail();
            }
            
            if (LDtkProviderErrorIdentifiers.Contains(key))
            {
                //this is to help prevent too much log spam. only one mistake from the same identifier get is necessary.
                return default;
            }
            
            foreach (ILDtkAsset asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    Debug.LogError($"LDtk: A field in {name} is null.", this);
                    continue;
                }

                if (asset.Identifier != key)
                {
                    continue;
                }

                if (asset.AssetExists)
                {
                    return (T)asset.Object;
                }

                if (!ignoreNullProblem)
                {
                    Debug.LogError($"LDtk: The asset \"{asset.Identifier}\" was required to build, but wasn't assigned.", asset.Object);
                }
                
                return OnFail();
            }

            Debug.LogError($"LDtk: Could not find any asset with identifier \"{key}\" in \"{name}\". Unassigned in project assets?", this);
            return OnFail();
            
            T OnFail()
            {
                LDtkProviderErrorIdentifiers.Add(key);
                return default;
            }
        }
        
        public Grid GetTilemapPrefab()
        {
            //if override exists, use it. Otherwise use a default. Similar to how unity resolves empty fields like Physics Materials for example.
            return _tilemapPrefab != null ? _tilemapPrefab : Resources.Load<Grid>(GRID_PREFAB_PATH);
        }
    }
}