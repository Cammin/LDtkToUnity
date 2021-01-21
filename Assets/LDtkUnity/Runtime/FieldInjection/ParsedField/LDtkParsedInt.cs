using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public string TypeName => "Int";
        public object ParseValue(object input)
        {
            return (int) input;

            /*if (int.TryParse(input, out int value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Int for {input}. Is the correct type specified?", LDtkInjectionErrorContext.Context);
            return default;*/
        }
    }
}