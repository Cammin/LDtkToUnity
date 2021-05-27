using System;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkParsedFloat : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFloat;

        public object ImportString(object input)
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