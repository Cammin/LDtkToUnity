using System.Collections.Generic;

namespace LDtkUnity
{
    internal abstract class LDtkDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        protected abstract TKey GetKeyFromValue(TValue value);
        
        protected void TryAdd(IEnumerable<TValue> values)
        {
            if (values == null) //most likely it was a separate level with no layers
            {
                return;
            }
            
            foreach (TValue value in values)
            {
                TKey key = GetKeyFromValue(value);

                if (key == null)
                {
                    LDtkDebug.LogError("Tried adding a null key");
                    continue;
                }
                
                if (ContainsKey(key))
                {
                    LDtkDebug.LogError($"{typeof(TValue).Name} database already has a {typeof(TKey).Name} entry for {key}");
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
            
            LDtkDebug.LogError($"{typeof(TKey).Name} Dictionary<{typeof(TValue).Name}> does not contain a key for \"{key}\"");
            return default;
        }
    }
}