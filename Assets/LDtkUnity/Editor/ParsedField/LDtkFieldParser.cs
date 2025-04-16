using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    internal static class LDtkFieldParser
    {
        private static LDtkBuilderEntity _builder;
        private static Dictionary<string, ILDtkValueParser> _parsers;

        //todo reinstate attribute if we add runtime building
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
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

        public static ParseFieldValueAction GetParserMethodForType(FieldDefinition def)
        {
            GetParserInstances();

            if (!_parsers.TryGetValue(def.Type, out ILDtkValueParser parser))
            {
                //maybe it was an enum that isn't based in hardcoded string
                if (def.IsEnum)
                {
                    parser = _parsers["LocalEnum"];
                }
                else
                {
                    LDtkDebug.LogError($"FieldParser doesnt contain a key for \"{def.Type}\".");
                    return null;
                }
            }
            
            //never apply post-processing to field values if it was a level. the builder would be null in this case
            if (_builder != null && parser is ILDtkPostParser postParser)
            {
                postParser.SupplyPostProcessorData(_builder, def);
            }

            //Debug.Log($"Parse FieldInstance {instance.Identifier}");
            return parser.ImportString;
        }
        
        private static void GetParserInstances()
        {
            if (_parsers == null)
            {
                _parsers = new Dictionary<string, ILDtkValueParser>
                {
                    { "Int", new LDtkParsedInt() },
                    { "Array<Int>", new LDtkParsedInt() },
                    
                    { "Float", new LDtkParsedFloat() },
                    { "Array<Float>", new LDtkParsedFloat() },
                    
                    { "Bool", new LDtkParsedBool() },
                    { "Array<Bool>", new LDtkParsedBool() },
                    
                    { "String", new LDtkParsedString() },
                    { "Array<String>", new LDtkParsedString() },
                    
                    { "Multilines", new LDtkParsedMultiline() },
                    { "Array<Multilines>", new LDtkParsedMultiline() },
                    
                    { "FilePath", new LDtkParsedFilePath() },
                    { "Array<FilePath>", new LDtkParsedFilePath() },
                    
                    { "Color", new LDtkParsedColor() },
                    { "Array<Color>", new LDtkParsedColor() },
                    
                    //only need the one because enums are an exception to the rule
                    { "LocalEnum", new LDtkParsedEnum() },
                    
                    { "Tile", new LDtkParsedTile() },
                    { "Array<Tile>", new LDtkParsedTile() },
                    
                    { "EntityRef", new LDtkParsedEntityRef() },
                    { "Array<EntityRef>", new LDtkParsedEntityRef() },
                    
                    { "Point", new LDtkParsedPoint() },
                    { "Array<Point>", new LDtkParsedPoint() }
                };
            }
        }
    }
}