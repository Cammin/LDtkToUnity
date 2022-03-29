using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //meant for serving the purpose of gaving a tag + drawers.
    internal abstract class LDtkGroupDrawer<TDef, TData, TDrawer> : LDtkContentDrawer<TData>
        where TDef : ILDtkIdentifier
        where TData : ILDtkIdentifier
        where TDrawer : LDtkContentDrawer<TDef> 
    {
        protected string Tag;
        protected readonly SerializedProperty ArrayProp;
        
        private List<TDrawer> _drawers;

        protected LDtkGroupDrawer(TData data, SerializedProperty serializedProperty) : base(data)
        {
            ArrayProp = serializedProperty;

            if (data != null)
            {
                Tag = _data.Identifier;
            }
        }

        public void InitDrawers()
        {
            _drawers = GetDrawersForGroup();
        }
        protected abstract List<TDrawer> GetDrawersForGroup();

        public override void Draw()
        {
            DrawGroupLabel(Tag);
            DrawItems();
        }

        protected void DrawItems()
        {
            foreach (TDrawer valueDrawer in _drawers)
            {
                valueDrawer?.Draw();
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