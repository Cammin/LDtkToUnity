using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkDrawerEntity : LDtkAssetDrawer<EntityDefinition, GameObject>
    {
        private readonly LDtkEntityIconFactory _iconFactory;

        private readonly GUIContent _objectContent;
        
        public LDtkDrawerEntity(EntityDefinition def, SerializedProperty prop) : base(def, prop)
        {
            Object targetObject = prop.serializedObject.targetObject;
            LDtkProjectImporter importer = (LDtkProjectImporter)targetObject;
            if (importer == null)
            {
                LDtkDebug.LogError($"importer was null, the type was {targetObject.name}");
            }

            _iconFactory = new LDtkEntityIconFactory(def, importer);
            _objectContent = new GUIContent
            {
                text = _data.Identifier,
                image = GetIcon(),
                tooltip = _data.Doc
            };
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
    }
}