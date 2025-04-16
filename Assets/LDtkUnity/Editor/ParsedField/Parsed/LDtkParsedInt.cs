using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedInt : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;
        
        public object ImportString(LDtkFieldParseContext ctx)
        {
            object input = ctx.Input;
            //ints can be legally null
            if (input == null)
            {
                return default;
            }

            if (!int.TryParse(input.ToString(), out int value))
            {
                return default;
            }
            
            if (_process != null)
            {
                value = (int)_process.Postprocess(value);
            }
            
            //a required cast to fix an issue with null ints
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldDefinition def)
        {
            float scale = builder.LayerScale;
            _process = new LDtkPostParserNumber(scale, def.EditorDisplayMode);
        }
    }
}