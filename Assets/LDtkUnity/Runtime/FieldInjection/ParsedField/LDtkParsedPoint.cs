using System;
using LDtkUnity.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedPoint : ILDtkValueParser
    {
        private struct LDtkPoint
        {
            public int cx;
            public int cy;
        }
        
        private static int _verticalCellCount;
        private static Vector2 _relativeLevelPosition;
        
        public string TypeName => "Point";

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _verticalCellCount = default;
            _relativeLevelPosition = default;
        }
        public static void InformOfRecentLayerVerticalCellCount(Vector2 relativeLevelPosition, int verticalCellCount)
        {
            _verticalCellCount = verticalCellCount;
            _relativeLevelPosition = relativeLevelPosition;
        }

        public object ParseValue(object input)
        {
            string stringInput = Convert.ToString(input);
            
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }

            LDtkPoint pointData = JsonConvert.DeserializeObject<LDtkPoint>(stringInput);
            
            int x = pointData.cx;
            int y = pointData.cy;

            Vector2Int point = new Vector2Int(x, y);
            return LDtkToolOriginCoordConverter.ConvertParsedValue(_relativeLevelPosition, point, _verticalCellCount);
        }
    }
}