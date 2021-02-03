using System;
using System.Linq;
using LDtkUnity.Builders;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelBuilderController))]
    public class LDtkLevelBuilderControllerEditor : UnityEditor.Editor
    {
        private bool _toggle;
        
        public override void OnInspectorGUI()
        {
            DrawMainContent();
        }

        private void DrawInternalData()
        {
            _toggle = EditorGUILayout.Foldout(_toggle, "Internal Data");
            if (_toggle)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                base.OnInspectorGUI();
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }

        private void DrawMainContent()
        {
            if (!GetProjectAsset(out LDtkProject project))
            {
                return;
            }
            
            SerializedProperty levelBoolsProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROP_LEVELS_TO_BUILD);

            if (levelBoolsProp.arraySize > 1)
            {
                DrawSelectButtons(levelBoolsProp);
            }

            DrawLevelBools(project, levelBoolsProp);

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        private void DrawLevelBools(LDtkProject project, SerializedProperty levelBoolsProp)
        {
            LdtkJson projectJson = project.ProjectJson.FromJson;
            Assert.IsNotNull(projectJson);

            levelBoolsProp.arraySize = projectJson.Levels.Length;


            bool[] levels = new bool[levelBoolsProp.arraySize];
            for (int i = 0; i < levels.Length; i++)
            {
                SerializedProperty element = levelBoolsProp.GetArrayElementAtIndex(i);


                LDtkLevelFile levelAsset = project.LevelAssets[i];

                string projectJsonLevelName = projectJson.Levels[i].Identifier;
                string fieldName = levelAsset != null
                    ? projectJsonLevelName
                    : $"(Unassigned \"{projectJsonLevelName}\")";

                element.boolValue = DrawLevelBool(element.boolValue, fieldName, levelAsset);
            }
        }

        private void DrawSelectButtons(SerializedProperty levelBoolsProp)
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
            
            DrawButton(leftButtonRect, true, "Select all", levelBoolsProp);
            DrawButton(rightButtonRect, false, "Deselect all", levelBoolsProp);
        }

        private void DrawButton(Rect rect, bool on, string label, SerializedProperty levelBoolsProp)
        {
            if (GUI.Button(rect, label, EditorStyles.miniButton))
            {
                SelectAll(on, levelBoolsProp);
            }
        }
        
        private void SelectAll(bool on, SerializedProperty levelBoolsProp)
        {
            for (int i = 0; i < levelBoolsProp.arraySize; i++)
            {
                SerializedProperty element = levelBoolsProp.GetArrayElementAtIndex(i);
                element.boolValue = on;
            }
        }
        
        
        public bool DrawLevelBool(bool prev, string label, LDtkLevelFile file)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            float rightIndent = controlRect.height;

            Rect toggleRect = new Rect(controlRect)
            {
                x = controlRect.x - controlRect.height + rightIndent,
                width = controlRect.height
            };
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + controlRect.height,
                width = controlRect.width - rightIndent
            };

            GUIContent content = new GUIContent
            {
                text = label
            };

            GUI.enabled = false;
            EditorGUI.ObjectField(fieldRect, content, file, typeof(LDtkLevelFile), false);
            GUI.enabled = true;
            return EditorGUI.Toggle(toggleRect, prev);

        }

        private bool GetProjectAsset(out LDtkProject project)
        {
            SerializedProperty projectProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROP_PROJECT_ASSETS);
            EditorGUILayout.PropertyField(projectProp);

            project = (LDtkProject) projectProp.objectReferenceValue;

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            return project != null;
        }
    }
}