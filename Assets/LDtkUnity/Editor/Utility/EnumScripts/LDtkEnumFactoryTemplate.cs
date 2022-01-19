using System.Linq;

namespace LDtkUnity.Editor
{
    internal readonly struct LDtkEnumFactoryTemplate
    {
        public readonly string Definition;
        public readonly string[] Values;
        
        private LDtkEnumFactoryTemplate(string definition, string[] values)
        {
            Definition = definition;
            Values = values;
        }
        
        public static LDtkEnumFactoryTemplate FromDefinition(EnumDefinition definition)
        {
            string[] values = definition.Values.Select(value => value.Id).ToArray();
            return new LDtkEnumFactoryTemplate(definition.Identifier, values);
        }
    }
}