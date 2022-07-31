using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkEditorGUIUtility
    {
        public static Rect ChangePositionBasedOnAnchor(Rect input, TextAnchor anchor)
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

        public static Rect GetFieldRect(Rect position)
        {
            Rect labelRect = GetLabelRect(position);
            Rect fieldRect = new Rect(position);
            fieldRect.x = labelRect.xMax;
            fieldRect.width = Mathf.Max(EditorGUIUtility.fieldWidth, position.width - labelRect.width);
            return fieldRect;
        }

        public static Rect GetLabelRect(Rect position)
        {
            Rect labelRect = new Rect(position);
            labelRect.width = EditorGUIUtility.labelWidth + 2;
            return labelRect;
        }
        
        public static float LabelWidth(float controlRectWidth)
        {
            const float divisor = 2.24f;
            const float offset = -33;
            float totalWidth = controlRectWidth + EditorGUIUtility.singleLineHeight;
            return Mathf.Max(totalWidth / divisor + offset, EditorGUIUtility.labelWidth);
        }
        
        public static void DrawDivider(float height = 2f)
        {
            const float space = 2;

            GUILayout.Space(space);
            
            Rect area = GUILayoutUtility.GetRect(0, height);
            area.xMin -= 15;
            
            float colorIntensity = EditorGUIUtility.isProSkin ? 0.1f : 0.5f;
            Color areaColor = new Color(colorIntensity, colorIntensity, colorIntensity, 1);
            EditorGUI.DrawRect(area, areaColor);
            
            GUILayout.Space(space);
        }

        public static void DenyPotentialResursiveGameObjects(SerializedProperty prop)
        {
            GameObject levelPrefab = (GameObject) prop.objectReferenceValue;
            if (ReferenceEquals(levelPrefab, null))
            {
                return;
            }
            
            if (!levelPrefab.GetComponent<LDtkComponentProject>() && 
                !levelPrefab.GetComponent<LDtkComponentLevel>())
            {
                return;
            }

            LDtkDebug.LogWarning("Not allowed to assign an imported LDtk GameObject. It would have resulted in a recursive crash.");
            prop.objectReferenceValue = null;
        }
    }
}