using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public Type Type => typeof(int);
        public Type TypeArray => typeof(int[]);
        public string TypeString => "Int";

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