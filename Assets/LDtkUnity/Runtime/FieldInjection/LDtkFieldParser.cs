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
        
        public static Type GetParsedFieldType(string typeName)
        {
            bool isArray = typeName.Contains("Array");
            bool isEnum = typeName.Contains("LocalEnum");

            ILDtkValueParser parsedType = ValueParsers.FirstOrDefault(p => typeName.Contains(p.TypeString));
            if (parsedType != null)
            {
                return isArray ? parsedType.TypeArray : parsedType.Type;
            }

            Debug.LogError($"LDtk: Was unable to parse the type of LDtk instance layer type \"{typeName}\"");
            return null;
        }

        public static ParseFieldValueAction GetParserMethodForType(Type type)
        {
            if (type.IsEnum || (type.IsArray && type.GetElementType().IsEnum))
            {
                return new LDtkParsedEnum().ParseValue;
            }

            ILDtkValueParser parser = ValueParsers.FirstOrDefault(p => type == p.Type || type == p.TypeArray);
            if (parser != null)
            {
                return parser.ParseValue;
            }

            Debug.LogError($"LDtk: Was unable to parse the type of LDtk field type \"{type.Name}\". Is the correct type specified in the field?");
            return null;
        }

    }
}