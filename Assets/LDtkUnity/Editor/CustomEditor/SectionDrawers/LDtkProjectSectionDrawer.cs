using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    public abstract class LDtkSectionDrawer<T> : ILDtkSectionDrawer where T : ILDtkIdentifier
    {
        protected abstract string PropertyName { get; }
        protected abstract string GuiText { get; }
        protected abstract string GuiTooltip { get; }
        protected abstract Texture GuiImage { get; }
        
        protected readonly SerializedObject SerializedObject;
        protected SerializedProperty ArrayProp;

        
        private bool _dropdown;

        protected LDtkContentDrawer<T>[] Drawers;
        
        protected LDtkProjectImporter Project => (LDtkProjectImporter)SerializedObject?.targetObject;
        
        public bool HasProblem => HasSectionProblem() || (Drawers != null && Drawers.Any(p => p != null && p.HasProblem()));
        public bool HasResizedArrayPropThisUpdate { get; private set; } = false;


        protected LDtkSectionDrawer(SerializedObject serializedObject)
        {
            SerializedObject = serializedObject;
        }

        public void Init()
        {
            string key = PropertyName;
            ArrayProp = SerializedObject.FindProperty(key);
            _dropdown = EditorPrefs.GetBool(key, true);
        }

        

        public void Dispose()
        {
            EditorPrefs.SetBool(PropertyName, _dropdown);
        }

        
        public void Draw(IEnumerable<ILDtkIdentifier> datas)
        {
            DrawInternal(datas.Cast<T>().ToArray());
        }
        public void DrawInternal(T[] datas)
        {
            HasResizedArrayPropThisUpdate = false;
            int arraySize = GetSizeOfArray(datas);
            
            if (arraySize == 0)
            {
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
            
            LDtkEditorGUIUtility.DrawDivider();
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawFoldoutArea(controlRect);

            List<LDtkContentDrawer<T>> drawers = new List<LDtkContentDrawer<T>>();
            GetDrawers(datas, drawers);
            Drawers = drawers.ToArray();
            
            if (_dropdown)
            {
                DrawDropdownContent(datas);
            }
            else if (HasProblem)
            {
                DrawSectionProblem(controlRect);
            }
        }

        private static void DrawSectionProblem(Rect controlRect)
        {
            float dimension = controlRect.height - 2;
            Rect errorArea = new Rect(controlRect)
            {
                x = controlRect.x + EditorGUIUtility.labelWidth - dimension + 1,
                y = controlRect.y +1,
                width = dimension,
                height = dimension
            };

            GUIContent tooltipContent = new GUIContent()
            {
                tooltip = "Expand this section to view the error"
            };
            
            GUI.Label(errorArea, tooltipContent);
            GUI.DrawTexture(errorArea, EditorGUIUtility.IconContent("console.warnicon.sml").image);
        }

        protected abstract void GetDrawers(T[] defs, List<LDtkContentDrawer<T>> drawers);


        protected virtual int GetSizeOfArray(T[] datas)
        {
            return datas.Length;
        }

        protected virtual bool HasSectionProblem()
        {
            return false;
        }
        
        private void DrawFoldoutArea(Rect controlRect)
        {
            GUIContent content = new GUIContent()
            {
                text = GuiText,
                tooltip = GuiTooltip,
                image = GuiImage
            };
            
            _dropdown = EditorGUI.Foldout(controlRect, _dropdown, content);
        }

        protected virtual void DrawDropdownContent(T[] datas)
        {
            foreach (LDtkContentDrawer<T> drawer in Drawers)
            {
                drawer.Draw();
            }
        }
    }
}