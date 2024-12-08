using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Purely a class to hold the LDtkConfigData as an import artifact
    /// </summary>
    internal sealed class LDtkConfig : ScriptableObject
    {
        public LDtkConfigData _data;
    }
}