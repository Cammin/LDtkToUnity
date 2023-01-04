using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkField))]
    internal sealed class LDtkFieldDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private SerializedProperty _arrayProp;
        private SerializedProperty _singleElement;
        private SerializedProperty _isSingleProp;
        private SerializedProperty _identifierProp;
        private SerializedProperty _tooltipProp;

        private GUIContent _guiContent;
        private GUIContent _label;

        private void Init(SerializedProperty property, GUIContent label)
        {
            _property = property;
            _label = label;
            
            _identifierProp = property.FindPropertyRelative(LDtkField.PROPERTY_IDENTIFIER);
            _arrayProp = property.FindPropertyRelative(LDtkField.PROPERTY_DATA);
            _isSingleProp = property.FindPropertyRelative(LDtkField.PROPERTY_SINGLE);
            _tooltipProp = property.FindPropertyRelative(LDtkField.PROPERTY_TOOLTIP);
            
            _guiContent = new GUIContent(label)
            {
                text = _identifierProp.stringValue,
                tooltip = _tooltipProp.stringValue
            };

            _singleElement = _isSingleProp.boolValue ? _arrayProp.GetArrayElementAtIndex(0) : null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property, label);
            
            if (_isSingleProp.boolValue)
            {
                _singleElement = _arrayProp.GetArrayElementAtIndex(0);
                return EditorGUI.GetPropertyHeight(_singleElement, _label);
            }

            _singleElement = null;
            return EditorGUI.GetPropertyHeight(_arrayProp, _label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property, label);
            
            Profiler.BeginSample("LDtkFieldDrawer.OnGUI");
            Draw(position);
            Profiler.EndSample();
        }
        
        private void Draw(Rect position)
        {
            if (_isSingleProp != null && _isSingleProp.boolValue)
            {
                EditorGUI.PropertyField(position, _singleElement, _guiContent);
                return;
            }

            EditorGUI.PropertyField(position, _arrayProp, _guiContent, true);
        }
    }
}