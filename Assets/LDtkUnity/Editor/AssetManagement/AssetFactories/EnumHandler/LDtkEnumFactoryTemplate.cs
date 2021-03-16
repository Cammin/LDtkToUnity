using System.Linq;

namespace LDtkUnity.Editor
{
    public readonly struct LDtkEnumFactoryTemplate
    {
        public readonly string EnumType;
        public readonly string[] Values;
        
        private LDtkEnumFactoryTemplate(string enumType, string[] values)
        {
            EnumType = enumType;
            Values = values;
        }
        
        public static LDtkEnumFactoryTemplate FromDefinition(EnumDefinition definition)
        {
            string[] values = definition.Values.Select(value => value.Id).ToArray();
            return new LDtkEnumFactoryTemplate(definition.Identifier, values);
        }
    }
}