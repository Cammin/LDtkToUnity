using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkField))]
    public class LDtkFieldDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty arrayProp = property.FindPropertyRelative(LDtkField.PROP_DATA);
            SerializedProperty isSingleProp = property.FindPropertyRelative(LDtkField.PROP_SINGLE);

            if (isSingleProp.boolValue)
            {
                SerializedProperty singleElement = arrayProp.GetArrayElementAtIndex(0);
                return EditorGUI.GetPropertyHeight(singleElement, label);
            }
            
            return EditorGUI.GetPropertyHeight(arrayProp, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty identifierProp = property.FindPropertyRelative(LDtkField.PROP_IDENTIFIER);
            GUIContent content = new GUIContent(label)
            {
                text = identifierProp.stringValue
            };
            
            SerializedProperty arrayProp = property.FindPropertyRelative(LDtkField.PROP_DATA);
            SerializedProperty isSingleProp = property.FindPropertyRelative(LDtkField.PROP_SINGLE);
            
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