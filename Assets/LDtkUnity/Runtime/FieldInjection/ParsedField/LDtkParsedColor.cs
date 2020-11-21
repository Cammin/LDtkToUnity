using System;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedColor : ILDtkValueParser
    {
        public Type Type => typeof(Color);
        public Type TypeArray => typeof(Color[]);
        public string TypeString => "Color";

        public object ParseValue(string input)
        {
            return input.ToColor();
        }
    }
}