using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkFields))]
    public class LDtkFieldsEditor : UnityEditor.Editor
    {
        private static readonly GUIContent HelpBox = new GUIContent()
        {
            text = "Access the fields by gaining reference to this component with GetComponent<LDtkFields>();",
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.HelpBox(HelpBox, true);
            
            SerializedProperty fieldsProp = serializedObject.FindProperty(LDtkFields.PROP_FIELDS);

            for (int i = 0; i < fieldsProp.arraySize; i++)
            {
                SerializedProperty elementProp = fieldsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(elementProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}