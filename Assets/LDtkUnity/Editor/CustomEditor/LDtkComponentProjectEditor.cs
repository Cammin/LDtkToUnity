using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentProject), true)]
    internal class LDtkComponentProjectEditor : UnityEditor.Editor
    {
        private readonly GUIContent _content = new GUIContent
        {
            text = "Json Data",
            tooltip = "Reference to the Json. Call FromJson in this component to get it's data"
        };

        public override void OnInspectorGUI()
        {
            SerializedProperty prop = serializedObject.FindProperty(LDtkComponentProject.PROPERTY_PROJECT);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(prop, _content);
            }
        }
    }
}