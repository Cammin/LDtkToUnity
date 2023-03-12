using UnityEditor;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentLayer), true)]
    [CanEditMultipleObjects]
    internal sealed class LDtkComponentLayerEditor : UnityEditor.Editor
    {
        private SerializedProperty _identifier;
        private SerializedProperty _doc;
        private SerializedProperty _type;
        private SerializedProperty _scale;

        private void OnEnable()
        {
            _identifier = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_IDENTIFIER);
            _doc = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_DOC);
            _type = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_TYPE);
            _scale = serializedObject.FindProperty(LDtkComponentLayer.PROPERTY_LAYER_SCALE);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_identifier);
            EditorGUILayout.PropertyField(_doc);
            EditorGUILayout.PropertyField(_type);
            EditorGUILayout.PropertyField(_scale);

            serializedObject.ApplyModifiedProperties();
        }
    }
}