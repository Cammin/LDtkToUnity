using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkFields))]
    internal sealed class LDtkFieldsEditor : UnityEditor.Editor
    {
        private SerializedProperty[] _elements;
        //private LDtkFieldDrawer[] _drawers;
        
        private static readonly GUIContent HelpBox = new GUIContent()
        {
            text = $"Access with GetComponent<{nameof(LDtkFields)}>, or with a custom component inheriting {nameof(ILDtkImportedFields)}",
        };

        private void OnEnable()
        {
            SerializedProperty fieldsProp = serializedObject.FindProperty(LDtkFields.PROPERTY_FIELDS);
            
            _elements = new SerializedProperty[fieldsProp.arraySize];
            //_drawers = new LDtkFieldDrawer[fieldsProp.arraySize];
            
            for (int i = 0; i < fieldsProp.arraySize; i++)
            {
                _elements[i] = fieldsProp.GetArrayElementAtIndex(i);
                //GUIContent content = new GUIContent(prop.displayName, prop.tooltip);
                //_drawers[i] = new LDtkFieldDrawer(prop, content);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.HelpBox(HelpBox, true);
            
            Profiler.BeginSample("LDtkFieldsEditor DrawElements");
            for (int i = 0; i < _elements.Length; i++)
            {
                SerializedProperty prop = _elements[i];
                EditorGUILayout.PropertyField(prop);
            }
            Profiler.EndSample();

            serializedObject.ApplyModifiedProperties();
        }

    }
}