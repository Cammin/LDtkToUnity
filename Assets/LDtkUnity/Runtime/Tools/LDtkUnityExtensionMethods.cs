using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Runtime.Tools
{
    internal static class LDtkUnityExtensionMethods
    {
        internal static bool NullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        internal static Vector2Int ToVector2Int(this int[] array)
        {
            return new Vector2Int(array[0], array[1]);
        }
        
        internal static Rect ToRect(this int[] array)
        {
            return new Rect(array[0], array[1],array[2], array[3]);
        }
    }
}