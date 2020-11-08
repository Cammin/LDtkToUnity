using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkUnityExtensionMethods
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        public static Vector2Int ToVector2(this int[] array)
        {
            return new Vector2Int(array[0], array[1]);
        }
        public static Vector2 ToVector2(this float[] array)
        {
            return new Vector2(array[0], array[1]);
        }
    }
}