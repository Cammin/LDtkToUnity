using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    internal static class GizmoUtil
    {
        /// <summary>
        /// Draws a texture at a world position during the gizmos draw event. Can also specify a rect slice for a certain area of the texture
        /// </summary>
        public static void DrawGUITextureInWorld(Texture tex, Vector3 worldPosition)
        {
            DrawGUITextureInWorld(tex, worldPosition, new Rect(0, 0, 1, 1));
        }
    
        /// <summary>
        /// Draws a texture at a world position during the gizmos draw event. Can also specify a rect slice for a certain area of the texture
        /// </summary>
        public static void DrawGUITextureInWorld(Texture tex, Vector3 worldPosition, Rect src)
        {
#if UNITY_EDITOR
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            
            if (!tex)
            {
                Debug.LogError("Tex null");
                return;
            }

            Camera sceneCamera = UnityEditor.SceneView.currentDrawingSceneView.camera;
        
            //if camera is in front of the point, then don't draw it
            Transform camTrans = sceneCamera.transform;
            Vector3 heading = worldPosition - camTrans.position;
            float dot = Vector3.Dot(heading, camTrans.forward);
            if (dot < 0)
            {
                return;
            }

            Vector3 screenPoint = sceneCamera.WorldToScreenPoint(worldPosition);


            Rect normalizedSrc = NormalizeSize(src, new Vector2(tex.width, tex.height));


            float iconSize = GetIconSize(worldPosition, src.size);

            Vector2 size = Vector2.one * iconSize;
            Vector2 coord = new Vector2(screenPoint.x, sceneCamera.pixelHeight - screenPoint.y) - size / 2;

            Rect rect = new Rect(coord, size);
        
            UnityEditor.Handles.BeginGUI();
            GUI.DrawTextureWithTexCoords(rect, tex, normalizedSrc);
            UnityEditor.Handles.EndGUI();
        }

        private static Rect NormalizeSize(Rect input, Vector2 totalSize)
        {
            return new Rect(input)
            {
                x = input.x / totalSize.x,
                y = input.y / totalSize.y,
                width = input.width / totalSize.x,
                height = input.height / totalSize.y,
            };
        }

        private static float GetIconSize(Vector3 worldPosition, Vector2 pxSize)
        {
            if (AnnotationUtiltyWrapper.Use3dGizmos)
            {
                float handleSize = UnityEditor.HandleUtility.GetHandleSize(worldPosition);
                return AnnotationUtiltyWrapper.IconSize * 3250 / handleSize;
            }

            const int maxResolution = 32;
            
            int maxDimension = Mathf.Max((int)pxSize.x, (int)pxSize.y, 1);
            if (maxDimension >= maxResolution)
            {
                return maxResolution;
            }

            int scale = 1 + Mathf.FloorToInt((float)maxResolution / maxDimension);
            return maxDimension * scale;
        }

        private static class AnnotationUtiltyWrapper
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
#endif
        }
    }
}





 
