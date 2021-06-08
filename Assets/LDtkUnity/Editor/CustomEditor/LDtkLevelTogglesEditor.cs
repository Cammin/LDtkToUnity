using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelToggles))]
    public class LDtkLevelTogglesEditor : UnityEditor.Editor
    {
        private LDtkLevelToggles Target => (LDtkLevelToggles) target;
        private SerializedProperty Toggles => serializedObject.FindProperty(LDtkLevelToggles.TOGGLES);
        private SerializedProperty Levels => serializedObject.FindProperty(LDtkLevelToggles.LEVELS);
        

        private void ApplyGameObjects()
        {
            Undo.SetCurrentGroupName("Change Level Visibility");
            int group = Undo.GetCurrentGroup();
            
            for (int i = 0; i < Levels.arraySize; i++)
            {
                LDtkComponentLevel level = (LDtkComponentLevel)Levels.GetArrayElementAtIndex(i).objectReferenceValue;
                bool toggle = Toggles.GetArrayElementAtIndex(i).boolValue;

                GameObject obj = level.gameObject;
                Undo.RecordObject(obj, obj.name);

                obj.SetActive(toggle);
                obj.hideFlags = toggle ? HideFlags.None : HideFlags.None;
                obj.tag = toggle ? "Untagged" : "EditorOnly";
            }
            
            Undo.CollapseUndoOperations(group);
            
            Repaint();
            EditorApplication.RepaintHierarchyWindow();
            EditorApplication.DirtyHierarchyWindowSorting();
        }
        
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            
            serializedObject.Update();
            Draw();

            serializedObject.ApplyModifiedProperties();
        }

        private void Draw()
        {
            Resize();
            
            if (Levels.arraySize > 1)
            {
                DrawSelectButtons();
            }

            DrawLevelBools();
        }

        private void DrawLevelBools()
        {
            for (int i = 0; i < Levels.arraySize; i++)
            {
                SerializedProperty toggleProp = Toggles.GetArrayElementAtIndex(i);
                SerializedProperty levelProp = Levels.GetArrayElementAtIndex(i);
                
                GUIContent content = new GUIContent
                {
                    text = levelProp.objectReferenceValue.name
                };
                
                EditorGUILayout.PropertyField(toggleProp, content);
            }
            
            if (serializedObject.hasModifiedProperties)
            {
                ApplyGameObjects();
            }
        }
        
        private void DrawSelectButtons()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            float width = 80;

            Rect leftButtonRect = new Rect(controlRect)
            {
                width = width
            };
            Rect rightButtonRect = new Rect(leftButtonRect)
            {
                x = controlRect.x + width
            };
            
            DrawButton(leftButtonRect, true, "Select all");
            DrawButton(rightButtonRect, false, "Deselect all");
        }
        private void DrawButton(Rect rect, bool on, string label)
        {
            if (!GUI.Button(rect, label, EditorStyles.miniButton))
            {
                return;
            }
            
            for (int i = 0; i < Toggles.arraySize; i++)
            {
                Toggles.GetArrayElementAtIndex(i).boolValue = on;
            }

            if (serializedObject.hasModifiedProperties)
            {
                ApplyGameObjects();
            }
        }
        
        private void Resize()
        {
            Toggles.arraySize = Levels.arraySize;
        }
    }
}