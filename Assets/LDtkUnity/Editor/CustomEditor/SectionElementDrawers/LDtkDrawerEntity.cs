using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        private readonly LDtkEntityIconFactory _iconFactory;

        private readonly GUIContent _objectContent;
        
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty prop) : base(def, prop)
        {
            Object targetObject = prop.serializedObject.targetObject;
            LDtkProjectImporter importer = (LDtkProjectImporter)targetObject;
            if (importer == null)
            {
                Debug.LogError($"importer was null, the type was {targetObject.name}");
            }

            _iconFactory = new LDtkEntityIconFactory(def, importer);
            _objectContent = ObjectContent();
        }

        public override void Draw()
        {
            //GUILayout.BeginHorizontal();
            //GUILayout.Box(copyTexture, GUILayout.Width(22), GUILayout.Height(EditorGUIUtility.singleLineHeight + 4));
            //GUILayout.EndHorizontal();
            
            
            EditorGUILayout.PropertyField(Value, _objectContent);
            
            
            /*Texture2D copyTexture = GetIcon();

            Rect controlRect = EditorGUILayout.GetControlRect(false, 48);
            controlRect.width = controlRect.height;
            GUI.DrawTexture(controlRect, copyTexture);*/
            
            
            LDtkEditorGUIUtility.DenyPotentialResursiveGameObjects(Value);
        }

        private Texture2D GetIcon()
        {
            Profiler.BeginSample("GetEntityIcon");
            Texture2D copyTexture = _iconFactory.GetIcon();
            Profiler.EndSample();
            return copyTexture;
        }

        private GUIContent ObjectContent()
        {
            GUIContent content = new GUIContent
            {
                text = _data.Identifier,
                image = GetIcon(),
                tooltip = string.Empty
            };

            if (_data.FieldDefs.IsNullOrEmpty())
            {
                return content;
            }
            
            IEnumerable<string> identifiers = _data.FieldDefs.Select(p => p.Identifier);
            content.tooltip = $"Fields:\n{string.Join(", ", identifiers)}";
            return content;
        }
    }
}