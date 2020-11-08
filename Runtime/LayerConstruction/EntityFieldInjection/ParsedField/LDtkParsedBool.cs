using System;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public class LDtkParsedBool : ILDtkParsedValue
    {
        public Type Type => typeof(bool);
        public Type TypeArray => typeof(bool[]);
        public string TypeString => "Bool";

        public object ParseValue(string input)
        {
            if (bool.TryParse(input, out bool value))
            {
                return value;
            }

            Debug.LogError($"Was unable to parse Bool for {input}");
            return default;
        }
    }
}