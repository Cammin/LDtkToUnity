using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public static class LDtkFieldParser
    {
        private static readonly List<ILDtkValueParser> ValueParsers = new List<ILDtkValueParser>
        {
            new LDtkParsedInt(),
            new LDtkParsedFloat(),
            new LDtkParsedBool(),
            
            new LDtkParsedString(),
            new LDtkParsedMultiline(),
            new LDtkParsedFilePath(),
            
            new LDtkParsedColor(),
            new LDtkParsedEnum(),
            new LDtkParsedPoint(),
        };
        
        public static ParseFieldValueAction GetParserMethodForType(FieldInstance instance)
        {
            ILDtkValueParser parser = ValueParsers.FirstOrDefault(p => p.TypeName(instance));
            if (parser != null)
            {
                return parser.ImportString;
            }

            Debug.LogError($"LDtk: C# type \"{instance.Type}\" is not a parseable LDtk field type.");
            return null;
        }
    }
}