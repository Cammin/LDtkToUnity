using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedFloat : ILDtkValueParser
    {
        public string TypeName => "Float";
        
        public object ParseValue(object input)
        {
            return (float) input;


            /*if (float.TryParse(input, out float value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Float for {input}", LDtkInjectionErrorContext.Context);
            return default;*/
        }
    }
}