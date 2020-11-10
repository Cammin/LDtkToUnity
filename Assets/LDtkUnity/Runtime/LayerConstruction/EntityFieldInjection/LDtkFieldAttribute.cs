using System;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LDtkFieldAttribute : Attribute
    {
        public readonly string DataIdentifier;

        public bool IsCustomDefinedName => DataIdentifier != null;
        
        public LDtkFieldAttribute(string dataIdentifier = null)
        {
            DataIdentifier = dataIdentifier;
        }
    }
}