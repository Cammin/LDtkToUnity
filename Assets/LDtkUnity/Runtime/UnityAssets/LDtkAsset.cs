using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.UnityAssets
{
    [Serializable]
    public abstract class LDtkAsset<T> : ILDtkAsset where T : Object
    {
        public const string PROP_KEY = nameof(_key);
        public const string PROP_ASSET = nameof(_asset);

        [SerializeField] protected string _key = null;
        [SerializeField] protected T _asset = default;

        protected LDtkAsset(string key, T asset)
        {
            _key = key;
            _asset = asset;
        }
        
        public string Identifier => _key;
        public string AssetTypeName => typeof(T).Name;
        public bool AssetExists => _asset != null;
        public Object Object => _asset;

        public T ReferencedAsset
        {
            get => _asset;
            set => _asset = value;
        }
    }
}