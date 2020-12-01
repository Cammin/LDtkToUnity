using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.UnityAssets.Entity;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public abstract class LDtkAssetReferenceDrawer<TData, TAsset> : LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier
    {
        protected TAsset Asset { get; set; }

        protected LDtkAssetReferenceDrawer(TData data, TAsset asset)
        {
            Asset = asset;
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
            
            LDtkEntityAsset t = null;
            t = (LDtkEntityAsset)EditorGUI.ObjectField(fieldRect, t, typeof(LDtkEntityAsset), false);
        }
        
        protected override void DrawSelfSimple(Rect controlRect, Texture2D iconTex, TData data)
        {
            base.DrawSelfSimple(controlRect, iconTex, data);
            DrawField(controlRect, data);
        }
    }
}