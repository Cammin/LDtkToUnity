using System;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LDtkInjectableFieldAttribute : Attribute
    {
        public readonly string DataIdentifier;

        public bool IsCustomDefinedName => DataIdentifier != null;
        
        public LDtkInjectableFieldAttribute(string dataIdentifier = null)
        {
            DataIdentifier = dataIdentifier;
        }
    }
}