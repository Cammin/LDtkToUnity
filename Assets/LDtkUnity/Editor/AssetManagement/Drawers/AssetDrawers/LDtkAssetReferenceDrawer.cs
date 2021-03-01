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
        protected readonly SerializedProperty Key;
        protected readonly SerializedProperty Value;
        
        public bool HasProblem { get; private set; } = false;
        
        protected LDtkAssetReferenceDrawer(SerializedProperty obj, string key)
        {
            if (obj == null)
            {
                Debug.LogError($"Null property for {key}");
                return;
            }
            
            Value = obj.FindPropertyRelative(LDtkAsset.PROP_ASSET);

            if (Value == null)
            {
                Debug.LogError($"FindProperty Value null for {key}");
            }
            
            Key = obj.FindPropertyRelative(LDtkAsset.PROP_KEY);

            if (Key == null)
            {
                Debug.LogError($"FindProperty Key null for {key}");
                return;
            }
            
            Key.stringValue = key;
            
            if (obj.serializedObject.hasModifiedProperties)
            {
                obj.serializedObject.ApplyModifiedProperties();
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
            
            if (Value.objectReferenceValue == null)
            {
                ThrowWarning(controlRect, "LDtk Asset is not assigned");
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
        
        //may potentially keep, delete later if never used
        /*protected void DrawFieldAndObject(Rect controlRect, TData data)
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
            
            EditorGUI.PropertyField(halfLeft, Value, GUIContent.none);
            Object propertyReference = Value.objectReferenceValue;
            if (propertyReference != null)
            {
                GUI.enabled = false;
                SerializedProperty assetProp = Value.FindPropertyRelative(LDtkAsset.PROP_ASSET);
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
        }*/

        protected void ThrowWarning(Rect controlRect, string message) => ThrowInternal(controlRect, message, LDtkDrawerUtil.DrawWarning);
        protected void ThrowError(Rect controlRect, string message) => ThrowInternal(controlRect, message, LDtkDrawerUtil.DrawError);

        private void ThrowInternal(Rect controlRect, string message, LDtkDrawerUtil.IconDraw draw)
        {
            Rect fieldRect = GetFieldRect(controlRect);
            Vector2 pos = new Vector2(fieldRect.xMin, fieldRect.yMin + fieldRect.height/2);
            draw.Invoke(pos, message, TextAnchor.MiddleRight);
            HasProblem = true;
        }
        
        protected void DrawSelfSimple<T>(Rect controlRect, Texture2D iconTex, TData data) where T : Object
        {
            DrawIconAndLabel(controlRect, iconTex, data);
            DrawField<T>(controlRect);

            
        }
        

    }
}