using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkFieldAttribute))]
    public class LDtkFieldAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent content = new GUIContent(label)
            {
                tooltip = "This field is set by LDtk",
                image = LDtkIconUtility.LoadSimpleIcon()
            };

            EditorGUI.PropertyField(position, property, content);
        }
    }
}
