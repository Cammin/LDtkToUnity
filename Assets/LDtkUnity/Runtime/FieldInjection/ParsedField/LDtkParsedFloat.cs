using System;

namespace LDtkUnity.FieldInjection
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