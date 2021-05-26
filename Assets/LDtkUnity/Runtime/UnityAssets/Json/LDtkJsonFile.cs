using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkJsonFile<T> : ScriptableObject, ILDtkJsonFile
    {
        [SerializeField] protected string _json; 

        public abstract T FromJson { get; }

        public virtual void SetJson(string json)
        {
            _json = json;
        }
    }
}