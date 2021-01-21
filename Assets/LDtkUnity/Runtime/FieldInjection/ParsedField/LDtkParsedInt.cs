using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedInt : ILDtkValueParser
    {
        public string TypeName => "Int";
        public object ParseValue(object input)
        {
            return Convert.ToInt32(input);
        }
    }
}