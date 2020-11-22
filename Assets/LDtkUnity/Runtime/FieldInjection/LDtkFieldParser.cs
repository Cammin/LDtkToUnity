using System;
using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.FieldInjection.ParsedField;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection
{
    
    public static class LDtkFieldParser
    {
        private static readonly List<ILDtkValueParser> ValueParsers = new List<ILDtkValueParser>
        {
            new LDtkParsedInt(),
            new LDtkParsedFloat(),
            new LDtkParsedBool(),
            new LDtkParsedString(),
            new LDtkParsedEnum(),
            new LDtkParsedColor(),
            new LDtkParsedPoint(),
        };
        
        public static ParseFieldValueAction GetParserMethodForType(Type type)
        {
            //enum check. Enums can be any sort of type, so a specific case in this context
            if (type.IsEnum)
            {
                LDtkParsedEnum parsedEnum = new LDtkParsedEnum();
                parsedEnum.SetEnumType(type);
                return parsedEnum.ParseValue;
            }
            
            ILDtkValueParser parser = ValueParsers.FirstOrDefault(p => p.IsType(type));
            if (parser != null)
            {
                return parser.ParseValue;
            }

            Debug.LogError($"LDtk: Was unable to parse the type of LDtk field type \"{type.Name}\". Is the correct type specified in the field?");
            return null;
        }

    }
}