using System.Collections.Generic;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Settings
{
    [HelpURL(LDtkHelpURL.PROJECT_ASSETS)]
    [CreateAssetMenu(fileName = nameof(LDtkProject), menuName = nameof(LDtkProject), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkProject : ScriptableObject
    {
        public const string PROP_JSON = nameof(_jsonProject);
        public const string PROP_INTGRID = nameof(_intGridValues);
        public const string PROP_ENTITIES = nameof(_entities);
        public const string PROP_TILESETS = nameof(_tilesets);
        public const string PROP_TILEMAP_PREFAB = nameof(_tilemapPrefab);
        public const string PROP_INTGRIDVISIBLE = nameof(_intGridValueColorsVisible);
        
        [SerializeField] private TextAsset _jsonProject;
        [Space]
        [SerializeField] private LDtkLevelIdentifier[] _levels;
        [SerializeField] private LDtkIntGridValueAsset[] _intGridValues;
        [SerializeField] private LDtkEntityAsset[] _entities = null;
        [SerializeField] private LDtkTilesetAsset[] _tilesets = null;
        [Space]
        [SerializeField] private Grid _tilemapPrefab = null;

        //intgrid
        [SerializeField] private bool _intGridValueColorsVisible;

        public Grid TilemapPrefab => _tilemapPrefab;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public TextAsset ProjectJson => _jsonProject;

        public LDtkIntGridValueAsset GetIntGridValue(string identifier) => GetAssetByIdentifier(_intGridValues, identifier);
        public LDtkEntityAsset GetEntity(string identifier) => GetAssetByIdentifier(_entities, identifier);
        public LDtkTilesetAsset GetTileset(string identifier) => GetAssetByIdentifier(_tilesets, identifier);
        
        private T GetAssetByIdentifier<T>(IEnumerable<T> input, string identifier) where T : ILDtkAsset
        {
            if (!Application.isPlaying || LDtkProviderErrorIdentifiers.Contains(identifier))
            {
                //this is to help prevent too much log spam. only one mistake from the same identifier get is necessary.
                return default;
            }
            
            foreach (T asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    Debug.LogError($"LDtk: A field in collection {name} is null.", this);
                    continue;
                }

                if (asset.Identifier != identifier)
                {
                    continue;
                }

                if (asset.AssetExists)
                {
                    return asset;
                }
                
                Debug.LogError($"LDtk: {asset.Identifier}'s {asset.AssetTypeName} asset was not assigned.", asset.Object);
                return OnFail();
            }

            Debug.LogError($"LDtk: Could not find any asset with identifier \"{identifier}\" in \"{name}\". Unassigned in collection or identifier mispelling?", this);
            return OnFail();
            
            T OnFail()
            {
                LDtkProviderErrorIdentifiers.Add(identifier);
                return default;
            }
        }

        public LDtkDataProject GetDeserializedProject()
        {
            return LDtkToolProjectLoader.DeserializeProject(_jsonProject.text);
        }
    }
}