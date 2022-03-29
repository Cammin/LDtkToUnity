using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    internal static class LDtkSceneDrawerLevel
    {
        public static void Draw(List<LDtkComponentLevel> levelComponents)
        {
            
            List<LDtkLevelDrawer> drawers = levelComponents.ConvertAll(p => new LDtkLevelDrawer(p));

            //borders, then labels, so that borders are never in front of labels
            foreach (LDtkLevelDrawer drawer in drawers)
            {
                drawer.OnDrawHandles();
            }
            foreach (LDtkLevelDrawer drawer in drawers)
            {
                drawer.DrawLabel();
            }
        }
    }
}