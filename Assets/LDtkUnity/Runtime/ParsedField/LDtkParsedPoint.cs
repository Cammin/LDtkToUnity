using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkParsedPoint : ILDtkValueParser
    {
        private struct LDtkPoint
        {
            [JsonProperty("cx")]
            public int Cx { get; set; }
            
            [JsonProperty("cy")]
            public int Cy { get; set; }
        }

        public struct PositionData
        {
            public Vector2 LevelPosition;
            public int LvlCellHeight;
            public int PixelsPerUnit;
            public int GridSize;
        }
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsPoint;
        
        private static PositionData _data;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _data = default;
        }
        public static void InformOfRecentLayerVerticalCellCount(PositionData data)
        {
            _data = data;
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

            Vector2Int cellPos = new Vector2Int(x, y);
            return LDtkCoordConverter.ConvertParsedPointValue(cellPos, _data);
        }
    }
}