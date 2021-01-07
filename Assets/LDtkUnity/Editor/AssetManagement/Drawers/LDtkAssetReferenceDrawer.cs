using LDtkUnity.Data;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// For the assignable assets 
    /// </summary>
    public abstract class LDtkAssetReferenceDrawer<TData> : LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier
    {
        protected readonly SerializedProperty Property;
        public bool HasProblem { get; private set; } = false;
        
        protected LDtkAssetReferenceDrawer(SerializedProperty asset)
        {
            Property = asset;
        }
        
        protected void DrawField(Rect controlRect)
        {
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };
            EditorGUI.PropertyField(fieldRect, Property, GUIContent.none);
        }

        protected Rect GetFieldRect(Rect controlRect)
        {
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            return new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };
        }
        
        protected void DrawFieldAndObject(Rect controlRect, TData data)
        {
            Rect fieldRect = GetFieldRect(controlRect);

            float halfWidth = fieldRect.width * 0.5f;
            Rect halfLeft = new Rect(fieldRect)
            {
                width = Mathf.CeilToInt(halfWidth)
            };
            Rect halfRight = new Rect(halfLeft)
            {
                x = fieldRect.x + halfWidth,
                width = halfWidth
            };
            
            EditorGUI.PropertyField(halfLeft, Property, GUIContent.none);

            Object propertyReference = Property.objectReferenceValue;
            if (propertyReference != null)
            {
                GUI.enabled = false;
                SerializedProperty assetProp = new SerializedObject(propertyReference).FindProperty(LDtkAsset<Object>.PROP_ASSET);
                EditorGUI.PropertyField(halfRight, assetProp, GUIContent.none);
                GUI.enabled = true;

                Object assetPropertyReference = assetProp.objectReferenceValue;
                if (assetPropertyReference == null)
                {
                    string typeName = assetProp.type.Replace("PPtr<$", "").Replace(">", "");
                    ThrowWarning(controlRect, $"{propertyReference.name}'s {typeName} is not assigned");
                }
                else if (propertyReference.name != data.Identifier)
                {
                    ThrowError(controlRect, $"Asset's name does not match the LDtk data's identifier: \"{data.Identifier}\". They must be identical.");
                }
            }
            else
            {
                ThrowWarning(controlRect, "LDtk Asset is not assigned");
            }
        }

        protected void ThrowWarning(Rect controlRect, string message) => ThrowInternal(controlRect, message, LDtkDrawerUtil.DrawWarning);
        protected void ThrowError(Rect controlRect, string message) => ThrowInternal(controlRect, message, LDtkDrawerUtil.DrawError);

        private void ThrowInternal(Rect controlRect, string message, LDtkDrawerUtil.IconDraw draw)
        {
            Rect fieldRect = GetFieldRect(controlRect);
            Vector2 pos = new Vector2(fieldRect.xMin, fieldRect.yMin + fieldRect.height/2);
            draw.Invoke(pos, message, TextAnchor.MiddleRight);
            HasProblem = true;
        }
        
        protected override void DrawSelfSimple(Rect controlRect, Texture2D iconTex, TData data)
        {
            base.DrawSelfSimple(controlRect, iconTex, data);
            DrawFieldAndObject(controlRect, data);
        }
        

    }
}