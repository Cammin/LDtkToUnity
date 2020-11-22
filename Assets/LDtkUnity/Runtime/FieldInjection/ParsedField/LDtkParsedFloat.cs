using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedFloat : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(float);
        public object ParseValue(string input)
        {
            if (float.TryParse(input, out float value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Float for {input}", LDtkInjectionErrorContext.Context);
            return default;
        }
    }
}