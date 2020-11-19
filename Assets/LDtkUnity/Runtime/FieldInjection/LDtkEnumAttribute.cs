using System;

namespace LDtkUnity.Runtime.FieldInjection
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class LDtkEnumAttribute : Attribute
    {
        public readonly string EnumIdentifier;

        public bool IsCustomDefinedName => EnumIdentifier != null;
        
        public LDtkEnumAttribute(string name = null)
        {
            EnumIdentifier = name;
        }
    }
}
