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

        internal static Color ToColor(this string hexString)
        {
            if (ColorUtility.TryParseHtmlString(hexString, out Color color))
            {
                return color;
            }
            Debug.LogError($"LDtk: Was unable to parse Color for \"{hexString}\"");
            return default;
        }
    }
}