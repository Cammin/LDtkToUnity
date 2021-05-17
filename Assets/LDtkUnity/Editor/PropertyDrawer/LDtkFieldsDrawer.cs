using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkFields))]
    public class LDtkFieldsDrawer : UnityEditor.Editor
    {
        private static readonly GUIContent HelpBox = new GUIContent()
        {
            text = "Access the fields by gaining reference to this component with GetComponent<LDtkFields>();",
        };

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(HelpBox, true);
            
            SerializedProperty fieldsProp = serializedObject.FindProperty(LDtkFields.PROP_FIELDS);
            EditorGUILayout.PropertyField(fieldsProp);
        }
    }
}