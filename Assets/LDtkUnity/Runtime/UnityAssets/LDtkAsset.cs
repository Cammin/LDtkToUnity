using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkAsset : ILDtkAsset
    {
        public const string PROP_KEY = nameof(_key);
        public const string PROP_ASSET = nameof(_asset);

        [SerializeField] private string _key = null;
        [SerializeField] private Object _asset = default;

        public LDtkAsset(string key, Object asset)
        {
            _key = key;
            _asset = asset;
        }
        
        public string Identifier => _key;
        public string AssetTypeName => typeof(Object).Name;
        public bool AssetExists => _asset != null;
        public Object Object => _asset;

        public T GetAsset<T>() where T : Object
        {
            if (_asset is T obj)
            {
                return obj;
            }
            
            return null;
        }
    }
}