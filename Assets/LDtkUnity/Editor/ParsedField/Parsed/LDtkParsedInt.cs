namespace LDtkUnity.Editor
{
    internal class LDtkParsedInt : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsInt;

        public object ImportString(object input)
        {
            //ints can be legally null
            if (input == null)
            {
                return 0;
            }

            if (!int.TryParse(input.ToString(), out int value))
            {
                return 0;
            }
            
            if (_process != null)
            {
                value = (int)_process.Postprocess(value);
            }
            
            //a required cast to fix an issue with null ints
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}