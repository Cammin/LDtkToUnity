using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIid))]
    [CanEditMultipleObjects]
    internal class LDtkIidEditor : UnityEditor.Editor
    {
        private static readonly GUIContent IidInfo = new GUIContent()
        {
            text = "iid",
            tooltip = "Unique instance identifier"
        };
        
        public override void OnInspectorGUI()
        {
            SerializedProperty property = serializedObject.FindProperty(LDtkIid.PROPERTY_IID);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(property, IidInfo);
            }
        }
    }
}