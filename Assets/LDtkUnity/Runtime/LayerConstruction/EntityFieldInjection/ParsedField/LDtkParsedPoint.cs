using System;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection.ParsedField
{
    public class LDtkParsedPoint : ILDtkParsedValue
    {
        public Type Type => typeof(Vector2Int);
        public Type TypeArray => typeof(Vector2Int[]);
        public string TypeString => "Point";

        public object ParseValue(string input)
        {
            string[] coords = input.Split(',');
            
            if (!int.TryParse(coords[0], out int x))
            {
                Debug.LogError($"Was unable to parse Point x for {input}");
                return default;
            }
            if (!int.TryParse(coords[1], out int y))
            {
                Debug.LogError($"Was unable to parse Point y for {input}");
                return default;
            }

            return new Vector2Int(x, y);
        }
    }
}