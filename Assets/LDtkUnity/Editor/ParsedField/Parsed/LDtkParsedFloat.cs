using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedFloat : ILDtkValueParser, ILDtkPostParser
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

            if (!float.TryParse(input.ToString(), out float value))
            {
                return default;
            }

            if (_process != null)
            {
                value = _process.Postprocess(value);
            }
            
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldDefinition def)
        {
            float scale = builder.LayerScale;
            _process = new LDtkPostParserNumber(scale, def.EditorDisplayMode);
        }
    }
}