using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerIntGridValue : LDtkAssetReferenceDrawer<LDtkDefinitionIntGridValue>
    {

        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionIntGridValue data)
        {
            controlRect.x += 15;
            Rect iconRect = GetLeftIconRect(controlRect);
            
            EditorGUI.DrawRect(iconRect, data.Color);
            DrawLabel(controlRect, data);
            
            controlRect.x -= 15;
            DrawField(controlRect, data);
        }

        public LDtkReferenceDrawerIntGridValue(SerializedProperty asset) : base(asset)
        {
        }
    }
}