using System.Collections.Generic;
using UnityEngine;

namespace LDtkUnity
{
    internal abstract class LDtkDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public abstract void CacheAllData(LdtkJson json);
        protected abstract TKey GetKeyFromValue(TValue value);
        
        protected void TryAdd(IEnumerable<TValue> values)
        {
            if (values == null)
            {
                LDtkDebug.LogError($"{typeof(TValue).Name} database tried to add a collection but was null");
                return;
            }
            
            foreach (TValue value in values)
            {
                TKey key = GetKeyFromValue(value);

                if (key == null)
                {
                    Debug.LogError("LDtk: Tried adding a null key");
                    continue;
                }
                
                if (ContainsKey(key))
                {
                    Debug.LogError($"LDtk: {typeof(TValue).Name} database already has a {typeof(TKey).Name} entry for {key}");
                    continue;
                }
                
                Add(key, value);
            }
        }

        public TValue TryGet(TKey key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }
            
            Debug.LogError($"LDtk: {nameof(LDtkUidBank)} Dictionary<{typeof(TValue).Name}> does not contain a key UID for \"{key}\"");
            return default;
        }
    }
}