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
            if (propToDraw == null)
            {
                Debug.LogError($"LDtk: Error drawing in the scene for field: {label.text}, serialized property was null");
                return 0;
            }
            
            
            float propertyHeight = EditorGUI.GetPropertyHeight(propToDraw, label);

            if (type == LDtkFieldType.FText) //todo multiline in LDtk json is currently of type string, waiting on LDtk update fix
            {
                propertyHeight *= 3;
            }

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LDtkFieldType type = GetFieldType(property);
            SerializedProperty propToDraw = GetPropertyToDraw(property, type);
            if (propToDraw == null)
            {
                Debug.LogError($"LDtk: Error drawing in the scene for field: {label.text}, serialized property was null");
                return;
            }

            if (TryDrawAlternateType(type, position, propToDraw, label))
            {
                return;
            }
            
            EditorGUI.PropertyField(position, propToDraw, label);
        }

        private bool TryDrawAlternateType(LDtkFieldType type, Rect position, SerializedProperty propToDraw, GUIContent label)
        {
            if (type == LDtkFieldType.FText)
            {
                //todo handle this when types can be properly detected as multi-lines
            }
            
            if (type == LDtkFieldType.FEntityRef)
            {
                string iid = propToDraw.stringValue;
                
                if (string.IsNullOrEmpty(iid))
                {
                    return false;
                }

                LDtkComponentIid component = LDtkIidComponentBank.FindObjectOfIid(iid);
                if (component == null)
                {
                    return false;
                }

                float desiredObjectWidth = 175;

                float objectWidth = Mathf.Min(desiredObjectWidth, position.width - desiredObjectWidth * 0.83f);
                float stringWidth = position.width - objectWidth;
                
                Rect amountRect = new Rect(position.x, position.y, stringWidth - 2, position.height);
                Rect objectRect = new Rect(position.x + stringWidth, position.y, Mathf.Max(desiredObjectWidth, objectWidth), position.height);
                
                EditorGUI.PropertyField(amountRect, propToDraw, label);
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.ObjectField(objectRect, component.gameObject, typeof(GameObject), true);//todo figure out this object field's width
                }
                
                return true;
            }

            return false;
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
                case LDtkFieldType.FInt:
                    return LDtkFieldElement.PROPERTY_INT;
                
                case LDtkFieldType.FFloat:
                    return LDtkFieldElement.PROPERTY_FLOAT;
                
                case LDtkFieldType.FBool:
                    return LDtkFieldElement.PROPERTY_BOOL;
                
                case LDtkFieldType.FString:
                case LDtkFieldType.FText:
                case LDtkFieldType.FPath:
                case LDtkFieldType.FEnum:
                case LDtkFieldType.FEntityRef:
                    return LDtkFieldElement.PROPERTY_STRING;
                
                case LDtkFieldType.FColor:
                    return LDtkFieldElement.PROPERTY_COLOR;
                
                case LDtkFieldType.FPoint:
                    return LDtkFieldElement.PROPERTY_VECTOR2;

                case LDtkFieldType.FTile:
                    return LDtkFieldElement.PROPERTY_SPRITE;
            }

            return null;
        }
    }
}