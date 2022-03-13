using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal class LDtkParsedFloat : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;

        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFloat;

        public object ImportString(object input)
        {
            //ints can be legally null
            if (input == null)
            {
                return default;
            }

            if (!float.TryParse(input.ToString(), out float value))
            {
                return default;
            }
            
            //todo currently there's a ldtk bug where float values are saved as 0 instead of null

            if (_process != null)
            {
                value = _process.Postprocess(value);
            }
            
            return value;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}