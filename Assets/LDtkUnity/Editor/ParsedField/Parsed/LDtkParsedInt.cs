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
            //ints can be legally null
            if (input == null)
            {
                return 0f;
            }

            try
            {
                
                int value = Convert.ToInt32(input);

                if (_process != null)
                {
                    value = (int)_process.Postprocess(value);
                }
                
                return value;
            
            }
            catch (Exception e)
            {
                Debug.LogError($"issue {input}");
            }

            return default;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            float scale = builder.LayerScale;//todo
            _process = new LDtkPostParserNumber(scale, field.Definition.EditorDisplayMode);
        }
    }
}