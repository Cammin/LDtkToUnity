using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    public abstract class LDtkSectionDataDrawer<T> : LDtkSectionDrawer, ILDtkSectionDataDrawer where T : ILDtkIdentifier
    {
        protected SerializedProperty ArrayProp;

        private LDtkContentDrawer<T>[] _drawers;
        
        public override bool HasProblem => HasSectionProblem() || (_drawers != null && _drawers.Any(p => p != null && p.HasProblem()));


        protected LDtkSectionDataDrawer(SerializedObject serializedObject) : base(serializedObject)
        {
            
        }

        public override void Init()
        {
            base.Init();
            ArrayProp = SerializedObject.FindProperty(PropertyName);
        }
        
        public void Draw(IEnumerable<ILDtkIdentifier> datas)
        {
            DrawInternal(datas.Cast<T>().ToArray());
        }

        private void DrawInternal(T[] datas)
        {
            HasResizedArrayPropThisUpdate = false;
            int arraySize = GetSizeOfArray(datas);
            
            //don't draw if there is no data for this project relating to this
            if (arraySize == 0 && !SerializedObject.isEditingMultipleObjects)
            {
                return;
            }
            
            LDtkEditorGUIUtility.DrawDivider();
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawFoldoutArea(controlRect);
            
            //don't process any data or resize arrays when we have multi-selections; references will break because of how dynamic the arrays can be.
            if (SerializedObject.isEditingMultipleObjects && !SupportsMultipleSelection)
            {
                EditorGUILayout.HelpBox($"Multi-object editing not supported for {GuiText}.", MessageType.None);
                return;
            }
            
            if (arraySize > 0)
            {
                if (ArrayProp != null)
                {
                    if (ArrayProp.arraySize != arraySize)
                    {
                        ArrayProp.arraySize = arraySize;
                        HasResizedArrayPropThisUpdate = true;
                    }
                }
            }

            List<LDtkContentDrawer<T>> drawers = new List<LDtkContentDrawer<T>>();
            GetDrawers(datas, drawers);
            _drawers = drawers.ToArray();

            if (TryDrawDropdown(controlRect))
            {
                DrawDropdownContent(datas);
            }
        }
        

        protected abstract void GetDrawers(T[] defs, List<LDtkContentDrawer<T>> drawers);


        protected virtual int GetSizeOfArray(T[] datas)
        {
            return datas.Length;
        }

        protected virtual void DrawDropdownContent(T[] datas)
        {
            foreach (LDtkContentDrawer<T> drawer in _drawers)
            {
                drawer.Draw();
            }
        }
    }
}