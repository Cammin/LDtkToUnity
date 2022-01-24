using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Reminder: Responsibility is just for drawing the Header content and and other unique functionality. All of the numerous content is handled in the Reference Drawers
    /// </summary>
    internal abstract class LDtkSectionDrawer : ILDtkSectionDrawer
    {
        protected readonly SerializedObject SerializedObject;
        private bool _dropdown;
        protected Rect _headerArea;

        
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
            _dropdown = EditorPrefs.GetBool(GetType().Name, true);
        }
        
        public virtual void Dispose()
        {
            EditorPrefs.SetBool(GetType().Name, _dropdown);
        }
        
        public void Draw()
        {
            _headerArea = EditorGUILayout.GetControlRect();
            DrawFoldoutArea(_headerArea);

            if (TryDrawDropdown(_headerArea))
            {
                DrawDropdownContent();
            }
        }
        protected bool TryDrawDropdown(Rect controlRect)
        {
            return _dropdown;
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
            DrawFoldout(controlRect);
            if (!string.IsNullOrEmpty(ReferenceLink))
            {
                LDtkEditorGUI.DrawHelpIcon(controlRect, ReferenceLink, GuiText);
            }
        }

        private void DrawFoldout(Rect controlRect)
        {
            GUIContent content = new GUIContent()
            {
                text = GuiText,
                tooltip = GuiTooltip,
                image = GuiImage
            };

            Rect foldoutRect = controlRect;
            foldoutRect.xMax -= 20;

            GUIStyle style = EditorStyles.foldoutHeader;
            _dropdown = EditorGUI.Foldout(foldoutRect, _dropdown, content, style);
        }

        protected virtual void DrawDropdownContent()
        {
            
        }

        public static void DenyPotentialResursiveGameObjects(SerializedProperty prop) //todo consider moving this to separate responsibility
        {
            GameObject levelPrefab = (GameObject) prop.objectReferenceValue;
            if (ReferenceEquals(levelPrefab, null))
            {
                return;
            }
            
            if (!levelPrefab.GetComponent<LDtkComponentProject>() && 
                !levelPrefab.GetComponent<LDtkComponentLevel>())
            {
                return;
            }

            Debug.LogWarning("LDtk: Not allowed to assign an imported LDtk GameObject. It would have resulted in a recursive crash.");
            prop.objectReferenceValue = null;
        }
    }
}