using System;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedColor : ILDtkParsedValue
    {
        public Type Type => typeof(Color);
        public Type TypeArray => typeof(Color[]);
        public string TypeString => "Color";

        public object ParseValue(string input)
        {
            if (ColorUtility.TryParseHtmlString(input, out Color color))
            {
                return color;
            }
            Debug.LogError($"Was unable to parse Color for {input}");
            return default;
        }
    }
}