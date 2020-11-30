using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using LDtkUnity.Runtime.UnityAssets.Entity;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Settings
{
    [HelpURL(LDtkHelpURL.PROJECT_ASSETS)]
    public class LDtkProject : ScriptableObject
    {
        public TextAsset _jsonProject;
        [Space]
        public LDtkIntGridValueAsset[] _intGridValues;
        public LDtkEntityAsset[] _entities = null;
        public LDtkTilesetAsset[] _tilesets = null;
        [Space]
        public Grid _tilemapPrefab = null;


        public LDtkIntGridValueAsset GetIntGridValue(string identifier) => GetAssetByIdentifier(_intGridValues, identifier);
        public LDtkEntityAsset GetEntity(string identifier) => GetAssetByIdentifier(_entities, identifier);
        public LDtkTilesetAsset GetTileset(string identifier) => GetAssetByIdentifier(_tilesets, identifier);
        
        private T GetAssetByIdentifier<T>(T[] input, string identifier) where T : ILDtkAsset
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
    }
}