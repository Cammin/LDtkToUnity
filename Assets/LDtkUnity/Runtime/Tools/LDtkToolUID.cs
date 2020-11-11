using System.Linq;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkToolUid
    {
        public delegate int UidGetter<in T>(T item);
        public static T GetDefinitionByUid<T>(int uid, T[] collection, UidGetter<T> getter)
        {
            bool ContainsID(T item) => getter.Invoke(item) == uid;
            
            if (collection.Any(ContainsID))
            {
                return collection.First(ContainsID);
            }
            Debug.LogError($"Could not get {typeof(T).Name}, uid {uid}");
            return default;
        }
    }
}