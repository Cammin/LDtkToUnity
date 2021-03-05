using System;

namespace LDtkUnity
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public string TypeName => "Int";
        public object ParseValue(object input)
        {
            return Convert.ToInt32(input);
        }
    }
}