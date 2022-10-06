using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentLayer), true)]
    internal class LDtkComponentLayerEditor : UnityEditor.Editor
    {
        private SerializedProperty _identifier;
        private SerializedProperty _type;
        private SerializedProperty _scale;

        private void OnEnable()
        {
            _identifier = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_IDENTIFIER);
            _type = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_TYPE);
            _scale = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_LAYER_SCALE);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_identifier);
            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.PropertyField(_scale);

            serializedObject.ApplyModifiedProperties();
        }
    }
}