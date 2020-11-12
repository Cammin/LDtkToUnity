using System;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public sealed class LDtkParsedEnum : ILDtkParsedValue
    {
        public Type Type => typeof(Enum);
        public Type TypeArray => typeof(Enum[]);
        public string TypeString => $"LocalEnum";

        public object ParseValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogWarning($"LDtk: Input Enum {Type.Name} included an empty string. Setting as default enum value");
                return default;
            }
            
            string[] tokens = input.Split('.');

            string typeString = "";
            string valueString = "";
            
            try
            {
                typeString = tokens[0];
                valueString = tokens[1];
            }
            catch
            {
                Debug.LogError(input);
            }

            Type enumType = LDtkProviderEnum.GetEnumType(typeString);

            if (enumType == null || !enumType.IsEnum)
            {
                Debug.LogError($"LDtk: Invalid Enum type: {typeString}");
                return default;
            }
            
            return Enum.Parse(enumType, valueString);
        }

        
        
        

    }
}