using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkComponentLevel), true)]
    internal class LDtkComponentLevelEditor : UnityEditor.Editor
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

            GUILayout.Label("Neighbours", EditorStyles.miniBoldLabel);
            for (int i = 0; i < _neighbors.arraySize; i++)
            {
                SerializedProperty prop = _neighbors.GetArrayElementAtIndex(i);
                LDtkField propObjectReferenceValue = (LDtkField)prop.boxedValue;
                string dir = propObjectReferenceValue.GetSingle().Value.AsNeighbourLevel().Dir;
                EditorGUILayout.PropertyField(prop);

                Rect dirRect = GUILayoutUtility.GetLastRect();
                dirRect.height = EditorGUIUtility.singleLineHeight;
                dirRect.width = EditorGUIUtility.singleLineHeight;
                dirRect.x -= 3;
                GUI.Label(dirRect, $"{char.ToUpper(dir[0])}");
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}