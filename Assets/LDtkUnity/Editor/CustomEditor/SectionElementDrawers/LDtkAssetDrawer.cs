using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry, for an asset.
    /// </summary>
    internal abstract class LDtkAssetDrawer<TData, TAsset> : LDtkContentDrawer<TData> where TData : ILDtkIdentifier where TAsset : Object
    {
        protected readonly SerializedProperty Root;
        protected readonly SerializedProperty Key;
        protected readonly SerializedProperty Value;

        public TAsset Asset
        {
            get
            {
                if (Value.objectReferenceValue is TAsset asset)
                {
                    return asset;
                }
                
                //in case the type changed in development
                Value.objectReferenceValue = null;
                Value.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                return (TAsset) Value.objectReferenceValue;
            }
        }

        protected LDtkAssetDrawer(TData data, SerializedProperty prop, string key) : base(data)
        {
            if (prop == null)
            {
                Debug.LogError($"Null property for {key}");
                return;
            }
            
            Root = prop;
            Value = prop.FindPropertyRelative(LDtkAsset<Object>.PROPERTY_ASSET);

            if (Value == null)
            {
                Debug.LogError($"FindProperty Value null for {key}");
            }
            
            Key = prop.FindPropertyRelative(LDtkAsset<Object>.PROPERTY_KEY);

            if (Key == null)
            {
                Debug.LogError($"FindProperty Key null for {key}");
                return;
            }
            
            Key.stringValue = key;
        }
        
        public override void Draw()
        {
            DrawField();
        }

        protected void DrawField(Texture tex = null)
        {
            if (Value == null)
            {
                Debug.LogError("Asset drawer's value property is null");
                return;
            }
            
            GUIContent guiContent = new GUIContent()
            {
                text = _data.Identifier,
                image = tex
            };

            EditorGUILayout.PropertyField(Value, guiContent);
        }

        protected void DrawField(Color iconColor)
        {
            if (Value == null)
            {
                Debug.LogError("Asset drawer's value property is null");
                return;
            }
            
            Texture2D icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, iconColor);
            icon.Apply();

            //use a space because otherwise the icon is gone too
            string identifier = _data.Identifier.IsNullOrEmpty() ? " " : _data.Identifier;
            
            GUIContent guiContent = new GUIContent()
            {
                text = identifier,
                image = icon
            };
            
            EditorGUILayout.PropertyField(Value, guiContent);
        }
    }
}