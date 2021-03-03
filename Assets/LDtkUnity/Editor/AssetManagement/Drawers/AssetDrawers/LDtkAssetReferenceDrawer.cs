using System;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// For the assignable assets 
    /// </summary>
    public abstract class LDtkAssetReferenceDrawer<TData> : LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier
    {
        protected readonly SerializedProperty Key;
        protected readonly SerializedProperty Value;

        private string _problemMessage = "";
        private LDtkDrawerUtil.IconDraw _problemDrawEvent = null;

        public virtual bool HasError(TData data)
        {
            if (Value == null)
            {
                return true;
            }
            
            if (Value.objectReferenceValue == null)
            {
                return true;
            }

            return false;
        }
        
        protected LDtkAssetReferenceDrawer(SerializedProperty prop, string key)
        {
            if (prop == null)
            {
                Debug.LogError($"Null property for {key}");
                return;
            }
            
            Value = prop.FindPropertyRelative(LDtkAsset.PROP_ASSET);

            if (Value == null)
            {
                Debug.LogError($"FindProperty Value null for {key}");
            }
            
            Key = prop.FindPropertyRelative(LDtkAsset.PROP_KEY);

            if (Key == null)
            {
                Debug.LogError($"FindProperty Key null for {key}");
                return;
            }
            
            Key.stringValue = key;
            
            if (prop.serializedObject.hasModifiedProperties)
            {
                prop.serializedObject.ApplyModifiedProperties();
            }
            
        }
        
        protected void DrawField<T>(Rect controlRect) where T : Object
        {
            if (Value == null)
            {
                Debug.LogError("Asset Reference's Value property is null");
                return;
            }
            
            float labelWidth = LDtkDrawerUtil.LabelWidth(controlRect.width);
            float fieldWidth = controlRect.width - labelWidth;
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + labelWidth,
                width = Mathf.Max(fieldWidth, EditorGUIUtility.fieldWidth)
            };

            Value.objectReferenceValue = EditorGUI.ObjectField(fieldRect, Value.objectReferenceValue, typeof(T), false);

            if (Value.serializedObject.hasModifiedProperties)
            {
                Value.serializedObject.ApplyModifiedProperties();
            }
            

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

        protected void ThrowWarning(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkDrawerUtil.DrawWarning;
        }

        protected void ThrowError(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkDrawerUtil.DrawError;
        }
        
        private void DrawProblem(Rect controlRect)
        {
            Rect fieldRect = GetFieldRect(controlRect);
            Vector2 pos = new Vector2(fieldRect.xMin, fieldRect.yMin + fieldRect.height/2);
            _problemDrawEvent.Invoke(pos, _problemMessage, TextAnchor.MiddleRight);
        }
        
        protected void DrawSelfSimple<T>(Rect controlRect, TData data) where T : Object
        {
            DrawLabel(controlRect, data.Identifier);
            DrawField<T>(controlRect);

            if (HasError(data))
            {
                DrawProblem(controlRect);
            }
        }
        

    }
}