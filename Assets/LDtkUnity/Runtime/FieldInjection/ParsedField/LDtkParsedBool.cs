using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedBool : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(bool);

        public object ParseValue(string input)
        {
            if (bool.TryParse(input, out bool value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Bool for {input}", LDtkInjectionErrorContext.Context);
            return default;
        }
    }
}