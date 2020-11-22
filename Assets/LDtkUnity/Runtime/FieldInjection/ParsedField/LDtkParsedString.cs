using System;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedString : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(string);
        
        public object ParseValue(string input)
        {
            //this is to correct the formatting for a Newline in Unity
            string properText = input.Replace("\\n", "\n");
            
            return properText;
        }
    }
}