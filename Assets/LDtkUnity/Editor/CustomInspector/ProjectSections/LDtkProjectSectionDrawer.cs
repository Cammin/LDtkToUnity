using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkProjectSectionDrawer<T> : IDisposable where T : ILDtkIdentifier
    {
        protected abstract string PropertyName { get; }
        protected abstract string GuiText { get; }
        protected abstract string GuiTooltip { get; }
        protected abstract Texture2D GuiImage { get; }
        
        protected readonly SerializedObject SerializedObject;
        protected SerializedProperty ArrayProp;
        private bool _dropdown;

        protected LDtkProject Project => (LDtkProject) SerializedObject.targetObject;

        protected LDtkProjectSectionDrawer(SerializedObject serializedObject)
        {
            SerializedObject = serializedObject;
        }

        public void Init()
        {
            ArrayProp = SerializedObject.FindProperty(PropertyName);
            _dropdown = EditorPrefs.GetBool(PropertyName, true);
        }
        public void Dispose()
        {
            EditorPrefs.SetBool(PropertyName, _dropdown);
        }

        public void Draw(T[] datas)
        {
            int arraySize = GetSizeOfArray(datas);
            
            if (arraySize <= 0)
            {
                return;
            }

            if (ArrayProp != null)
            {
                ArrayProp.arraySize = arraySize;
            }
            
            DrawDropdownArea();

            
            if (_dropdown)
            {
                DrawDropdownContent(datas);
            }
        }

        protected virtual int GetSizeOfArray(T[] datas)
        {
            return datas.Length;
        }
        
        private Rect DrawDropdownArea()
        {
            LDtkDrawerUtil.DrawDivider();
            GUIContent content = new GUIContent()
            {
                text = GuiText,
                tooltip = GuiTooltip,
                image = GuiImage
            };
            Rect area = EditorGUILayout.GetControlRect();
            _dropdown = EditorGUI.Foldout(area, _dropdown, content);
            return area;
        }
        
        protected abstract void DrawDropdownContent(T[] datas);
    }
}