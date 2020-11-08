using System;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public class LDtkParsedInt : ILDtkParsedValue
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

            Debug.LogError($"Was unable to parse Int for {input}");
            return default;
        }
    }
}