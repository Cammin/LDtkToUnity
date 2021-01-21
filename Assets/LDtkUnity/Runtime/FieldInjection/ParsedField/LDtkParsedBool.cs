using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedBool : ILDtkValueParser
    {
        public string TypeName => "Bool";

        public object ParseValue(object input)
        {
            return Convert.ToBoolean(input);
        }
    }
}