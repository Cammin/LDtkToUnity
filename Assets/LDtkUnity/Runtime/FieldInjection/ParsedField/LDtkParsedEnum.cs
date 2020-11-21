using System;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public sealed class LDtkParsedEnum : ILDtkValueParser
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

            string ldtkType = "";
            string ldtkValue = "";
            
            try
            {
                ldtkType = tokens[0];
                ldtkValue = tokens[1];
            }
            catch
            {
                Debug.LogError($"LDtk: Invalid Enum parse of: {input}");
            }

            Type enumType = LDtkProviderEnum.GetEnumFromLDtkIdentifier(ldtkType);

            if (enumType != null && enumType.IsEnum)
            {
                return Enum.Parse(enumType, ldtkValue);
            }
            
            Debug.LogError($"LDtk: Invalid Enum type: {ldtkType}");
            return default;

        }
    }
}