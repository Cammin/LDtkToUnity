using System;

namespace LDtkUnity
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public string TypeName => "Int";
        public object ParseValue(object input)
        {
            //ints can be legally null
            if (input == null)
            {
                return 0f;
            }
            
            return Convert.ToInt32(input);
        }
    }
}