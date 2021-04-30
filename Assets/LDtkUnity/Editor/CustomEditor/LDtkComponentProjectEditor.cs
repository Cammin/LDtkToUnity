using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentProject), true)]
    public class LDtkComponentProjectEditor : UnityEditor.Editor
    {
        public static readonly GUIContent Content = new GUIContent
        {
            text = "Json Data",
            tooltip = "Reference to the Json. Call FromJson in this component to get it's data"
        };
        
        public override void OnInspectorGUI()
        {
            SerializedProperty prop = serializedObject.FindProperty(LDtkComponentProject.PROP_PROJECT);
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(prop, Content);
            GUI.enabled = false;
        }
    }
}