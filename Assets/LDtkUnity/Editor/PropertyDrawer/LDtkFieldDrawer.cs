using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkField))]
    internal class LDtkFieldDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty arrayProp = property.FindPropertyRelative(LDtkField.PROPERTY_DATA);
            SerializedProperty isSingleProp = property.FindPropertyRelative(LDtkField.PROPERTY_SINGLE);

            if (isSingleProp.boolValue)
            {
                SerializedProperty singleElement = arrayProp.GetArrayElementAtIndex(0);
                return EditorGUI.GetPropertyHeight(singleElement, label);
            }
            
            return EditorGUI.GetPropertyHeight(arrayProp, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty identifierProp = property.FindPropertyRelative(LDtkField.PROPERTY_IDENTIFIER);
            GUIContent content = new GUIContent(label)
            {
                text = identifierProp.stringValue
            };
            
            SerializedProperty arrayProp = property.FindPropertyRelative(LDtkField.PROPERTY_DATA);
            SerializedProperty isSingleProp = property.FindPropertyRelative(LDtkField.PROPERTY_SINGLE);
            
            if (isSingleProp.boolValue)
            {
                SerializedProperty singleElement = arrayProp.GetArrayElementAtIndex(0);
                EditorGUI.PropertyField(position, singleElement, content);
                return;
            }
            
            EditorGUI.PropertyField(position, arrayProp, content, true);
        }
    }
}