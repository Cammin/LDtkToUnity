using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(int);
        public object ParseValue(string input)
        {
            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Int for {input}. Is the correct type specified?");
            return default;
        }
    }
}