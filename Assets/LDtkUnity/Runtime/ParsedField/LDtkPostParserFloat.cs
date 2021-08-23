using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkPostParserFloat : ILDtkPostParser<float>
    {
        private static float _scale;
        private EditorDisplayMode _mode;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _scale = default;
        }
        public static void InformOfRecentLayer(float scale)
        {
            _scale = scale;
        }
        
        public float Postprocess(float value)
        {
            if (_mode == EditorDisplayMode.RadiusPx)
            {
                value *= _scale;
            }

            if (_mode == EditorDisplayMode.RadiusGrid)
            {
                //value *= someGridAdjustedValue
            }
            
            return value;
        }
    }
}