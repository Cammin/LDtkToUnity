using System;

namespace LDtkUnity.Runtime.FieldInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LDtkFieldAttribute : Attribute
    {
        public readonly string DataIdentifier;

        public bool IsCustomDefinedName => DataIdentifier != null;
        
        public LDtkFieldAttribute(string name = null)
        {
            DataIdentifier = name;
        }
    }
}