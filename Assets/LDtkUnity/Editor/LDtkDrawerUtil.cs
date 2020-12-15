using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkDrawerUtil
    {
        public static void ScrollView(ref Vector2 scroll, Action draw)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            draw.Invoke();
            EditorGUILayout.EndScrollView();
        }

        private const string ICON_NAME_INFO = "console.infoicon.sml";
        private const string ICON_NAME_INFO_BIG = "console.infoicon@2x";
        private const string ICON_NAME_WARNING = "console.warnicon.sml";
        private const string ICON_NAME_WARNING_BIG = "console.warnicon@2x";
        private const string ICON_NAME_ERROR = "console.erroricon.sml";
        private const string ICON_NAME_ERROR_BIG = "console.erroricon@2x";
        
        public delegate void IconDraw(Vector2 pos, string tooltipText, TextAnchor anchor);
        
        public static void DrawInfo(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_INFO, tooltipText, anchor);
        public static void DrawInfoBig(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_INFO_BIG, tooltipText, anchor);
        public static void DrawWarning(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_WARNING, tooltipText, anchor);
        public static void DrawWarningBig(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_WARNING_BIG, tooltipText, anchor);
        public static void DrawError(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_ERROR, tooltipText, anchor);
        public static void DrawErrorBig(Vector2 pos, string tooltipText, TextAnchor anchor) => DrawIconInternal(pos, ICON_NAME_ERROR_BIG, tooltipText, anchor);

        private static void DrawIconInternal(Vector2 pos, string iconName, string tooltipText, TextAnchor anchor)
        {
            GUIContent gui = EditorGUIUtility.IconContent(iconName);
            gui.tooltip = tooltipText;

            Texture2D tex = (Texture2D)gui.image;
            Vector2 size = new Vector2(tex.width, tex.height);
            Rect rect = new Rect(pos, size);
            rect = ChangeBasedOnPivot(rect, anchor);
            
            GUI.Label(rect, gui);
            GUI.DrawTexture(rect, tex);
        }

        private static Rect ChangeBasedOnPivot(Rect input, TextAnchor anchor)
        {
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    Upper();
                    Left();
                    break;
                
                case TextAnchor.UpperCenter:
                    Upper();
                    Center();
                    break;
                
                case TextAnchor.UpperRight:
                    Upper();
                    Right();
                    break;
                
                case TextAnchor.MiddleLeft:
                    Middle();
                    Left();
                    break;
                case TextAnchor.MiddleCenter:
                    Middle();
                    Center();
                    break;
                
                case TextAnchor.MiddleRight:
                    Middle();
                    Right();
                    break;
                
                case TextAnchor.LowerLeft:
                    Lower();
                    Left();
                    break;
                
                case TextAnchor.LowerCenter:
                    Lower();
                    Center();
                    break;
                
                case TextAnchor.LowerRight:
                    Lower();
                    Right();
                    break;
            }

            return input;

            void Left()
            {
                //do nothing to x
            }
            void Center()
            {
                input.x -= input.width * 0.5f;
            }
            void Right()
            {
                input.x -= input.width;
            }
            void Upper()
            {
                //do nothing to y
            }
            void Middle()
            {
                input.y -= input.height * 0.5f;
            }
            void Lower()
            {
                input.y -= input.height;
            }
        }


        public static float LabelWidth(float controlRectWidth)
        {
            const float divisor = 2.24f;
            const float offset = -33;
            float totalWidth = controlRectWidth + EditorGUIUtility.singleLineHeight;
            return Mathf.Max(totalWidth / divisor + offset, EditorGUIUtility.labelWidth);
        }
    }
}