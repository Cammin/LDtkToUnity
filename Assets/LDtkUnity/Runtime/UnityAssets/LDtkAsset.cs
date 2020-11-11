using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets
{
    public abstract class LDtkAsset<T> : ScriptableObject, ILDtkAsset
    {
        [SerializeField] private T _asset = default;

        public string Identifier => name;
        public T Asset => _asset;
    }
}