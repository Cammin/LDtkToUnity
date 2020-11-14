using System.Collections.Generic;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets
{
    public abstract class LDtkAssetCollection<T> : ScriptableObject where T : ILDtkAsset
    {
        [SerializeField] private List<T> _includedAssets = default;

        //todo put this in docs
        //LDtkAssetCollections are completely centered around associating with the scriptable object's identifiers and the uniquely named identifier from within the LDtk editor. //todo put this text in the doumcentation
        
        public T GetAssetByIdentifier(string identifier)
        {
            if (LDtkProviderErrorIdentifiers.Contains(identifier))
            {
                //this is to help prevent too much log spam. only one mistake from the same identifier get is necessary.
                return default;
            }
            
            T OnFail()
            {
                LDtkProviderErrorIdentifiers.Add(identifier);
                return default;
            }
            
            foreach (T asset in _includedAssets)
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
        }
    }
}