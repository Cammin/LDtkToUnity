using System;
using LDtkUnity.Builders;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelBuilderController))]
    public class LDtkLevelBuilderControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty projectProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROP_PROJECT_ASSETS);
            SerializedProperty buildPrefProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROP_BUILD_PREFERENCE);
            SerializedProperty levelsProp = serializedObject.FindProperty(LDtkLevelBuilderController.PROP_LEVELS_TO_BUILD);

            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(projectProp);
            EditorGUILayout.PropertyField(buildPrefProp);

            
            switch ((LDtkLevelBuilderControllerPreference)buildPrefProp.enumValueIndex)
            {
                case LDtkLevelBuilderControllerPreference.Single:
                    levelsProp.arraySize = 1;
                    GUIContent content = new GUIContent("Level to Build");
                    EditorGUILayout.PropertyField(levelsProp.GetArrayElementAtIndex(0), content);
                    break;
                
                case LDtkLevelBuilderControllerPreference.Partial:
                    EditorGUILayout.PropertyField(levelsProp);
                    break;
                
                case LDtkLevelBuilderControllerPreference.All:
                    levelsProp.arraySize = 0;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}