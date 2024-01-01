using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkReferenceToAnEntityInstance))]
    internal sealed class LDtkReferenceToAnEntityInstanceDrawer : PropertyDrawer
    {
        private SerializedProperty _propEntity;
        private SerializedProperty _propLayer;
        private SerializedProperty _propLevel;
        private SerializedProperty _propWorld;

        private Rect _position;
        private Rect _labelRect;

        private static readonly GUIContent GUIEntity = new GUIContent("Entity", LDtkIconUtility.LoadEntityIcon());
        private static readonly GUIContent GUILayer = new GUIContent("Layer", LDtkIconUtility.LoadLayerIcon());
        private static readonly GUIContent GUILevel = new GUIContent("Level", LDtkIconUtility.LoadLevelIcon());
        private static readonly GUIContent GUIWorld = new GUIContent("World", LDtkIconUtility.LoadWorldIcon());

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 5f;
        }

        public void CacheProps(SerializedProperty property)
        {
            _propEntity = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_ENTITY);
            _propLayer = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_LAYER);
            _propLevel = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_LEVEL);
            _propWorld = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_WORLD);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _position = position;
            _labelRect = LDtkEditorGUIUtility.GetLabelRect(_position);
            
            if (property.serializedObject.targetObject is LDtkTableOfContents)
            {
                _position = IndentedRect(_position, property);
                _labelRect = IndentedRect(_labelRect, property);
            }
            
            CacheProps(property);
            
            GUI.Label(_position, GUIContent.none, EditorStyles.helpBox);
            
            Rect tempRect = _position;
            tempRect.height = EditorGUIUtility.singleLineHeight;
            
            GUI.Label(tempRect, label, EditorStyles.label);

            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 18))
            {
                tempRect.y += EditorGUIUtility.singleLineHeight;
                LDtkIidEditor.DrawIidAndGameObject(tempRect, _labelRect, _propEntity, GUIEntity);
            
                tempRect.y += EditorGUIUtility.singleLineHeight;
                LDtkIidEditor.DrawIidAndGameObject(tempRect, _labelRect, _propLayer, GUILayer);

                tempRect.y += EditorGUIUtility.singleLineHeight;
                LDtkIidEditor.DrawIidAndGameObject(tempRect, _labelRect, _propLevel, GUILevel);
            
                tempRect.y += EditorGUIUtility.singleLineHeight;
                LDtkIidEditor.DrawIidAndGameObject(tempRect, _labelRect, _propWorld, GUIWorld);
            }
        }
        
        public static Rect IndentedRect(Rect source, SerializedProperty prop)
        {
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = prop.depth;
            source = EditorGUI.IndentedRect(source);
            EditorGUI.indentLevel = oldIndent;

            return source;
        }
    }
}