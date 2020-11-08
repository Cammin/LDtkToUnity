using NaughtyAttributes;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets
{
    public abstract class LDtkAsset<T> : ScriptableObject, ILDtkAsset
    {
        [SerializeField, ShowAssetPreview] private T _asset = default;

        public string Identifier => name;
        public T Asset => _asset;
    }
}