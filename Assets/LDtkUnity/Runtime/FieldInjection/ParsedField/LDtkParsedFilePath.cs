using System;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedFilePath : ILDtkValueParser
    {
        public string TypeName => "FilePath";

        public object ParseValue(object input)
        {
            string stringInput = (string) input;
            
            return stringInput;
            
            
            /*if (bool.TryParse(input, out bool value))
            {
                return value;
            }

            Debug.LogError($"LDtk: Was unable to parse Bool for {input}", LDtkInjectionErrorContext.Context);
            return default;*/
        }

        
    }
}