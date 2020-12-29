using System;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.FieldInjection
{
    public class LDtkParsedPoint : ILDtkValueParser
    {
        public bool IsType(Type triedType) => triedType == typeof(Vector2Int);
        private static int _verticalCellCount;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _verticalCellCount = default;
        }
        public static void InformOfRecentLayerVerticalCellCount(int verticalCellCount)
        {
            _verticalCellCount = verticalCellCount;
        }

        public object ParseValue(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }
            
            string[] coords = input.Split(',');
            
            if (!int.TryParse(coords[0], out int x))
            {
                Debug.LogError($"LDtk: Was unable to parse Point x for {input}", LDtkInjectionErrorContext.Context);
                return default;
            }
            if (!int.TryParse(coords[1], out int y))
            {
                Debug.LogError($"LDtk: Was unable to parse Point y for {input}", LDtkInjectionErrorContext.Context);
                return default;
            }

            Vector2Int point = new Vector2Int(x, y);
            return LDtkToolOriginCoordConverter.ConvertCell(point, _verticalCellCount);
        }
    }
}