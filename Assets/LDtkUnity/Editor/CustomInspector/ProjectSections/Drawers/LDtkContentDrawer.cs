using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry
    /// </summary>
    public abstract class LDtkContentDrawer<TData> where TData : ILDtkIdentifier
    {
        public void Draw(TData definition)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawInternal(controlRect, definition);
        }

        protected abstract void DrawInternal(Rect controlRect, TData data);

        

        protected bool DrawButtonToLeftOfField(Rect controlRect, string iconContent, string tooltip)
        {
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            
            Rect buttonRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth - controlRect.height,
                
                width = controlRect.height,
                height = controlRect.height,
            };
            bool isPressed = GUI.Button(buttonRect, GUIContent.none);

            if (!string.IsNullOrEmpty(iconContent))
            {
                GUIContent refreshImage = EditorGUIUtility.IconContent(iconContent);
                if (refreshImage != null)
                {
                    Rect imageArea = new Rect(buttonRect)
                    {
                        width = refreshImage.image.width,
                        height = refreshImage.image.height,
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