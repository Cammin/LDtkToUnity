namespace LDtkUnity.Editor
{
    internal sealed class LDtkPostParserNumber : ILDtkPostParseProcess<float>
    {
        private readonly float _scale;
        private readonly EditorDisplayMode _mode;

        public LDtkPostParserNumber(float scale, EditorDisplayMode mode)
        {
            _scale = scale;
            _mode = mode;
        }

        public float Postprocess(float value)
        {
            //we want to convert the radius to perfect radius based on scale as if it was displayed in ldtk

            //Debug.Log($"event {_mode}");
            
            if (_mode == EditorDisplayMode.RadiusPx)
            {
                //Debug.Log($"px grid from {value} to {value * _scale}");
                value *= _scale;
            }

            if (_mode == EditorDisplayMode.RadiusGrid)
            {
                //Debug.Log($"radius grid from {value} to {value * _scale}");
                value *= _scale;
            }
            
            return value;
        }
    }
}