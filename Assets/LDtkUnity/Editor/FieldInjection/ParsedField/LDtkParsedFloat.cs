using System;

namespace LDtkUnity.Editor
{
    public class LDtkParsedFloat : ILDtkValueParser
    {
        public string TypeName => "Float";
        
        public object ParseValue(object input)
        {
            //floats can be legally null
            if (input == null)
            {
                return 0f;
            }
            
            return Convert.ToSingle(input);
        }
    }
}