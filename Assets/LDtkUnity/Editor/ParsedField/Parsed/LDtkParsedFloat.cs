using System;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedFloat : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;

        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFloat;

        public object ImportString(object input)
        {
            //floats can be legally null
            if (input == null)
            {
                return 0f;
            }

            float value = Convert.ToSingle(input);

            if (_process != null)
            {
                value = _process.Postprocess(value);
            }
            
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;//todo
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}