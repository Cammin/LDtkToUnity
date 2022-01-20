using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    internal static class LDtkExtensionMethods
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        internal static Vector2Int ToVector2Int(this long[] array)
        {
            return new Vector2Int((int)array[0], (int)array[1]);
        }
        internal static Vector2 ToVector2(this double[] array)
        {
            return new Vector2((float)array[0], (float)array[1]);
        }
        
        internal static Rect ToRect(this long[] array)
        {
            return new Rect(array[0], array[1],array[2], array[3]);
        }
        internal static Rect ToRect(this double[] array)
        {
            return new Rect((float)array[0], (float)array[1],(float)array[2], (float)array[3]);
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
        
        internal static void SetOpacity(this Tilemap tilemap, LayerInstance layer)
        {
            Color original = tilemap.color;
            original.a = (float)layer.Opacity;
            tilemap.color = original;
        }

        internal static GameObject CreateChildGameObject(this GameObject parent, string name = "New GameObject")
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent.transform);
            child.transform.localPosition = Vector3.zero;
            return child;
        }
    }
}