using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkIid))]
    public class LDtkIidEditor : UnityEditor.Editor
    {
        private static readonly GUIContent IidInfo = new GUIContent()
        {
            text = "iid",
            tooltip = "Unique instance identifier"
        };
        
        private SerializedProperty _iidProperty;

        private void OnEnable()
        {
            _iidProperty = serializedObject.FindProperty(LDtkIid.PROPERTY_IID);
        }

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_iidProperty, IidInfo);
            }
        }
    }
}