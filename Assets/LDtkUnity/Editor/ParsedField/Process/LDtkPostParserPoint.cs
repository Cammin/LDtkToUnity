using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkPostParserPoint : ILDtkPostParseProcess<Vector2>
    {
        private readonly PointParseData _data;

        public LDtkPostParserPoint(PointParseData data)
        {
            _data = data;
        }
        
        public Vector2 Postprocess(Vector2 value)
        {
            return LDtkCoordConverter.ConvertParsedPointValue(Vector2Int.RoundToInt(value), _data);
        }
    }
}