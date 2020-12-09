using UnityEngine;

namespace LDtkUnity.FieldInjection
{
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