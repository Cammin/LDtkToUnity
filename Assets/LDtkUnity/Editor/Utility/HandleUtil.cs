using System;
using UnityEditor;
using UnityEngine;
using Rect = UnityEngine.Rect;

namespace LDtkUnity.Editor
{
    internal static class HandleUtil
    {
        public static Rect GetNormalizedTextureCoords(Texture tex, Rect srcPx)
        {
            return new Rect
            {
                x = srcPx.x / tex.width,
                y = srcPx.y / tex.height,
                width = srcPx.width / tex.width,
                height = srcPx.height / tex.height,
            };
        }

        public static float GetIconGUISize(Vector3 worldPosition, Vector2 pxSize)
        {
            /*if (AnnotationUtiltyWrapper.Use3dGizmos)
            {
                float handleSize = HandleUtility.GetHandleSize(worldPosition);
                return AnnotationUtiltyWrapper.IconSize * 3250 / handleSize;
            }*/

            const int maxResolution = 32;
            
            int maxVector = Mathf.Max((int)pxSize.x, (int)pxSize.y, 1);
            if (maxVector >= maxResolution)
            {
                return maxResolution;
            }

            int scale = 1 + Mathf.FloorToInt(((float)maxResolution-9) / maxVector);
            return maxVector * scale;
        }

        public static void DrawText(string text, Vector3 pos, Vector2 guiOffset = default, Action onClicked = null)
        {
            DrawText(text, pos, Color.black, guiOffset, onClicked);
        }
        
        public static void DrawText(string text, Vector3 pos, Color color, Vector2 guiOffset = default, Action onClicked = null)
        {
            Vector3 guiPoint = HandleUtility.WorldToGUIPointWithDepth(pos);
            //if camera is in front of the point, then don't draw it
            if (guiPoint.z < 0)
            {
                return;
            }
            
            Handles.BeginGUI();
            
            GUIContent content = new GUIContent(text);
            GUIStyle style = new GUIStyle(EditorStyles.whiteMiniLabel)
            {
                alignment = TextAnchor.UpperLeft
            };

            Rect textArea = HandleUtility.WorldPointToSizedRect(pos, content, style);
            
            
            const float yOffset = -3;
            textArea.x += 1;
            textArea.y += yOffset;
            textArea.position += guiOffset;

            Rect backdropArea = new Rect(textArea);
            backdropArea.x += 1;
            backdropArea.y += 4;
            backdropArea.width -= 6;
            backdropArea.height -= 10;
                
            if (GUI.Button(backdropArea, GUIContent.none, GUIStyle.none))
            {
                onClicked?.Invoke();
            }
            
            //maintain hue, maximise saturation, maintain value
            Color.RGBToHSV(color, out float h, out float s, out float v);

            if (s > 0)
            {
                s = 1;
            }
            
            Color backdropColor = Color.HSVToRGB(h, s, v);
            backdropColor.a = 0.75f;

            Color textColor = GetTextColorForSceneText(backdropColor);
            style.normal = new GUIStyleState()
            {
                textColor = textColor
            };
            
            EditorGUI.DrawRect(backdropArea, backdropColor);
            
            GUI.Label(textArea, content, style);
            
            Handles.EndGUI();
        }


        public static Color GetTextColorForSceneText(Color backdropColor)
        {
            return GetTextColorForBackdrop(backdropColor, 149);
        }

        public static Color GetTextColorForIntGridValueNumber(Color backdropColor)
        {
            return GetTextColorForBackdrop(backdropColor, 75);
        }
        
        private static Color GetTextColorForBackdrop(Color backdropColor, float threshold)
        {
            const float colorValue = 0.1f;

            float red = backdropColor.r * 255;
            float green = backdropColor.g * 255;
            float blue = backdropColor.b * 255;
            float luminosity = red * 0.299f + green * 0.587f + blue * 0.114f;
            
            //credit https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color
            return luminosity > threshold 
                ? new Color(colorValue, colorValue, colorValue) 
                : Color.white;
        }

        public static void SelectIfNotAlreadySelected(GameObject obj)
        {
            if (Selection.activeGameObject != obj)
            {
                Selection.activeGameObject = obj;
            }
        }


    }
}





 
