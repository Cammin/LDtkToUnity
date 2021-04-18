using UnityEngine;

namespace LDtkUnity
{
    public abstract class LDtkJsonComponent<T> : MonoBehaviour
    {
        public abstract T FromJson();
    }
}