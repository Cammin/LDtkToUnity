using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Simply displaying parts of an LDtk project
    /// </summary>
    public abstract class LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier
    {
        public void Draw(TData definition)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawInternal(controlRect, definition);
        }

        protected abstract void DrawInternal(Rect controlRect, TData data);
        
        
        
        protected void DrawIconAndLabel(Rect controlRect, Texture2D iconTex, TData item)
        {
            DrawLeftIcon(controlRect, iconTex);
            DrawLabel(controlRect, item.Identifier);
        }
        
        protected Rect DrawLeftIcon(Rect controlRect, Texture2D icon)
        {
            Rect iconRect = GetLeftIconRect(controlRect);
            GUI.DrawTexture(iconRect, icon);
            return iconRect;
        }
        
        protected Rect GetLeftIconRect(Rect controlRect)
        {
            Rect textureRect = new Rect(controlRect)
            {
                width = controlRect.height
            };
            return textureRect;
        }
        
        protected void DrawIndentedLabel(Rect controlRect, string label)
        {
            controlRect.xMin += controlRect.height;
            EditorGUI.LabelField(controlRect, label);
        }
        protected void DrawLabel(Rect controlRect, string label)
        {
            EditorGUI.LabelField(controlRect, label);
        }
        
        protected bool DrawRightFieldIconButton(Rect controlRect, string iconContent, string tooltip)
        {
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            
            Rect buttonRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth - controlRect.height,
                
                width = controlRect.height,
                height = controlRect.height,
            };
            bool isPressed = GUI.Button(buttonRect, GUIContent.none);

            Texture refreshImage = EditorGUIUtility.IconContent(iconContent).image;
            if (refreshImage == null)
            {
                Debug.LogError("Refresh image null");
                return false;
            }
            
            Rect imageContent = new Rect(buttonRect)
            {
                width = refreshImage.width,
                height = refreshImage.height,
                center = buttonRect.center
            }; 
            
            GUI.DrawTexture(imageContent, refreshImage);
            

            return isPressed;
        }
    }
}