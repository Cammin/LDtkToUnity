using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkDefinitionObject : ScriptableObject
    {
        internal abstract void SetAssetName();
    }
    
    [ExcludeFromDocs]
    public abstract class LDtkDefinitionObject<T> : LDtkDefinitionObject
    {
        internal abstract void Populate(LDtkDefinitionObjectsCache cache, T def);
    }
}