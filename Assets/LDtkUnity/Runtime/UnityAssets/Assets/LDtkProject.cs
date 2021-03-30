using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

#pragma warning disable 0414

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_PROJECT)]
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
        public const string LEVEL_BACKGROUNDS = nameof(_levelBackgrounds);
        
        
        public const string TILEMAP_PREFABS = nameof(_gridPrefabs);
        
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string LEVEL_FIELDS_PREFAB = nameof(_levelFieldsPrefab);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);
        
        
        [SerializeField] private LDtkProjectFile _jsonProject = null;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private int _pixelsPerUnit = 16;
        
        [SerializeField] private string _enumNamespace = string.Empty;
        [SerializeField] private LDtkTileCollection _intGridValueTiles = null;
        [SerializeField] private Object _enumAssembly = null;
        [SerializeField] private GameObject _levelFieldsPrefab = null;

        [SerializeField] private LDtkAsset[] _levels = null;
        [SerializeField] private LDtkAsset[] _levelBackgrounds = null;
        [SerializeField] private LDtkAsset[] _intGridValues = null;
        [SerializeField] private LDtkAsset[] _entities = null;
        [SerializeField] private LDtkAsset[] _tilesets = null;
        [SerializeField] private LDtkAsset[] _tileCollections = null;
        [SerializeField] private LDtkAsset[] _gridPrefabs = null;

        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public LDtkProjectFile ProjectJson => _jsonProject;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject LevelFieldsPrefab => _levelFieldsPrefab;

        public LDtkLevelFile[] LevelAssets => _levels.Select(p => p.GetAsset<LDtkLevelFile>()).ToArray();

        public LDtkLevelFile GetLevel(string key)
        {
            return GetAssetByIdentifier<LDtkLevelFile>(_levels, key);
        }
        
        public Texture2D GetLevelBackground(string levelName)
        {
            return GetAssetByIdentifier<Texture2D>(_levelBackgrounds, levelName, true);
        }

        public GameObject GetEntity(string key)
        {
            return GetAssetByIdentifier<GameObject>(_entities, key);
        }

        public Texture2D GetTileset(string key)
        {
            return GetAssetByIdentifier<Texture2D>(_tilesets, key);
        }
        
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
            
            return _intGridValueTiles.GetByName(key);
        }

        public Sprite GetIntGridValueSprite(string key)
        {
            return GetAssetByIdentifier<Sprite>(_intGridValues, key, true);
        }
        
        public LDtkTileCollection GetTileCollection(string identifier)
        {
            return GetAssetByIdentifier<LDtkTileCollection>(_tileCollections, identifier);
        }

        public Grid GetTilemapPrefab(string identifier)
        {
            //prefer to get the custom prefab from the specified player first.
            Grid customLayerGridPrefab = GetAssetByIdentifier<Grid>(_gridPrefabs, identifier, true);

            //if override exists, use it. Otherwise use a default. Similar to how unity resolves empty fields like Physics Materials for example.
            return customLayerGridPrefab != null ? customLayerGridPrefab : LDtkResourcesLoader.LoadDefaultGridPrefab();
        }
        
        private T GetAssetByIdentifier<T>(IEnumerable<ILDtkAsset> input, string key, bool ignoreNullProblem = false) where T : Object
        {
            if (input == null)
            {
                Debug.LogError("LDtk: Tried getting an asset from LDtk project but the array was null. Is the project asset properly saved?", this);
                return OnFail();
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
    }
}