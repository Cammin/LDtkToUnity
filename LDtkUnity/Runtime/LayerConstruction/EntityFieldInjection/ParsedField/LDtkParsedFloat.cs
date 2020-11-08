using System;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public class LDtkParsedFloat : ILDtkParsedValue
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

            Debug.LogError($"Was unable to parse Float for {input}");
            return default;
        }
    }
}