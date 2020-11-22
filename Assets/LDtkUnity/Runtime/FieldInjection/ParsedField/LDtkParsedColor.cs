using System;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedColor : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(Color);
        public object ParseValue(string input)
        {
            return input.ToColor();
        }
    }
}