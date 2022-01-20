using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkEditorExtensionMethods
    {
        internal static SerializedProperty DrawField(this SerializedObject obj, string propName)
        {
            SerializedProperty prop = obj.FindProperty(propName);
            EditorGUILayout.PropertyField(prop);
            return prop;
        }
        internal static SerializedProperty DrawField(this SerializedObject obj, string propName, GUIContent content)
        {
            SerializedProperty prop = obj.FindProperty(propName);
            EditorGUILayout.PropertyField(prop, content);
            return prop;
        }
        internal static SerializedProperty DrawField(this SerializedProperty prop, string propName)
        {
            SerializedProperty relProp = prop.FindPropertyRelative(propName);
            EditorGUILayout.PropertyField(relProp);
            return prop;
        }
        internal static SerializedProperty DrawField(this SerializedProperty prop, string propName, GUIContent content)
        {
            SerializedProperty relProp = prop.FindPropertyRelative(propName);
            EditorGUILayout.PropertyField(relProp, content);
            return prop;
        }
    }
}