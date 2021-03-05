using System;

namespace LDtkUnity
{
    public class LDtkParsedFloat : ILDtkValueParser
    {
        public string TypeName => "Float";
        
        public object ParseValue(object input)
        {
            return Convert.ToSingle(input);
        }
    }
}