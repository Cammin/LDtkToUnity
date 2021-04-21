using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionLevels : LDtkProjectSectionDrawer<Level>
    {
        protected override string PropertyName => LDtkProjectImporter.LEVELS_TO_BUILD;
        protected override string GuiText => "Levels";
        protected override string GuiTooltip => "The levels. Select the ones you'd prefer to build in case you'd like to ignore some.";
        protected override Texture GuiImage => LDtkIconLoader.LoadWorldIcon();
        
        public LDtkProjectSectionLevels(SerializedObject serializedObject) : base(serializedObject)
        {

        }

        protected override void GetDrawers(Level[] defs, List<LDtkContentDrawer<Level>> drawers)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                Level level = defs[i];
                SerializedProperty levelObj = ArrayProp.GetArrayElementAtIndex(i);
                LDtkDrawerLevel drawer = new LDtkDrawerLevel(level, levelObj);
                
                drawers.Add(drawer);
            }
        }

        protected override void DrawDropdownContent(Level[] datas)
        {
            if (datas.Length > 1)
            {
                DrawSelectButtons(ArrayProp);
            }
            
            base.DrawDropdownContent(datas);

            if (Project == null)
            {
                return;
            }
            
            /*LDtkRelativeAssetFinderLevels finderLevels = new LDtkRelativeAssetFinderLevels();
            finderLevels.GetRelativeAssets(datas, Project.JsonFile);*/
        }
        
        private void DrawSelectButtons(SerializedProperty levelBoolsProp)
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            float width = 80;

            Rect leftButtonRect = new Rect(controlRect)
            {
                width = width
            };
            Rect rightButtonRect = new Rect(leftButtonRect)
            {
                x = controlRect.x + width
            };
            
            DrawButton(leftButtonRect, true, "Select all", levelBoolsProp);
            DrawButton(rightButtonRect, false, "Deselect all", levelBoolsProp);
        }
        
        private void DrawButton(Rect rect, bool on, string label, SerializedProperty levelBoolsProp)
        {
            if (GUI.Button(rect, label, EditorStyles.miniButton))
            {
                SelectAll(on, levelBoolsProp);
            }
        }
        
        private void SelectAll(bool on, SerializedProperty levelBoolsProp)
        {
            for (int i = 0; i < levelBoolsProp.arraySize; i++)
            {
                SerializedProperty element = levelBoolsProp.GetArrayElementAtIndex(i);
                element.boolValue = on;
            }
        }
        
        
        
        
        
    }
}