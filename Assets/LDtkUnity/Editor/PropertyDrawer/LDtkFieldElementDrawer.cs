using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkFieldElement))]
    public class LDtkFieldElementDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeProp = property.FindPropertyRelative(LDtkFieldElement.PROP_TYPE);
            
            Array values = Enum.GetValues(typeof(LDtkFieldType));
            LDtkFieldType type = (LDtkFieldType)values.GetValue(typeProp.enumValueIndex);

            string propName = GetPropertyNameForType(type);
            if (propName == null)
            {
                EditorGUI.LabelField(position, label);
                return;
            }
            
            SerializedProperty propToDraw = property.FindPropertyRelative(propName);

            EditorGUI.PropertyField(position, propToDraw, label);
        }

        private string GetPropertyNameForType(LDtkFieldType type)
        {
            switch (type)
            {
                case LDtkFieldType.Int:
                    return LDtkFieldElement.PROP_INT;
                
                case LDtkFieldType.Float:
                    return LDtkFieldElement.PROP_FLOAT;
                
                case LDtkFieldType.Boolean:
                    return LDtkFieldElement.PROP_BOOL;
                
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                    return LDtkFieldElement.PROP_STRING;
                
                case LDtkFieldType.Color:
                    return LDtkFieldElement.PROP_COLOR;
                
                case LDtkFieldType.Point:
                    return LDtkFieldElement.PROP_VECTOR2;
            }

            return null;
        }
    }
}