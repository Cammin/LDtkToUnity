using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets
{
    public abstract class LDtkAsset<T> : ScriptableObject, ILDtkAsset where T : Object
    {
        [SerializeField] private T _asset = default;

        
        
        public string Identifier => name;
        public string AssetTypeName => typeof(T).Name;
        public bool AssetExists => _asset != null;
        public Object Object => this;

        public T ReferencedAsset
        {
            get => _asset;
            set => _asset = value;
        }
    }
}