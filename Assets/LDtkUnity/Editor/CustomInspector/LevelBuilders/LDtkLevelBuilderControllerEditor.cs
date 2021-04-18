using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    /*public abstract class LDtkLevelBuilderControllerEditor : UnityEditor.Editor
    {
        private bool _toggle;

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

        protected bool DrawJsonField()
        {
            LDtkProject project = ProjectJsonField();
            if (!project)
            {
                return false;
            }

            if (project.ProjectJson != null)
            {
                return true;
            }
            
            EditorGUILayout.HelpBox("Project asset's json file is not assigned", MessageType.Error);
            return false;

        }

        protected void DrawLevels()
        {
            SerializedProperty projectProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROJECT_ASSETS);
            LDtkProject project = (LDtkProject)projectProp.objectReferenceValue;
            if (!project)
            {
                return;
            }

            LDtkDrawerUtil.DrawDivider();
            
            
            SerializedProperty levelBoolsProp = serializedObject.FindProperty(LDtkLevelBuilderController.LEVELS_TO_BUILD);

            if (levelBoolsProp.arraySize > 1)
            {
                DrawSelectButtons(levelBoolsProp);
            }

            DrawLevelBools(project, levelBoolsProp);
            
            serializedObject.ApplyModifiedProperties();
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

            Texture2D blankSpace = GetBlankImage();

            GUIContent content = new GUIContent
            {
                image = blankSpace,
                text = label
            };

            GUI.enabled = false;
            EditorGUI.ObjectField(controlRect, content, file, typeof(LDtkLevelFile), false);
            GUI.enabled = true;
            return EditorGUI.Toggle(toggleRect, prev);

        }

        private static Texture2D GetBlankImage()
        {
            Texture2D blankSpace = new Texture2D(15, 15);
            for (int y = 0; y < blankSpace.height; y++)
            {
                for (int x = 0; x < blankSpace.width; x++)
                {
                    Color color = Color.clear;
                    blankSpace.SetPixel(x, y, color);
                }
            }

            blankSpace.Apply();
            return blankSpace;
        }

        private LDtkProject ProjectJsonField()
        {
            SerializedProperty projectProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROJECT_ASSETS);
            EditorGUILayout.PropertyField(projectProp);

            LDtkProject project = (LDtkProject) projectProp.objectReferenceValue;

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            return project;
        }
    }*/
}