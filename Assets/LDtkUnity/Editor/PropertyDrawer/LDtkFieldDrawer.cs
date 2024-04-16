using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkField))]
    internal sealed class LDtkFieldDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private SerializedProperty _defProp;
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
            
            _defProp = property.FindPropertyRelative(LDtkField.PROPERTY_DEF);
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
            Rect fieldRect = new Rect(position);
            Rect labelRect = LDtkEditorGUIUtility.GetLabelRect(position);
            
            if (_isSingleProp != null && _isSingleProp.boolValue)
            {
                DrawDefAndField(position,labelRect, _singleElement, _guiContent);
                return;
            }

            //when drawing an array, we can fit the definition object's field into the first line's bar
            DrawDefAndField(fieldRect, labelRect, _arrayProp, _guiContent, true);
        }

        /// <summary>
        /// For single fields that need to fit everything in the same line
        /// </summary>
        public bool DrawDefAndField(Rect position, Rect labelRect, SerializedProperty fieldProp, GUIContent label, bool includeChildren = false)
        {
            Profiler.BeginSample("LDtkFieldDrawer.DrawDefAndField");

            const float desiredObjectWidth = 35;
            float objectWidth = Mathf.Min(desiredObjectWidth, position.width - desiredObjectWidth * 0.83f);
            float fieldWidth = Mathf.Max(position.width - objectWidth);
            
            Rect fieldRect = new Rect(
                position.x, 
                position.y, 
                fieldWidth - 2, 
                position.height);
            
            Rect objRect = new Rect(
                position.x + fieldWidth, 
                position.y, 
                Mathf.Max(desiredObjectWidth, objectWidth), 
                EditorGUIUtility.singleLineHeight);

            fieldRect.xMin = labelRect.xMin;

            EditorGUI.PropertyField(fieldRect, fieldProp, label, includeChildren);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.ObjectField(objRect, _defProp.objectReferenceValue, typeof(LDtkDefinitionObjectField), true);
            }

            Profiler.EndSample();
            return true;
        }
    }
}