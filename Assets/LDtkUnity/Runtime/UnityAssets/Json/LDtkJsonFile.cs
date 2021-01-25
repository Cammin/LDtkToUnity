using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    public abstract class LDtkJsonFile<T> : ScriptableObject, ILDtkJsonFile
    {
        [SerializeField, HideInInspector] protected string _json;
        
        public string DataPropName => nameof(_json);

        public abstract T FromJson { get; }

        public void SetJson(string json)
        {
            _json = json;
        }

    }
}