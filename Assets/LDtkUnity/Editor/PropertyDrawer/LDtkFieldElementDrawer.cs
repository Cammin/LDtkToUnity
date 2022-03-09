using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkFieldElement))]
    internal class LDtkFieldElementDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, Texture2D> _icons = new Dictionary<string, Texture2D>();

        private SerializedProperty _canBeNullProp;
        private SerializedProperty _isNullProp;
        private SerializedProperty _valueProp;

        private static readonly GUIContent NullToggle = new GUIContent()
        {
            tooltip = "Is null"
        };

        private Rect _position;
        private Rect _labelRect;
        private Rect _fieldRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            LDtkFieldType type = GetFieldType(property);
            _valueProp = GetPropertyToDraw(property, type);
            if (_valueProp == null)
            {
                Debug.LogError($"LDtk: Error drawing in the scene for field: {label.text}, serialized property was null");
                return 0;
            }
            
            float propertyHeight = EditorGUI.GetPropertyHeight(_valueProp, label);

            if (type == LDtkFieldType.Multiline)
            {
                propertyHeight *= 3;
            }

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Profiler.BeginSample("LDtkFieldElementDrawer.OnGUI");
            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 14))
            {
                Draw(position, property, label);
            }
            Profiler.EndSample();
        }

        private void Draw(Rect position, SerializedProperty property, GUIContent label)
        {
            Texture2D emptyTex = new Texture2D(1, 1);
            emptyTex.SetPixel(0, 0, Color.clear);
            emptyTex.Apply();
            label.image = emptyTex;

            LDtkFieldType type = GetFieldType(property);
            _isNullProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_NULL);
            _canBeNullProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_CAN_NULL);

            _position = position;
            _labelRect = GetLabelRect(position);
            _fieldRect = GetFieldRect(position, _labelRect);
            //_labelRect.xMin += EditorGUIUtility.singleLineHeight;

            _valueProp = GetPropertyToDraw(property, type);
            if (_valueProp == null)
            {
                Debug.LogError($"LDtk: Error drawing in the scene for field: {label.text}, serialized property was null");
                return;
            }

            if (TryDrawNullable(label, type))
            {
                return;
            }

            if (TryDrawAlternateType(type, label))
            {
                return;
            }


            DrawField(label);
        }

        private bool TryDrawNullable(GUIContent label, LDtkFieldType type)
        {
            if (!CanBeNull(type))
            {
                return false;
            }
            
            Rect boolRect = _position;
            //boolRect.xMin += labelRect.width - position.height;
            boolRect.width = _position.height;
            
            

            
            if (!IsNull(type))
            {
                EditorGUI.PropertyField(boolRect, _isNullProp, NullToggle);
                return false;
            }

            GUIContent nullContent = new GUIContent($"(Null {type})");
            //DrawField(nullContent);
            
            EditorGUI.LabelField(_labelRect, label);
            EditorGUI.LabelField(_fieldRect, nullContent);

            EditorGUI.PropertyField(boolRect, _isNullProp, NullToggle);
            return true;

        }

        private bool TryDrawAlternateType(LDtkFieldType type, GUIContent label)
        {
            Rect betterPos = _position;
            betterPos.xMin += EditorGUIUtility.singleLineHeight;
            switch (type)
            {
                case LDtkFieldType.Multiline:
                {
                    EditorGUI.LabelField(_labelRect, label);
                    _valueProp.stringValue = EditorGUI.TextArea(_fieldRect, _valueProp.stringValue);
                    return true;
                }
                
                case LDtkFieldType.EntityRef:
                {
                    string iid = _valueProp.stringValue;
                
                    if (string.IsNullOrEmpty(iid))
                    {
                        return false;
                    }

                    LDtkIid component = LDtkIidComponentBank.FindObjectOfIid(iid);
                    if (component == null)
                    {
                        return false;
                    }

                    float desiredObjectWidth = 175;

                    float objectWidth = Mathf.Min(desiredObjectWidth, _position.width - desiredObjectWidth * 0.83f);
                    float stringWidth = _position.width - objectWidth;
                
                    Rect amountRect = new Rect(_position.x, _position.y, stringWidth - 2, _position.height);
                    Rect objectRect = new Rect(_position.x + stringWidth, _position.y, Mathf.Max(desiredObjectWidth, objectWidth), _position.height);

                    amountRect.xMin = _labelRect.xMin;
                    
                    _labelRect = amountRect;
                    _fieldRect.width -= objectRect.width;
                    DrawField(label);
                    
                    //EditorGUI.PropertyField(amountRect, _valueProp, label);
                    
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUI.ObjectField(objectRect, component.gameObject, typeof(GameObject), true);//todo figure out this object field's width
                    }
                
                    return true;
                }
                case LDtkFieldType.Tile:
                {
                    Sprite spr = (Sprite)_valueProp.objectReferenceValue;
                    if (spr == null)
                    {
                        return false;
                    }
                    
                    string key = label.text;
                    if (!_icons.ContainsKey(key))
                    {
                        Texture2D tex = AssetPreview.GetAssetPreview(spr);
                        _icons.Add(key, tex);
                    }
                
                    GUIContent content = new GUIContent(label);
                    if (_icons.ContainsKey(key))
                    {
                        //content.image = _icons[key];
                    }
                
                    DrawField(content);
                    return true;
                }
                default:
                    return false;
            }
        }

        private void DrawField(GUIContent content)
        {
            EditorGUI.LabelField(_labelRect, content);
            EditorGUI.PropertyField(_fieldRect, _valueProp, GUIContent.none);
        }

        private static Rect GetFieldRect(Rect position, Rect labelRect)
        {
            Rect fieldRect = new Rect(position);
            fieldRect.x = labelRect.xMax;
            fieldRect.width = Mathf.Max(EditorGUIUtility.fieldWidth, position.width - labelRect.width);
            return fieldRect;
        }

        private static Rect GetLabelRect(Rect position)
        {
            Rect labelRect = new Rect(position);
            labelRect.width = EditorGUIUtility.labelWidth + 2;
            return labelRect;
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
                
                case LDtkFieldType.Bool:
                    return LDtkFieldElement.PROPERTY_BOOL;
                
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                case LDtkFieldType.EntityRef:
                    return LDtkFieldElement.PROPERTY_STRING;
                
                case LDtkFieldType.Color:
                    return LDtkFieldElement.PROPERTY_COLOR;
                
                case LDtkFieldType.Point:
                    return LDtkFieldElement.PROPERTY_VECTOR2;

                case LDtkFieldType.Tile:
                    return LDtkFieldElement.PROPERTY_SPRITE;
            }

            return null;
        }

        private bool CanBeNull(LDtkFieldType type)
        {
            if (!_canBeNullProp.boolValue)
            {
                return false;
            }
            
            switch (type)
            {
                case LDtkFieldType.None:
                case LDtkFieldType.Int:
                case LDtkFieldType.Float:
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                case LDtkFieldType.Point:
                case LDtkFieldType.EntityRef:
                case LDtkFieldType.Tile: //for any unity engine object references, it will check if the value is actually null instead 
                    return true;

                case LDtkFieldType.Bool:
                case LDtkFieldType.Color:
                default:
                    return false;
            }
        }
        public bool IsNull(LDtkFieldType type)
        {
            switch (type)
            {
                case LDtkFieldType.None:
                    return true;
                    
                case LDtkFieldType.Bool: //these are values that are never able to be null from ldtk
                case LDtkFieldType.Color:
                    return false;

                //for our use cases, it's null when the value was set as null from the parsed data types.
                case LDtkFieldType.Int:
                case LDtkFieldType.Float:
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                case LDtkFieldType.Point:
                case LDtkFieldType.EntityRef:
                case LDtkFieldType.Tile:
                    return !_isNullProp.boolValue;

                default:
                    return false;
            }
        }
    }
}