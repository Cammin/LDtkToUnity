using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the numerous content specifically. Each of these drawers consolidates a single entry
    /// </summary>
    public abstract class LDtkAssetDrawer<TData, TAsset> : LDtkContentDrawer<TData> where TData : ILDtkIdentifier where TAsset : Object
    {
        protected readonly SerializedProperty Root;
        protected readonly SerializedProperty Key;
        protected readonly SerializedProperty Value;

        private string _problemMessage = "";
        private LDtkDrawerUtil.IconDraw _problemDrawEvent = null;

        public TAsset Asset => (TAsset)Value.objectReferenceValue;

        protected virtual string AssetUnassignedText => "Unassigned object";
        
        protected LDtkAssetDrawer(TData data, SerializedProperty prop, string key) : base(data)
        {
            if (prop == null)
            {
                Debug.LogError($"Null property for {key}");
                return;
            }
            
            Root = prop;
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
            
            Texture2D image = new Texture2D(1, 1);
            image.SetPixel(0, 0, Color.clear);
            image.Resize((int)textIndent, (int)controlRect.height);
            
            GUIContent objectContent = new GUIContent()
            {
                text = _data.Identifier,
                image = tex != null ? tex : image
            };
            
            Value.objectReferenceValue = EditorGUI.ObjectField(controlRect, objectContent, Value.objectReferenceValue, typeof(TAsset), false);
        }

        protected void CacheWarning(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkDrawerUtil.DrawFieldWarning;
        }

        protected void CacheError(string message)
        {
            _problemMessage = message;
            _problemDrawEvent = LDtkDrawerUtil.DrawFieldError;
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