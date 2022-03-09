using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    //[CustomPropertyDrawer(typeof(LDtkField))]
    internal struct LDtkFieldDrawer
    {

        private SerializedProperty _property;
        private readonly SerializedProperty _arrayProp;
        private readonly SerializedProperty _singleElement;
        private readonly SerializedProperty _isSingleProp;
        private readonly SerializedProperty _identifierProp;

        private readonly GUIContent _guiContent;
        private readonly GUIContent _label;

        public float PropertyHeight;

        public LDtkFieldDrawer(SerializedProperty property, GUIContent label)
        {
            _property = property;
            _label = label;
            
            _identifierProp = property.FindPropertyRelative(LDtkField.PROPERTY_IDENTIFIER);
            _arrayProp = property.FindPropertyRelative(LDtkField.PROPERTY_DATA);
            _isSingleProp = property.FindPropertyRelative(LDtkField.PROPERTY_SINGLE);


            _guiContent = new GUIContent(label)
            {
                text = _identifierProp.stringValue
            };

            if (_isSingleProp.boolValue)
            {
                _singleElement = _arrayProp.GetArrayElementAtIndex(0);
                PropertyHeight = EditorGUI.GetPropertyHeight(_singleElement, _label);
            }
            else
            {
                _singleElement = null;
                PropertyHeight = EditorGUI.GetPropertyHeight(_arrayProp, _label);
            }
        }

        public void OnGUI(Rect position)
        {
            Profiler.BeginSample("LDtkFieldDrawer.OnGUI");
            Draw(position);
            Profiler.EndSample();
        }

        private void Draw(Rect position)
        {
            if (_isSingleProp.boolValue)
            {
                EditorGUI.PropertyField(position, _singleElement, _guiContent);
                return;
            }

            EditorGUI.PropertyField(position, _arrayProp, _guiContent, true);
        }
    }
}