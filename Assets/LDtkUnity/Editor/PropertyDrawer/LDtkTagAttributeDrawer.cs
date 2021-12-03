using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    [CustomPropertyDrawer(typeof(LDtkTagAttribute))]
    public class LDtkTagAttributeDrawer : PropertyDrawer
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