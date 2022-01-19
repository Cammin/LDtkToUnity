using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry
    /// </summary>
    internal abstract class LDtkAssetDrawer<TData, TAsset> : LDtkContentDrawer<TData> where TData : ILDtkIdentifier where TAsset : Object
    {
        protected readonly SerializedProperty Root;
        protected readonly SerializedProperty Key;
        protected readonly SerializedProperty Value;

        private string _problemMessage = "";
        private LDtkEditorGUI.IconDraw _problemDrawEvent = null;

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

        protected virtual string AssetUnassignedText => "Unassigned object";
        
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
            Rect controlRect = EditorGUILayout.GetControlRect();

            DrawField(controlRect);
            
            if (HasProblem())
            {
                DrawCachedProblem(controlRect);
            }
        }
        
        public override bool HasProblem()
        {
            if (Value == null)
            {
                CacheError("Serialized property is null");
                return true;
            }
            
            if (Asset == null)
            {
                CacheWarning(AssetUnassignedText);
                return true;
            }

            return false;
        }

        protected void DrawField(Rect controlRect, float textIndent = 0, Texture tex = null)
        {
            if (Value == null)
            {
                Debug.LogError("Asset drawer's value property is null");
                return;
            }
            
            GUIContent objectContent = new GUIContent()
            {
                text = _data.Identifier,
            };

            if (tex != null)
            {
                objectContent.image = tex;
            }
            else
            {
                Texture2D image = new Texture2D(1, 1);
                image.SetPixel(0, 0, Color.clear);
            
#if UNITY_2021_2_OR_NEWER
                image.Reinitialize((int)textIndent, (int)controlRect.height);
#else
                image.Resize((int)textIndent, (int)controlRect.height);
#endif
                objectContent.image = image;
            }

            EditorGUI.PropertyField(controlRect, Value, objectContent);
        }

        protected void CacheWarning(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkEditorGUI.DrawFieldWarning;
        }

        protected void CacheError(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkEditorGUI.DrawFieldError;
        }
        
        protected void DrawCachedProblem(Rect controlRect)
        {
            if (_problemDrawEvent == null)
            {
                Debug.LogError("Tried drawing problem but the event was null");
                return;
            }
            
            _problemDrawEvent.Invoke(controlRect, _problemMessage);
        }
    }
}