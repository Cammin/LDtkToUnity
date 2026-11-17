using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkSortingLayerAttribute))]
    internal sealed class LDtkSortingLayerAttributeDrawer : PropertyDrawer
    {
        private static readonly GUIContent SortingLayerLabel = new GUIContent("Sorting Layer", "The sortingLayerID of the renderer");

        private static readonly MethodInfo SortingLayerMethod = typeof(EditorGUI).GetMethod("SortingLayerField",
            BindingFlags.Static | BindingFlags.NonPublic, null,
            new[] { typeof(Rect), typeof(GUIContent), typeof(SerializedProperty), typeof(GUIStyle), typeof(GUIStyle) }, null);
        
        public void DrawProperty(SerializedProperty property)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 18);
            OnGUI(rect, property, SortingLayerLabel);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer) 
            {
                EditorGUI.HelpBox(position, $"{property.name} is not an integer but has [SortingLayer].", MessageType.Error);
                return;
            }
            
            SortingLayerMethod.Invoke(null, new object[] { position, label, property, EditorStyles.popup, EditorStyles.label});
        }
    }
}