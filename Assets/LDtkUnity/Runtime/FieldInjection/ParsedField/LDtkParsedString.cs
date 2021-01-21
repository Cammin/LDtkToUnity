using System;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedString : ILDtkValueParser
    {
        public string TypeName => "String";

        public object ParseValue(object input)
        {
            string stringInput = (string) input;
            
            //this is to correct the formatting for a Newline in Unity
            string properText = stringInput.Replace("\\n", "\n");
            
            return properText;
        }
    }
}