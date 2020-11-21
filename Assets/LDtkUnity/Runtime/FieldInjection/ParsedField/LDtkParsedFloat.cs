using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedFloat : ILDtkValueParser
    {
        public Type Type => typeof(float);
        public Type TypeArray => typeof(float[]);
        public string TypeString => "Float";

        public object ParseValue(string input)
        {
            if (float.TryParse(input, out float value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Float for {input}");
            return default;
        }
    }
}