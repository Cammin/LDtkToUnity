using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    public abstract class LDtkSectionDrawer : ILDtkSectionDrawer
    {
        protected readonly SerializedObject SerializedObject;
        private bool _dropdown;

        protected abstract string PropertyName { get; }
        protected abstract string GuiText { get; }
        protected abstract string GuiTooltip { get; }
        protected abstract Texture GuiImage { get; }
        protected abstract string ReferenceLink { get; }




        public bool HasResizedArrayPropThisUpdate { get; protected set; } = false;

        protected LDtkProjectImporter Importer => (LDtkProjectImporter)SerializedObject?.targetObject;
        public virtual bool HasProblem => false;
        protected virtual bool SupportsMultipleSelection => false; 
        


        protected LDtkSectionDrawer(SerializedObject serializedObject)
        {
            SerializedObject = serializedObject;
        }

        public virtual void Init()
        {
            _dropdown = EditorPrefs.GetBool(PropertyName, true);
        }
        
        public void Dispose()
        {
            EditorPrefs.SetBool(PropertyName, _dropdown);
        }
        
        public void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            DrawFoldoutArea(controlRect);

            if (TryDrawDropdown(controlRect))
            {
                DrawDropdownContent();
            }
        }
        protected bool TryDrawDropdown(Rect controlRect)
        {
            if (_dropdown)
            {
                return true;
            }

            if (HasProblem)
            {
                DrawSectionProblem(controlRect);
            }

            return false;
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

        protected void DrawFoldoutArea(Rect controlRect)
        {
            GUIContent content = new GUIContent()
            {
                text = GuiText,
                tooltip = GuiTooltip,
                image = GuiImage
            };

#if UNITY_2019_3_OR_NEWER
            GUIStyle style = EditorStyles.foldoutHeader;
#else
            GUIStyle style = EditorStyles.foldout;
#endif
            
            _dropdown = EditorGUI.Foldout(controlRect, _dropdown, content, style);
            
            DrawHelpIcon(controlRect);
        }

        private void DrawHelpIcon(Rect controlRect)
        {
            if (string.IsNullOrEmpty(ReferenceLink))
            {
                return;
            }
            
            //draw the help symbol
            Texture tex = LDtkIconUtility.GetUnityIcon("_Help", "");
            GUIContent content = new GUIContent()
            {
                tooltip = $"Open Reference for {GuiText}.",
                image = tex
            };
            
            const int indent = 2;
            Rect helpRect = new Rect(controlRect)
            {
                x = controlRect.xMax - tex.width - indent,
                width = tex.width, 
                height = tex.height
            };
            if (GUI.Button(helpRect, content, GUIStyle.none))
            {
                Application.OpenURL(ReferenceLink);
            }
            
        }

        protected virtual bool HasSectionProblem()
        {
            return false;
        }

        protected virtual void DrawDropdownContent()
        {
            
        }
    }
}