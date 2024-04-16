using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomPropertyDrawer(typeof(LDtkNeighbour))]
    internal sealed class LDtkNeighbourDrawer : PropertyDrawer
    {
        public SerializedProperty _propIdentifier;
        public SerializedProperty _propDir;
        public SerializedProperty _propLevelIid;
        
        private Rect _position;
        private Rect _labelRect;
        private Rect _fieldRect;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TryInitialise(property);
            TryInitTex();

            GUIContent content = new GUIContent(_propIdentifier.stringValue);
            _position = position;
            _labelRect = LDtkEditorGUIUtility.GetLabelRect(position);
            _fieldRect = LDtkEditorGUIUtility.GetFieldRect(position);

            if (content.image == null)
            {
                TryInitTex();
                content.image = _blankSquareTex;
            }

            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 25))
            {
                LDtkIidEditor.DrawIidAndGameObject(position, _labelRect, _propLevelIid, content);
                DrawNeighbour(_labelRect);
            }
        }
        
        public void TryInitialise(SerializedProperty property)
        {
            _propIdentifier = property.FindPropertyRelative(LDtkNeighbour.PROPERTY_IDENTIFIER);
            _propDir = property.FindPropertyRelative(LDtkNeighbour.PROPERTY_DIR);
            _propLevelIid = property.FindPropertyRelative(LDtkNeighbour.PROPERTY_LEVEL_IID);
        }
        
        private void DrawNeighbour(Rect labelRect)
        {
            Rect dirRect = labelRect;
            dirRect.height = EditorGUIUtility.singleLineHeight;
            dirRect.width = EditorGUIUtility.singleLineHeight + 6;
            dirRect.x -= 3;
            
            GUI.Label(dirRect, $"{_propDir.stringValue.ToUpper()}");
        }
        
        private static Texture2D _blankSquareTex; 
        private static void TryInitTex()
        {
            if (_blankSquareTex != null)
            {
                return;
            }
            _blankSquareTex = new Texture2D(1, 1);
            _blankSquareTex.SetPixel(0, 0, Color.clear);
            _blankSquareTex.Apply();
        }
    }
}