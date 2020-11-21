using System;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedString : ILDtkValueParser
    {
        public Type Type => typeof(string);
        public Type TypeArray => typeof(string[]);
        public string TypeString => "String";

        public object ParseValue(string input)
        {
            //this is to correct the formatting for a Newline in Unity
            string properText = input.Replace("\\n", "\n");
            
            return properText;
        }
    }
}