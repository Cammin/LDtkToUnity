using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkField))]
    internal sealed class LDtkFieldDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private SerializedProperty _defProp;
        private SerializedProperty _arrayProp;
        private SerializedProperty _singleElement;
        
        private LDtkDefinitionObjectField _defObject;

        private GUIContent _guiContent;
        private GUIContent _label;

        private bool IsArray => _defObject != null && _defObject.IsArray;

        private void Init(SerializedProperty property, GUIContent label)
        {
            _property = property;
            _label = label;
            
            _defProp = property.FindPropertyRelative(LDtkField.PROPERTY_DEF);
            _defObject = _defProp.objectReferenceValue as LDtkDefinitionObjectField;
            _arrayProp = property.FindPropertyRelative(LDtkField.PROPERTY_DATA);
            _singleElement = IsArray ? null : _arrayProp.GetArrayElementAtIndex(0);
            
            Debug.Assert(_defObject != null, "Field element definition object was null!");
            _guiContent = new GUIContent(label)
            {
                text = _defObject.Identifier,
                tooltip = _defObject.Doc
            };
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property, label);

            if (IsArray)
            {
                return EditorGUI.GetPropertyHeight(_arrayProp, _label);
            }
            
            _singleElement = _arrayProp.GetArrayElementAtIndex(0);
            return EditorGUI.GetPropertyHeight(_singleElement, _label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property, label);
            
            LDtkProfiler.BeginSample("LDtkFieldDrawer.OnGUI");
            Draw(position);
            LDtkProfiler.EndSample();
        }
        
        private void Draw(Rect position)
        {
            Rect fieldRect = new Rect(position);
            Rect labelRect = LDtkEditorGUIUtility.GetLabelRect(position);
            
            if (!IsArray)
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
            LDtkProfiler.BeginSample("LDtkFieldDrawer.DrawDefAndField");

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

            LDtkProfiler.EndSample();
            return true;
        }
    }
}