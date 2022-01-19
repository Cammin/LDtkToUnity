using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkFields))]
    internal class LDtkFieldsEditor : UnityEditor.Editor
    {
        private readonly GUIContent _helpBox = new GUIContent()
        {
            text = "Access with GetComponent<LDtkFields>, or with a custom component inheriting ILDtkImportedFields",
        };

        private void OnSceneGUI()
        {
            //todo consider drawing the values in here. might be better than a point drawer component
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.HelpBox(_helpBox, true);
            
            SerializedProperty fieldsProp = serializedObject.FindProperty(LDtkFields.PROPERTY_FIELDS);

            for (int i = 0; i < fieldsProp.arraySize; i++)
            {
                SerializedProperty elementProp = fieldsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(elementProp);
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}