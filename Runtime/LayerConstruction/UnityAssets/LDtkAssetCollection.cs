using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets
{
    public abstract class LDtkAssetCollection<T> : ScriptableObject where T : ILDtkAsset
    {
        [ReorderableList, SerializeField] private List<T> _includedAssets = default;

        public List<T> IncludedAssets => _includedAssets;

        public T GetAssetByIdentifier(string identifier)
        {
            bool ContainsMatch(T asset) => asset.Identifier == identifier;
            return _includedAssets.Any(ContainsMatch) ? _includedAssets.First(ContainsMatch): default;
        }
    }
}