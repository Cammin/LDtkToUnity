using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
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
            Handles.BeginGUI();
            
            GUIContent content = new GUIContent(text);
            GUIStyle style = new GUIStyle(EditorStyles.whiteMiniLabel)
            {
                alignment = TextAnchor.UpperLeft
            };

            Rect textArea = HandleUtility.WorldPointToSizedRect(pos, content, style);
            
            textArea.x += 1;
            textArea.y -= 3;
            textArea.position += guiOffset;
            //textArea.position += Vector2.one;
                
            Rect backdropArea = new Rect(textArea);
            backdropArea.x += 1;
            backdropArea.y += 4;
            backdropArea.width -= 6;
            backdropArea.height -= 10;
                
            if (GUI.Button(backdropArea, GUIContent.none, GUIStyle.none))
            {
                onClicked?.Invoke();
            }
                
            Color color = new Color(0,0,0, 0.33f);
                
            EditorGUI.DrawRect(backdropArea, color);
            GUI.Label(textArea, content, style);
            
            Handles.EndGUI();
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





 
