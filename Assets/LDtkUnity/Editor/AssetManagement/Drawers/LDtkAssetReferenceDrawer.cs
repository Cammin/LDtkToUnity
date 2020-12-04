using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.UnityAssets;
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
        
        protected void DrawField(Rect controlRect)
        {
            float labelWidth = LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };
            EditorGUI.PropertyField(fieldRect, Property, GUIContent.none);
        }
        
        protected void DrawFieldAndObject(Rect controlRect)
        {
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
        
        protected override void DrawSelfSimple(Rect controlRect, Texture2D iconTex, TData data)
        {
            base.DrawSelfSimple(controlRect, iconTex, data);
            DrawFieldAndObject(controlRect);
        }
    }
}