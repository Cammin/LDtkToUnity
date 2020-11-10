using System;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public sealed class LDtkParsedEnum : ILDtkParsedValue
    {
        public Type Type => typeof(Enum);
        public Type TypeArray => typeof(Enum[]);
        public string TypeString => $"LocalEnum";

        public object ParseValue(string input)
        {
            //Debug.Log($"Parse Enum with input: \"{input}\"");
            
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogWarning($"Input Enum {Type.Name} included an empty string. Setting as default enum value");
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
                Debug.LogError($"Invalid Enum type: {typeString}");
                return default;
            }
            
            return Enum.Parse(enumType, valueString);

            //Debug.LogError($"Was unable to parse Enum '{typeof(T).Name}' for {input}");
        }

        
        
        

    }
}