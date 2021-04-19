using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionLevels : LDtkProjectSectionDrawer<Level>
    {
        protected override string PropertyName => "LDtkProjectImporter.LEVEL";
        protected override string GuiText => "Levels";
        protected override string GuiTooltip => "The levels. Hit the button at the bottom of this dropdown to automatically assign them.";
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
                LDtkDrawerLevel drawer = new LDtkDrawerLevel(level, levelObj, level.Identifier);
                
                drawers.Add(drawer);
            }
        }

        protected override void DrawDropdownContent(Level[] datas)
        {
            EditorGUILayout.Space();
        
            base.DrawDropdownContent(datas);

            if (Project == null)
            {
                return;
            }
            
            LDtkRelativeAssetFinderLevels finderLevels = new LDtkRelativeAssetFinderLevels();
            finderLevels.GetRelativeAssets(datas, Project.ProjectJson);
        }
        
        
        
        
        
    }
}