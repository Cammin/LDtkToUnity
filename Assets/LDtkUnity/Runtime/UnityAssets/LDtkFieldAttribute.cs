using System;
using UnityEngine;

namespace LDtkUnity
{
    [Obsolete("Delete this; No longer used. Data is provided from the LDtkFields component instead, which is added to entities during the import process.")]
    public class LDtkFieldAttribute : PropertyAttribute
    {
        public readonly string DataIdentifier;

        public bool IsCustomDefinedName => DataIdentifier != null;
        
        public LDtkFieldAttribute() {}
        public LDtkFieldAttribute(string name)
        {
            DataIdentifier = name;
        }
    }
}