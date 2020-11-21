using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedBool : ILDtkValueParser
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

            Debug.LogError($"LDtk: Was unable to parse Bool for {input}");
            return default;
        }
    }
}