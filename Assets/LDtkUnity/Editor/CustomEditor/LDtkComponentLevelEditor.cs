using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentLevel), true)]
    internal sealed class LDtkComponentLevelEditor : UnityEditor.Editor
    {
        private SerializedProperty _identifier;
        private SerializedProperty _size;
        private SerializedProperty _bgColor;
        private SerializedProperty _smartColor;
        private SerializedProperty _worldDepth;
        private SerializedProperty _neighbors;

        private void OnEnable()
        {
            _identifier = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_IDENTIFIER);
            _size = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_SIZE);
            _bgColor = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_BG_COLOR);
            _smartColor = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_SMART_COLOR);
            _worldDepth = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_WORLD_DEPTH);
            _neighbors = serializedObject.FindProperty(LDtkComponentLevel.PROPERTY_NEIGHBOURS);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_identifier);
            EditorGUILayout.PropertyField(_size);
            EditorGUILayout.PropertyField(_bgColor);
            EditorGUILayout.PropertyField(_smartColor);
            EditorGUILayout.PropertyField(_worldDepth);
            DrawNeighbours();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNeighbours()
        {
            if (_neighbors.arraySize <= 0)
            {
                return;
            }
            
            GUILayout.Label("Neighbours", EditorStyles.miniBoldLabel);
            for (int i = 0; i < _neighbors.arraySize; i++)
            {
                SerializedProperty prop = _neighbors.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop);
            }
        }
    }
}