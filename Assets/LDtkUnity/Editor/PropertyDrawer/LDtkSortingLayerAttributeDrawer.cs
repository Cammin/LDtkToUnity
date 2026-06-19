using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkSortingLayerAttribute))]
    internal sealed class LDtkSortingLayerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer) 
            {
                EditorGUI.HelpBox(position, $"{property.name} is not an integer but has [SortingLayer].", MessageType.Error);
                return;
            }

            SortingLayer[] layers = SortingLayer.layers;
            string[] sortingLayerNames = layers.Select(p => p.name).ToArray();
            string[] sortingLayerVisualNames = layers.Select(p => $"{p.value}: {p.name}" ).ToArray();

            EditorGUI.BeginProperty(position, label, property);

            string oldName = SortingLayer.IDToName(property.intValue);
            int oldLayerIndex = Array.IndexOf(sortingLayerNames, oldName);
            int newLayerIndex = EditorGUI.Popup(position, label.text, oldLayerIndex, sortingLayerVisualNames);

            if (newLayerIndex != oldLayerIndex) 
            {
                property.intValue = SortingLayer.NameToID(sortingLayerNames[newLayerIndex]);
            }
            EditorGUI.EndProperty();
        }
    }
}