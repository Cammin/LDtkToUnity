using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkTagAttribute))]
    internal sealed class LDtkTagAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}