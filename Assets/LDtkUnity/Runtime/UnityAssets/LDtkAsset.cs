using System;
using UnityEngine;
using UnityEngine.Internal;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public abstract class LDtkAsset<T> : ILDtkAsset where T : Object
    {
        public const string PROP_KEY = nameof(_key);
        public const string PROP_ASSET = nameof(_asset);

        [SerializeField] private string _key = null;
        [SerializeField] private LazyLoadReference<T> _asset = default;

        public string Key => _key;
        public Object Asset => _asset.asset;
        protected LDtkAsset(string key, T asset)
        {
            _key = key;
            _asset = new LazyLoadReference<T>
            {
                asset = asset
            };
        }
    }
}