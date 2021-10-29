using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public static class LDtkFieldParser
    {
        private static LDtkBuilderEntity _builder;
        
        [RuntimeInitializeOnLoadMethod]
        private static void ResetValue()
        {
            _builder = null;
        }

        public static void CacheRecentBuilder(LDtkBuilderEntity builder)
        {
            //builder could be null if we are prepping to set level fields instead of an entity's
            _builder = builder;
        }
        
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
                //never apply post processing to field values if it was a level. the builder would be null in this case
                if (_builder != null && parser is ILDtkPostParser postParser)
                {
                    postParser.SupplyPostProcessorData(_builder, instance);
                }

                return parser.ImportString;
            }

            Debug.LogError($"LDtk: C# type \"{instance.Type}\" is not a parseable LDtk field type.");
            return null;
        }
    }
}