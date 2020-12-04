using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets;
using LDtkUnity.Runtime.UnityAssets.IntGridValue;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerIntGridValue : LDtkAssetReferenceDrawer<LDtkDefinitionIntGridValue>
    {
        public LDtkReferenceDrawerIntGridValue(SerializedProperty asset) : base(asset)
        {
        }
        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionIntGridValue data)
        {
            controlRect.x += 15;
            Rect iconRect = GetLeftIconRect(controlRect);
            
            EditorGUI.DrawRect(iconRect, data.Color);
            DrawLabel(controlRect, data);
            
            controlRect.x -= 15;
            //DrawField(controlRect);
            
            //TODO this is a shameless copypaste from base class, clean up later
            float labelWidth = LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };
            Rect halfLeft = new Rect(fieldRect)
            {
                width = fieldRect.width * 0.5f
            };
            Rect halfRight = new Rect(halfLeft)
            {
                x = fieldRect.x + fieldRect.width/2
            };
            
            
            
            EditorGUI.PropertyField(halfLeft, Property, GUIContent.none);
            if (Property.objectReferenceValue != null)
            {
                GUI.enabled = false;
                SerializedProperty assetProp = new SerializedObject(Property.objectReferenceValue).FindProperty(LDtkAsset<Object>.PROP_ASSET);
                EditorGUI.PropertyField(halfRight, assetProp, GUIContent.none);
                GUI.enabled = true;
            }
        }


    }
}