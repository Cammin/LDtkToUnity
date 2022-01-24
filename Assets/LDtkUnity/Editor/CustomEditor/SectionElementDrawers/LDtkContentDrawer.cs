using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry
    /// </summary>
    internal abstract class LDtkContentDrawer<TData> where TData : ILDtkIdentifier
    {
        protected TData _data;

        protected LDtkContentDrawer(TData data)
        {
            _data = data;
        }

        public virtual void Draw()
        {
            EditorGUILayout.LabelField(_data.Identifier);
        }

        protected bool DrawButtonToLeftOfField(Rect controlRect, GUIContent content, int indentLevel = 0)
        {
            float labelWidth = LDtkEditorGUIUtility.LabelWidth(controlRect.width);
            
            Rect buttonRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth - controlRect.height*(indentLevel+1),
                
                width = controlRect.height,
                height = controlRect.height,
            };
            bool isPressed = GUI.Button(buttonRect, GUIContent.none);

            if (content == null || content.image == null)
            {
                return isPressed;
            }
            
            Rect imageArea = new Rect(buttonRect)
            {
                width = buttonRect.width - 2,
                height = buttonRect.height - 2,
                center = buttonRect.center
            };

            GUIContent tooltipContent = new GUIContent()
            {
                tooltip = content.tooltip
            };
                
                
            GUI.Label(imageArea, tooltipContent);
            GUI.DrawTexture(imageArea, content.image);

            return isPressed;
        }
    }
}