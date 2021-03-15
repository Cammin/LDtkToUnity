using UnityEngine;

namespace LDtkUnity
{
    public abstract class LDtkJsonFile<T> : ScriptableObject, ILDtkJsonFile
    {
        [SerializeField, HideInInspector] protected string _json;

        public abstract T FromJson { get; }

        public virtual void SetJson(string json)
        {
            _json = json;
        }
    }
}