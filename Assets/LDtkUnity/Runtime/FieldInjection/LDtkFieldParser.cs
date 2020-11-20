using System;
using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.FieldInjection.ParsedField;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection
{
    public delegate object ParseFieldValueAction(string input);
    public static class LDtkFieldParser
    {
        private static readonly List<ILDtkParsedValue> ParsedTypes = new List<ILDtkParsedValue>
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

            ILDtkParsedValue parsedType = ParsedTypes.FirstOrDefault(p => typeName.Contains(p.TypeString));
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

            ILDtkParsedValue parsedType = null;
            foreach (ILDtkParsedValue p in ParsedTypes)
            {
                if (type == p.Type || type == p.TypeArray)
                {
                    parsedType = p;
                    break;
                }
            }

            if (parsedType != null)
            {
                return parsedType.ParseValue;
            }

            Debug.LogError($"LDtk: Was unable to parse the type of LDtk field type \"{type.Name}\". Is the correct type specified in the field?");
            return null;
        }

    }
}