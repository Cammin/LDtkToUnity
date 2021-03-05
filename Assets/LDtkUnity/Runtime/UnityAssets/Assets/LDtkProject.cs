using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
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
        public const string ENTITIES = nameof(_entities);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        public const string ENUM_ASSEMBLY = nameof(_enumAssembly);
        public const string TILESETS = nameof(_tilesets);
        public const string TILEMAP_PREFAB = nameof(_tilemapPrefab);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        
        private const string GRID_PREFAB_PATH = "LDtkDefaultGrid";
        
        [SerializeField] private LDtkProjectFile _jsonProject = null;
        [SerializeField] private Grid _tilemapPrefab = null;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private int _pixelsPerUnit = 16;
        [SerializeField] private string _enumNamespace = string.Empty;
        [SerializeField] private AssemblyDefinitionAsset _enumAssembly = null;
        [SerializeField] private LDtkAsset[] _levels = null;
        [SerializeField] private LDtkAsset[] _intGridValues = null;
        [SerializeField] private LDtkAsset[] _entities = null;
        [SerializeField] private LDtkAsset[] _tilesets = null;
        [SerializeField] private LDtkAsset[] _metaTiles = null;


        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public int PixelsPerUnit => _pixelsPerUnit;
        public LDtkProjectFile ProjectJson => _jsonProject;

        public LDtkLevelFile[] LevelAssets => _levels.Select(p => p.GetAsset<LDtkLevelFile>()).ToArray();

        public LDtkLevelFile GetLevel(string identifier) => GetAssetByIdentifier<LDtkLevelFile>(_levels, identifier);
        public Sprite GetIntGridValue(string identifier) => GetAssetByIdentifier<Sprite>(_intGridValues, identifier);
        public GameObject GetEntity(string identifier) => GetAssetByIdentifier<GameObject>(_entities, identifier);
        public Texture2D GetTileset(string identifier) => GetAssetByIdentifier<Texture2D>(_tilesets, identifier);
        public Tile GetMetaTile(string identifier) => GetAssetByIdentifier<Tile>(_metaTiles, identifier);

        private T GetAssetByIdentifier<T>(IEnumerable<ILDtkAsset> input, string identifier) where T : Object
        {
            if (LDtkProviderErrorIdentifiers.Contains(identifier))
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

                if (asset.Identifier != identifier)
                {
                    continue;
                }

                if (asset.AssetExists)
                {
                    return (T)asset.Object;
                }
                
                Debug.LogError($"LDtk: {asset.Identifier}'s {asset.AssetTypeName} asset was not assigned.", asset.Object);
                return OnFail();
            }

            Debug.LogError($"LDtk: Could not find any asset with identifier \"{identifier}\" in \"{name}\". Unassigned in project assets or identifier misspelling?", this);
            return OnFail();
            
            T OnFail()
            {
                LDtkProviderErrorIdentifiers.Add(identifier);
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