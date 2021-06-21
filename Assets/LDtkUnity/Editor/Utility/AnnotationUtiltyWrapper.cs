using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class AnnotationUtiltyWrapper
    {
        private static readonly System.Type _annotationUtilityType;
        private static readonly System.Reflection.PropertyInfo _iconSize;
        private static readonly System.Reflection.PropertyInfo _use3dGizmos;

        public static bool Use3dGizmos => _use3dGizmos != null && (bool) _use3dGizmos.GetValue(null, null);


        
        public static float IconSize => (_iconSize == null) ? 1f : (float) _iconSize.GetValue(null, null);

        public static float IconSizeLinear => ConvertTexelWorldSizeTo01(IconSize);
        
        static AnnotationUtiltyWrapper()
        {
            _annotationUtilityType = typeof(UnityEditor.Editor).Assembly.GetTypes().FirstOrDefault(t => t.Name == "AnnotationUtility");
            if (_annotationUtilityType == null)
            {
                Debug.LogWarning("The internal type 'AnnotationUtility' could not be found. Maybe something changed inside Unity");
                return;
            }

            _iconSize = _annotationUtilityType.GetProperty("iconSize", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (_iconSize == null)
            {
                Debug.LogWarning("The internal class 'AnnotationUtility' doesn't have a property called 'iconSize'");
            }
            
            _use3dGizmos = _annotationUtilityType.GetProperty("use3dGizmos", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (_iconSize == null)
            {
                Debug.LogWarning("The internal class 'AnnotationUtility' doesn't have a property called 'use3dGizmos'");
            }
        }

        private static float Convert01ToTexelWorldSize(float value01)
        {
            return value01 <= 0f ? 0f : Mathf.Pow(10f, -3f + 3f * value01);
        }

        private static float ConvertTexelWorldSizeTo01(float texelWorldSize)
        {
            if (texelWorldSize == -1f)
            {
                return 1f;
            }

            if (texelWorldSize == 0f)
            {
                return 0f;
            }

            return (Mathf.Log10(texelWorldSize) - -3f) / 3f;
        }
    }
}