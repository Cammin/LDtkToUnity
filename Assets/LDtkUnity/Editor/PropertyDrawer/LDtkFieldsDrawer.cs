using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkFields))]
    public class LDtkFieldsDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty fieldsProp = serializedObject.FindProperty(LDtkFields.PROP_FIELDS);

            EditorGUILayout.PropertyField(fieldsProp);
        }
    }
}