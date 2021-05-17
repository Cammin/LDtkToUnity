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
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsPoint;
        
        private static int _verticalCellCount;
        private static Vector2 _relativeLevelPosition;
        
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


        public object ImportString(object input)
        {
            //Point can be legally null. for the purposes of the scene drawer, a null point in LDtk will translate to a magic vector2 that tells the scene drawer not to draw
            if (input == null)
            {
                return Vector2.negativeInfinity;
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
            return LDtkToolOriginCoordConverter.ConvertParsedPointValue(_relativeLevelPosition, point, _verticalCellCount);
        }
    }
}