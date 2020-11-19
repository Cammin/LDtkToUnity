using System;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public class LDtkParsedPoint : ILDtkParsedValue
    {
        public Type Type => typeof(Vector2Int);
        public Type TypeArray => typeof(Vector2Int[]);
        public string TypeString => "Point";

        private static int _verticalCellCount;
        
#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
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
            string[] coords = input.Split(',');
            
            if (!int.TryParse(coords[0], out int x))
            {
                Debug.LogError($"LDtk: Was unable to parse Point x for {input}");
                return default;
            }
            if (!int.TryParse(coords[1], out int y))
            {
                Debug.LogError($"LDtk: Was unable to parse Point y for {input}");
                return default;
            }

            Vector2Int point = new Vector2Int(x, y);
            return LDtkToolOriginCoordConverter.ConvertCell(point, _verticalCellCount);
        }
    }
}