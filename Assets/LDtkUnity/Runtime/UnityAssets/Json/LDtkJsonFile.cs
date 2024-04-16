using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkJsonFile<T> : ScriptableObject, ILDtkJsonFile //todo we really need to make this a TextAsset
    {
        [SerializeField] protected string _json; 

        public abstract T FromJson { get; }

        public virtual void SetJson(string json)
        {
            _json = json;
        }
    }
}