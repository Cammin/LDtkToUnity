using LDtkUnity.FieldInjection;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkFieldAttribute))]
    public class LDtkFieldAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.image = LDtkIconLoader.LoadSimpleIcon();
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
