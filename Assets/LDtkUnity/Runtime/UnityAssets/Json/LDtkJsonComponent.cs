using UnityEngine;

namespace LDtkUnity
{
    public abstract class LDtkJsonComponent<T> : MonoBehaviour, ILDtkJsonFile
    {
        [SerializeField, HideInInspector] protected string _json; //todo this should not be cached as it takes up memory. find a way to reference the actual file, and then read it. 

        public abstract T FromJson { get; }

        public virtual void SetJson(string json)
        {
            _json = json;
        }
    }
}