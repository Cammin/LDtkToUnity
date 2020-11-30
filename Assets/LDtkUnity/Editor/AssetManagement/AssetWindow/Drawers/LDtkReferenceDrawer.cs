using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Settings;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public abstract class LDtkReferenceDrawer<T> where T : ILDtkIdentifier
    {
        protected abstract Texture2D FieldIcon { get; }
        
        public void Draw(T definition)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            
            const int indent = 0;
            controlRect.xMin += indent;

            Rect textureRect = new Rect(controlRect)
            {
                width = controlRect.height
            };
            GUI.DrawTexture(textureRect, FieldIcon);

            controlRect.xMin += textureRect.width;

            Rect labelRect = new Rect(controlRect)
            {
                width = Mathf.Max(controlRect.width/2, EditorGUIUtility.labelWidth) - EditorGUIUtility.fieldWidth
            };
            EditorGUI.LabelField(labelRect, definition.identifier);
            
            Rect fieldRect = new Rect(controlRect)
            {
                x = labelRect.xMax,
                width = Mathf.Max(controlRect.width - labelRect.width, EditorGUIUtility.fieldWidth)
            };
            
            DrawField(fieldRect, definition);
        }

        protected abstract void DrawField(Rect fieldRect, T data);
    }
}