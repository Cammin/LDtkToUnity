using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkJsonFile<T> : ScriptableObject, ILDtkJsonFile
    {
        [SerializeField] protected byte[] _json; 

        public abstract T FromJson { get; }

        public virtual void SetJson(byte[] json)
        {
            _json = json;
        }
    }
}