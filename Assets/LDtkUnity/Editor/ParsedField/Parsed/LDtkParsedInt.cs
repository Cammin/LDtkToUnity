using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedInt : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<float> _process;
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsInt;

        public object ImportString(object input)
        {
            if (_process == null)
            {
                Debug.LogError("LDtk: Didn't process a field value, field data may be missing");
                return 0f;
            }
            
            //ints can be legally null
            if (input == null)
            {
                return 0f;
            }

            int value = Convert.ToInt32(input);
            
            return (int)_process.Postprocess(value);
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;//todo
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}