using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    
    public static class LDtkFieldParser
    {
        private static readonly List<ILDtkValueParser> ValueParsers = new List<ILDtkValueParser>
        {
            new LDtkParsedInt(),
            new LDtkParsedFloat(),
            new LDtkParsedBool(),
            new LDtkParsedString(),
            new LDtkParsedColor(),
            new LDtkParsedPoint(),
            new LDtkParsedFilePath(),
        };
        
        public static ParseFieldValueAction GetParserMethodForType(string typeName)
        {
            ILDtkValueParser parser = ValueParsers.FirstOrDefault(p => p.TypeName.Contains(typeName));
            if (parser != null)
            {
                return parser.ParseValue;
            }

            Debug.LogError($"LDtk: C# type \"{typeName}\" is not a parseable LDtk field type.", LDtkInjectionErrorContext.Context);
            return null;
        }

        public static ParseFieldValueAction GetEnumMethod(Type type)
        {
            LDtkParsedEnum parsedEnum = new LDtkParsedEnum();
            parsedEnum.SetEnumType(type);
            return parsedEnum.ParseValue;
        }

    }
}