using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionLevelBackgrounds  : LDtkProjectSectionDrawer<Level>
    {
        protected override string PropertyName => LDtkProject.LEVEL_BACKGROUNDS;
        protected override string GuiText => "Level Backgrounds";
        protected override string GuiTooltip => "LDtk's level backgrounds.\n" +
                                                "Hit the button at the bottom of this dropdown to automatically assign them.";
        protected override Texture GuiImage => LDtkIconLoader.GetUnityIcon("RawImage");
        
        public LDtkProjectSectionLevelBackgrounds(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override bool HasSectionProblem()
        {
            return false;
        }

        protected override void GetDrawers(Level[] lvls, List<LDtkContentDrawer<Level>> drawers)
        {
            for (int i = 0; i < lvls.Length; i++)
            {
                
                
                Level lvl = lvls[i];
                SerializedProperty elementProp = ArrayProp.GetArrayElementAtIndex(i);
                LDtkDrawerLevelBackground drawer = new LDtkDrawerLevelBackground(lvl, elementProp, lvl.Identifier);
                drawers.Add(drawer);
            }
        }

        protected override void DrawDropdownContent(Level[] datas)
        {
            base.DrawDropdownContent(datas);
            
            //new LDtkRelativeAssetFinderLevelBackground().GetRelativeAssets(ArrayProp, datas, Project.ProjectJson);
        }
    }
}