using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkFieldElement))]
    internal class LDtkFieldElementDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            LDtkFieldType type = GetFieldType(property);
            SerializedProperty propToDraw = GetPropertyToDraw(property, type);
            float propertyHeight = EditorGUI.GetPropertyHeight(propToDraw, label);

            if (type == LDtkFieldType.Multiline) //todo multiline in LDtk json is currently of type string, waiting on LDtk update fix
            {
                propertyHeight *= 3;
            }

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LDtkFieldType type = GetFieldType(property);
            SerializedProperty propToDraw = GetPropertyToDraw(property, type);
            EditorGUI.PropertyField(position, propToDraw, label);
        }

        private SerializedProperty GetPropertyToDraw(SerializedProperty property, LDtkFieldType type)
        {
            string propName = GetPropertyNameForType(type);
            return property.FindPropertyRelative(propName);
        }

        private LDtkFieldType GetFieldType(SerializedProperty property)
        {
            SerializedProperty typeProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_TYPE);
            Array values = Enum.GetValues(typeof(LDtkFieldType));
            return (LDtkFieldType)values.GetValue(typeProp.enumValueIndex);
        }

        private string GetPropertyNameForType(LDtkFieldType type)
        {
            switch (type)
            {
                case LDtkFieldType.Int:
                    return LDtkFieldElement.PROPERTY_INT;
                
                case LDtkFieldType.Float:
                    return LDtkFieldElement.PROPERTY_FLOAT;
                
                case LDtkFieldType.Boolean:
                    return LDtkFieldElement.PROPERTY_BOOL;
                
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                    return LDtkFieldElement.PROPERTY_STRING;
                
                case LDtkFieldType.Color:
                    return LDtkFieldElement.PROPERTY_COLOR;
                
                case LDtkFieldType.Point:
                    return LDtkFieldElement.PROPERTY_VECTOR2;
            }

            return null;
        }
    }
}