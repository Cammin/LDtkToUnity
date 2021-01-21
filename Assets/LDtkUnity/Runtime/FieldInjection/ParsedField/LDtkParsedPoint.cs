using System;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedPoint : ILDtkValueParser
    {
        
        
        public string TypeName => "Point";
        
        
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

        public object ParseValue(object input)
        {
            string stringInput = (string) input;
            
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }
            
            string[] coords = stringInput.Split(',');
            
            if (!int.TryParse(coords[0], out int x))
            {
                Debug.LogError($"LDtk: Was unable to parse Point x for {stringInput}", LDtkInjectionErrorContext.Context);
                return default;
            }
            if (!int.TryParse(coords[1], out int y))
            {
                Debug.LogError($"LDtk: Was unable to parse Point y for {stringInput}", LDtkInjectionErrorContext.Context);
                return default;
            }

            Vector2Int point = new Vector2Int(x, y);
            return LDtkToolOriginCoordConverter.ConvertParsedValue(_relativeLevelPosition, point, _verticalCellCount);
        }
    }
}