using System;
using UnityEngine;

namespace LDtkUnity
{
    public sealed class LDtkParsedEnum : ILDtkValueParser
    {
        private Type _enumType;
        
        public string TypeName => "LocalEnum";

        public void SetEnumType(Type type)
        {
            if (!type.IsEnum)
            {
                Debug.LogError("Trying to set a non-enum as it's stored type in LDtkParsedEnum", LDtkInjectionErrorContext.Context);
            }
            _enumType = type;
        }
        
        public object ParseValue(object input)
        {
            string stringInput = (string) input;
            
            //enums are able to be legally null
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }

            //give enum value an underscore if a space was in the LDtk definition
            stringInput = stringInput.Replace(' ', '_');
            
            if (_enumType != null && _enumType.IsEnum)
            {
                if (Enum.IsDefined(_enumType, stringInput))
                {
                    return Enum.Parse(_enumType, stringInput);
                }
                
                Debug.LogError($"LDtk: C# enum \"{_enumType.Name}\" does not contain an LDtk enum value identifier \"{stringInput}\". Name mismatch, misspelling, or undefined in LDtk editor?", LDtkInjectionErrorContext.Context);
                return default;

            }
            
            Debug.LogError($"LDtk: Invalid enum parse for enum value: {stringInput}", LDtkInjectionErrorContext.Context);
            return default;

        }
    }
}