using System;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.FieldInjection
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