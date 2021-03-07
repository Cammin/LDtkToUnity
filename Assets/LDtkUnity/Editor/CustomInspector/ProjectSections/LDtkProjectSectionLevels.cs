using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionLevels : LDtkProjectSectionDrawer<Level>
    {
        protected override string PropertyName => LDtkProject.LEVEL;
        protected override string GuiText => "Levels";
        protected override string GuiTooltip => "The levels. Hit the button at the bottom to automatically assign them.";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadWorldIcon();
        
        public LDtkProjectSectionLevels(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void DrawDropdownContent(Level[] datas)
        {
            DrawLevels(datas);

            new AutoAssetLinkerLevels().DrawButton(ArrayProp, datas, Project.ProjectJson);
        }

        private bool DrawLevels(Level[] lvls)
        {
            bool passed = true;
            for (int i = 0; i < lvls.Length; i++)
            {
                Level level = lvls[i];
                SerializedProperty levelObj = ArrayProp.GetArrayElementAtIndex(i);
                
                LDtkReferenceDrawerLevel drawer = new LDtkReferenceDrawerLevel(levelObj, level.Identifier);
                
                if (drawer.HasError(level))
                {
                    passed = false;
                }
                
                drawer.Draw(level);
            }

            return passed;
        }
    }
}