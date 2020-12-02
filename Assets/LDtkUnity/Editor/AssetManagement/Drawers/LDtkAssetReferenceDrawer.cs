using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.Entity;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public abstract class LDtkAssetReferenceDrawer<TData> : LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier

    {
        protected readonly SerializedProperty Property;
        
        protected LDtkAssetReferenceDrawer(SerializedProperty asset)
        {
            Property = asset;
        }
        
        protected void DrawField(Rect controlRect, TData definition)
        {
            float labelWidth = LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };
            
            EditorGUI.ObjectField(fieldRect, Property);
        }
        
        protected override void DrawSelfSimple(Rect controlRect, Texture2D iconTex, TData data)
        {
            base.DrawSelfSimple(controlRect, iconTex, data);
            DrawField(controlRect, data);
        }
    }
}