using System;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedColor : ILDtkValueParser
    {
        public string TypeName => "Color";
        
        public bool IsType(Type triedType) => triedType == typeof(Color);
        public object ParseValue(object input)
        {
            string colorString = (string) input;
            
            return colorString.ToColor();
        }
    }
}