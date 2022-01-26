using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkGroupDrawer<TDef, TData, TDrawer> : LDtkContentDrawer<TData>
        where TDef : ILDtkIdentifier
        where TData : ILDtkIdentifier
        where TDrawer : LDtkContentDrawer<TDef> 
    {
        protected List<TDrawer> Drawers;
        protected readonly SerializedProperty ArrayProp;

        protected LDtkGroupDrawer(TData data, SerializedProperty serializedProperty) : base(data)
        {
            ArrayProp = serializedProperty;
        }

        protected abstract List<TDrawer> GetDrawers();

        public override void Draw()
        {
            DrawGroupLabel(_data.Identifier);
            DrawItems();
        }

        protected void DrawItems()
        {
            foreach (TDrawer valueDrawer in Drawers)
            {
                valueDrawer.Draw();
            }
        }
        
        protected void DrawGroupLabel(string label)
        {
            GUILayout.Space(3);
            Rect controlRect = EditorGUILayout.GetControlRect(GUILayout.Height(11));
            EditorGUI.LabelField(controlRect, label, EditorStyles.miniLabel);
        }
    }
}