using System.Collections.Generic;
using System.Linq;

namespace LDtkUnity.Editor
{
    public class LDtkSceneDrawerLevel : LDtkSceneDrawerComponents
    {
        public override void Draw()
        {
            List<LDtkComponentLevel> components = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>().Where(CanDrawItem).ToList();
            List<LDtkLevelDrawer> drawers = components.ConvertAll(p => new LDtkLevelDrawer(p));

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