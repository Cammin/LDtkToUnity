using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry
    /// </summary>
    public abstract class LDtkContentDrawer<TData> where TData : ILDtkIdentifier
    {
        protected TData _data;

        protected LDtkContentDrawer(TData data)
        {
            _data = data;
        }

        public virtual void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(controlRect, _data.Identifier);
        }


        public virtual bool HasProblem()
        {
            return false;
        }

        protected bool DrawButtonToLeftOfField(Rect controlRect, string iconContent, string tooltip, int indentLevel = 0)
        {
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            
            Rect buttonRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth - controlRect.height*(indentLevel+1),
                
                width = controlRect.height,
                height = controlRect.height,
            };
            bool isPressed = GUI.Button(buttonRect, GUIContent.none);

            if (!string.IsNullOrEmpty(iconContent))
            {
                GUIContent refreshImage = EditorGUIUtility.IconContent(iconContent);
                if (refreshImage != null && refreshImage.image != null)
                {
                    Rect imageArea = new Rect(buttonRect)
                    {
                        width = buttonRect.width - 2,//refreshImage.image.width,
                        height = buttonRect.height - 2,
                        center = buttonRect.center
                    };

                    GUIContent tooltipContent = new GUIContent()
                    {
                        tooltip = tooltip
                    };
                
                
                    GUI.Label(imageArea, tooltipContent);
                    GUI.DrawTexture(imageArea, refreshImage.image);
            
                }
            }
            
            return isPressed;
        }
    }
}