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
        private static readonly Dictionary<string, Texture2D> Icons = new Dictionary<string, Texture2D>();
        private static Texture2D _blankSquareTex; 

        private SerializedProperty _canBeNullProp;
        private SerializedProperty _isNullProp;
        private SerializedProperty _valueProp;
        private Rect _position;
        private Rect _labelRect;
        private Rect _fieldRect;

        private static readonly GUIContent NullToggle = new GUIContent()
        {
            tooltip = "Is null"
        };
        
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

            _isNullProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_NULL);
            if (type == LDtkFieldType.Multiline && _isNullProp.boolValue)
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
            Profiler.BeginSample("LDtkFieldElementDrawer.Draw");
            
            TryInitTex();

            label.image = _blankSquareTex;

            LDtkFieldType type = GetFieldType(property);
            _isNullProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_NULL);
            _canBeNullProp = property.FindPropertyRelative(LDtkFieldElement.PROPERTY_CAN_NULL);
            
            _position = position;
            _labelRect = GetLabelRect(position);
            _fieldRect = GetFieldRect(position, _labelRect);
            //_labelRect.xMin += EditorGUIUtility.singleLineHeight;


            _valueProp = GetPropertyToDraw(property, type);
            Profiler.EndSample();
            
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

            Profiler.BeginSample("LDtkFieldElementDrawer.PropertyField");
            EditorGUI.PropertyField(_position, _valueProp, label);
            Profiler.EndSample();
        }
        
        private static void TryInitTex()
        {
            if (_blankSquareTex != null)
            {
                return;
            }
            
            _blankSquareTex = new Texture2D(1, 1);
            _blankSquareTex.SetPixel(0, 0, Color.clear);
            _blankSquareTex.Apply();
        }

        private bool TryDrawNullable(GUIContent label, LDtkFieldType type)
        {
            Profiler.BeginSample("LDtkFieldElementDrawer.TryDrawNullable");
            if (!CanBeNull(type))
            {
                
                Profiler.EndSample();
                return false;
            }
            
            Rect boolRect = _position;
            //boolRect.xMin += labelRect.width - position.height;
            boolRect.width = EditorGUIUtility.singleLineHeight;
            boolRect.height = EditorGUIUtility.singleLineHeight;
            
            if (!IsNull(type))
            {
                EditorGUI.PropertyField(boolRect, _isNullProp, NullToggle);
                
                Profiler.EndSample();
                return false;
            }

            GUIContent nullContent = new GUIContent($"(Null {type})");

            EditorGUI.PropertyField(boolRect, _isNullProp, NullToggle);
            EditorGUI.LabelField(_labelRect, label);
            EditorGUI.LabelField(_fieldRect, nullContent);

            Profiler.EndSample();
            return true;
        }

        private bool TryDrawAlternateType(LDtkFieldType type, GUIContent label)
        {
            switch (type)
            {
                case LDtkFieldType.Multiline:
                {
                    return DrawMultiline(label);
                }
                
                case LDtkFieldType.EntityRef:
                {
                    return DrawEntityRef(label);
                }
                
                case LDtkFieldType.Tile:
                {
                    return DrawTileField(label);
                }
                
                default:
                    return false;
            }
        }

        private bool DrawMultiline(GUIContent label)
        {
            Profiler.BeginSample("LDtkFieldElementDrawer.DrawMultiline");
            
            GUIStyle labelStyle = _valueProp.prefabOverride ? EditorStyles.boldLabel : EditorStyles.label;
            
            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.fontStyle = _valueProp.prefabOverride ? FontStyle.Bold : FontStyle.Normal;
            
            _labelRect.height *= 0.33f;
            
            EditorGUI.LabelField(_labelRect, label, labelStyle);

            _valueProp.stringValue = EditorGUI.TextArea(_fieldRect, _valueProp.stringValue, textAreaStyle);
            
            Profiler.EndSample();
            return true;
        }

        private bool DrawEntityRef(GUIContent label)
        {
            Profiler.BeginSample("LDtkFieldElementDrawer.DrawEntityRef");
            
            string iid = _valueProp.stringValue;

            if (string.IsNullOrEmpty(iid))
            {
                Profiler.EndSample();
                return false;
            }

            LDtkIid component = LDtkIidComponentBank.FindObjectOfIid(iid);
            if (component == null)
            {
                Profiler.EndSample();
                return false;
            }

            float desiredObjectWidth = 175;

            float objectWidth = Mathf.Min(desiredObjectWidth, _position.width - desiredObjectWidth * 0.83f);
            float fieldWidth = Mathf.Max(_position.width - objectWidth);

            Rect fieldRect = new Rect(_position.x, _position.y, fieldWidth - 2, _position.height);
            Rect gameObjectRect = new Rect(_position.x + fieldWidth, _position.y, Mathf.Max(desiredObjectWidth, objectWidth), _position.height);

            fieldRect.xMin = _labelRect.xMin;

            _labelRect = fieldRect;
            _fieldRect.width -= gameObjectRect.width;
            //DrawField(label);

            EditorGUI.PropertyField(fieldRect, _valueProp, label);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.ObjectField(gameObjectRect, component.gameObject, typeof(GameObject), true); //todo figure out this object field's width
            }

            Profiler.EndSample();
            return true;
        }

        private bool DrawTileField(GUIContent label)
        {
            Sprite spr = (Sprite)_valueProp.objectReferenceValue;
            if (spr == null)
            {
                return false;
            }

            string key = label.text;
            
            GUIContent content = new GUIContent(label);
            if (!_canBeNullProp.boolValue)
            {
                if (!Icons.ContainsKey(key))
                {
                    Texture2D tex = AssetPreview.GetAssetPreview(spr);
                    Icons.Add(key, tex);
                }
                
                if (Icons.ContainsKey(key))
                {
                    Texture2D tex = Icons[key];
                    content.image = tex;
                }
            }
            
            Profiler.BeginSample("LDtkFieldElementDrawer.DrawTileField");
            EditorGUI.PropertyField(_position, _valueProp, content);
            Profiler.EndSample();
            
            return true;
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