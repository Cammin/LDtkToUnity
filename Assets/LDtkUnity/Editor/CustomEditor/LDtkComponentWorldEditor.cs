using UnityEditor;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentWorld), true)]
    [CanEditMultipleObjects]
    internal sealed class LDtkComponentWorldEditor : UnityEditor.Editor
    {
        private SerializedProperty _identifier;
        private SerializedProperty _worldGridSize;
        private SerializedProperty _worldLayout;
        
        private void OnEnable()
        {
            _identifier = serializedObject.FindProperty(LDtkComponentWorld.PROPERTY_IDENTIFIER);
            _worldGridSize = serializedObject.FindProperty(LDtkComponentWorld.PROPERTY_WORLD_GRID_SIZE);
            _worldLayout = serializedObject.FindProperty(LDtkComponentWorld.PROPERTY_WORLD_LAYOUT);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_identifier);
            EditorGUILayout.PropertyField(_worldGridSize);
            EditorGUILayout.PropertyField(_worldLayout);
            serializedObject.ApplyModifiedProperties();
        }
    }
}