using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    internal static class LDtkExtensionMethods
    {
        internal static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
        
        internal static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
        
        internal static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        internal static Vector2Int ToVector2Int(this int[] array)
        {
            return new Vector2Int(array[0], array[1]);
        }
        internal static Vector2 ToVector2(this float[] array)
        {
            return new Vector2(array[0], array[1]);
        }
        
        internal static Rect ToRect(this int[] array)
        {
            return new Rect(array[0], array[1],array[2], array[3]);
        }
        internal static Rect ToRect(this float[] array)
        {
            return new Rect(array[0], array[1],array[2], array[3]);
        }

        internal static Color ToColor(this string hexString)
        {
            if (hexString.IsNullOrEmpty())
            {
                return Color.clear;
            }
            
            if (ColorUtility.TryParseHtmlString(hexString, out Color color))
            {
                return color;
            }
            LDtkDebug.LogError($"Was unable to parse Color for \"{hexString}\"");
            return default;
        }
        
        internal static string ToHex(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        
        internal static Color ToColor(this int hexInt)
        {
            string hexString = hexInt.ToString("X6");
            hexString = $"#{hexString}";
            return ToColor(hexString);
        }
        
        internal static void SetOpacity(this Tilemap tilemap, LayerInstance layer)
        {
            Color original = tilemap.color;
            original.a = layer.Opacity;
            tilemap.color = original;
        }

        internal static GameObject CreateChildGameObject(this GameObject parent, string name = "New GameObject")
        {
            GameObject child = new GameObject(name);
            child.transform.SetParent(parent.transform);
            child.transform.localPosition = Vector3.zero;
            return child;
        }
        
        internal static bool IsInteger(this float value)
        {
            return Math.Abs(value - Mathf.Floor(value)) < 0.0001f;
        }
        
        internal static Rect ToRect(this RectInt rect)
        {
            return new Rect(rect.x, rect.y, rect.width, rect.height);
        }
        
        internal static RectInt ToRectInt(this Rect rect)
        {
            return new RectInt(Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y), Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));
        }
        
        internal static Vector2Int IntPosition(this Rect rect)
        {
            return new Vector2Int(Mathf.RoundToInt(rect.x), Mathf.RoundToInt(rect.y));
        }
        
        internal static Vector2Int IntPosition(this RectInt rect)
        {
            return new Vector2Int(rect.x, rect.y);
        }
        
        public static Texture2D Copy(this Texture2D src)
        {
            Profiler.BeginSample("Graphics.CopyTexture");
            Texture2D tex = new Texture2D(src.width, src.height);
            Graphics.CopyTexture(src, 0, 0, 0, 0, tex.width, tex.height, tex, 0, 0, 0, 0);
            Profiler.EndSample();
            return tex;
        }
    }
}