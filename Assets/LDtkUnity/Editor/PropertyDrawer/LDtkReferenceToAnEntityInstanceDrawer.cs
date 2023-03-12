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

        public void TryInitialise(SerializedProperty property)
        {
            _propEntity = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_ENTITY);
            _propLayer = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_LAYER);
            _propLevel = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_LEVEL);
            _propWorld = property.FindPropertyRelative(LDtkReferenceToAnEntityInstance.PROPERTY_WORLD);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _position = position;
            _labelRect = LDtkEditorGUIUtility.GetLabelRect(position);
            TryInitialise(property);
            
            Rect rect = _position;
            rect.height = EditorGUIUtility.singleLineHeight;
            
            GUI.Label(_position, GUIContent.none, EditorStyles.helpBox);
            GUI.Label(rect, label, EditorStyles.label);
            
            rect.y += EditorGUIUtility.singleLineHeight;
            LDtkIidEditor.DrawIidAndGameObject(rect, _labelRect, _propEntity, GUIEntity);
            
            rect.y += EditorGUIUtility.singleLineHeight;
            LDtkIidEditor.DrawIidAndGameObject(rect, _labelRect, _propLayer, GUILayer);

            rect.y += EditorGUIUtility.singleLineHeight;
            LDtkIidEditor.DrawIidAndGameObject(rect, _labelRect, _propLevel, GUILevel);
            
            rect.y += EditorGUIUtility.singleLineHeight;
            LDtkIidEditor.DrawIidAndGameObject(rect, _labelRect, _propWorld, GUIWorld);
        }
    }
}