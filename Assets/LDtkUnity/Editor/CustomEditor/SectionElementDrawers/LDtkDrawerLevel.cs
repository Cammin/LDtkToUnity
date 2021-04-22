using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerLevel : LDtkContentDrawer<Level>
    {
        private SerializedProperty element;
        public LDtkDrawerLevel(Level data, SerializedProperty obj) : base(data)
        {
            element = obj;
        }

        public override void Draw()
        {
            GUIContent content = new GUIContent()
            {
                text = _data.Identifier 
            };
            
            EditorGUILayout.PropertyField(element, content); // DrawLevelBool(element.boolValue, fieldName, levelAsset);
        }
        
        /*public bool DrawLevelBool(bool prev, string label, LDtkLevelFile file)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            float rightIndent = controlRect.height;

            Rect toggleRect = new Rect(controlRect)
            {
                x = controlRect.x - controlRect.height + rightIndent,
                width = controlRect.height
            };
            Rect fieldRect = new Rect(controlRect)
            {
                x = controlRect.x + controlRect.height,
                width = controlRect.width - rightIndent
            };

            Texture2D blankSpace = GetBlankImage();

            GUIContent content = new GUIContent
            {
                image = blankSpace,
                text = label
            };

            GUI.enabled = false;
            EditorGUI.ObjectField(controlRect, content, file, typeof(LDtkLevelFile), false);
            GUI.enabled = true;
            return EditorGUI.Toggle(toggleRect, prev);

        }*/
    }
}