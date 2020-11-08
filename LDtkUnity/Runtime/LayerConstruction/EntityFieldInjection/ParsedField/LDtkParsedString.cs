using System;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public class LDtkParsedString : ILDtkParsedValue
    {
        public Type Type => typeof(string);
        public Type TypeArray => typeof(string[]);
        public string TypeString => "String";

        public object ParseValue(string input)
        {
            return input;
        }
    }
}