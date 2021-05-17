using System;

namespace LDtkUnity.Editor
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsInt;

        public object ImportString(object input)
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