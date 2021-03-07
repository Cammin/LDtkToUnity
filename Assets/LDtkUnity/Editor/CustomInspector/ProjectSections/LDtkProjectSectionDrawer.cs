using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    public abstract class LDtkProjectSectionDrawer<T> : IDisposable where T : ILDtkIdentifier
    {
        protected abstract string PropertyName { get; }
        protected abstract string GuiText { get; }
        protected abstract string GuiTooltip { get; }
        protected abstract Texture2D GuiImage { get; }
        
        protected readonly SerializedObject SerializedObject;
        protected SerializedProperty ArrayProp;
        private bool _dropdown;

        public bool HasProblem { get; protected set; }
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

        public bool Draw(T[] datas)
        {
            int arraySize = GetSizeOfArray(datas);
            
            if (arraySize <= 0)
            {
                return false;
            }

            if (ArrayProp != null)
            {
                ArrayProp.arraySize = arraySize;
            }

            LDtkDrawerUtil.DrawDivider();
            Rect area = EditorGUILayout.GetControlRect();
            DrawFoldoutArea(area);

            if (_dropdown)
            {
                DrawDropdownContent(datas);
            }
            else if (HasProblem)
            {
                area.xMin += EditorGUIUtility.labelWidth;
                GUI.DrawTexture(area, EditorGUIUtility.IconContent("console.warnicon.sml").image);
            }

            return !HasProblem;
        }

        protected virtual int GetSizeOfArray(T[] datas)
        {
            return datas.Length;
        }
        
        private void DrawFoldoutArea(Rect controlRect)
        {
            
            GUIContent content = new GUIContent()
            {
                text = GuiText,
                tooltip = GuiTooltip,
                image = GuiImage
            };
            
            _dropdown = EditorGUI.Foldout(controlRect, _dropdown, content);
        }
        
        protected abstract void DrawDropdownContent(T[] datas);
    }
}