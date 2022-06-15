using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkFieldParser
    {
        private static LDtkBuilderEntity _builder;
        private static List<ILDtkValueParser> _parsers;

        [RuntimeInitializeOnLoadMethod]
        private static void ResetValue()
        {
            _builder = null;
            _parsers = null;
        }

        public static void CacheRecentBuilder(LDtkBuilderEntity builder)
        {
            //builder could be null if we are prepping to set level fields instead of an entity's
            _builder = builder;
        }

        public static ParseFieldValueAction GetParserMethodForType(FieldInstance instance)
        {
            GetParserInstances();

            ILDtkValueParser parser = _parsers.FirstOrDefault(p => p.TypeName(instance));
            
            if (parser != null)
            {
                //never apply post processing to field values if it was a level. the builder would be null in this case
                if (_builder != null && parser is ILDtkPostParser postParser)
                {
                    postParser.SupplyPostProcessorData(_builder, instance);
                }

                //Debug.Log($"Parse FieldInstance {instance.Identifier}");
                return parser.ImportString;
            }

            LDtkDebug.LogError($"C# type \"{instance.Type}\" is not a parseable LDtk field type.");
            return null;
        }

        private static void GetParserInstances()
        {
            if (_parsers == null)
            {
                TypeCache.TypeCollection parserTypes = TypeCache.GetTypesDerivedFrom<ILDtkValueParser>();
                _parsers = parserTypes.Select(Activator.CreateInstance).Cast<ILDtkValueParser>().ToList();
            }
        }
    }
}