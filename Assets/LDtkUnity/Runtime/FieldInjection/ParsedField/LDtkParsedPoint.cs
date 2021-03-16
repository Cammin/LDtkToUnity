using System;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkParsedPoint : ILDtkValueParser
    {
        private struct LDtkPoint
        {
            [JsonProperty("cx")]
            public int Cx { get; set; }
            
            [JsonProperty("cy")]
            public int Cy { get; set; }
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
            //Point can be legally null
            if (input == null)
            {
                return Vector2.zero;
            }
            
            
            string stringInput = Convert.ToString(input);
            
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }

            LDtkPoint pointData = JsonConvert.DeserializeObject<LDtkPoint>(stringInput);
            
            int x = pointData.Cx;
            int y = pointData.Cy;

            Vector2Int point = new Vector2Int(x, y);
            return LDtkToolOriginCoordConverter.ConvertParsedValue(_relativeLevelPosition, point, _verticalCellCount);
        }
    }
}