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
            
            GUILayout.Label("Neighbours", EditorStyles.miniBoldLabel);
            for (int i = 0; i < _neighbors.arraySize; i++)
            {
                SerializedProperty prop = _neighbors.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(prop);
                
                Rect dirRect = GUILayoutUtility.GetLastRect();
                dirRect.height = EditorGUIUtility.singleLineHeight;
                dirRect.width = EditorGUIUtility.singleLineHeight;
                dirRect.x -= 3;
                
                SerializedProperty dataArray = prop.FindPropertyRelative(LDtkField.PROPERTY_DATA);
                SerializedProperty element = dataArray.GetArrayElementAtIndex(0);
                SerializedProperty intValue = element.FindPropertyRelative(LDtkFieldElement.PROPERTY_INT);
                char dir = (char)intValue.intValue;
                
                GUI.Label(dirRect, $"{char.ToUpper(dir)}");
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}