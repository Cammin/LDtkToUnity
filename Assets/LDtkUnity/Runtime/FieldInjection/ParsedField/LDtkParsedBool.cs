using System;

namespace LDtkUnity
{
    public class LDtkParsedBool : ILDtkValueParser
    {
        public string TypeName => "Bool";

        public object ParseValue(object input)
        {
            return Convert.ToBoolean(input);
        }
    }
}