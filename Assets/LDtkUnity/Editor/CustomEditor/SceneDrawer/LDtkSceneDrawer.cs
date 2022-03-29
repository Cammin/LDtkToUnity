using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [InitializeOnLoad]
    internal class LDtkSceneDrawer
    {
        private static readonly LDtkSceneDrawerWorldDepthGUI WorldDepthGUI = new LDtkSceneDrawerWorldDepthGUI();

        static LDtkSceneDrawer()
        {
            SceneView.duringSceneGui += CustomOnSceneGUI;
        }
        
        private static void CustomOnSceneGUI(SceneView view)
        {
            List<LDtkEntityDrawerComponent> entityComponents = LDtkFindInScenes.FindInAllScenes<LDtkEntityDrawerComponent>().Where(CanDrawItem).ToList();
            LDtkSceneDrawerEntity.Draw(entityComponents);
            LDtkSceneDrawerField.Draw(entityComponents);
            
            List<LDtkComponentLevel> levelComponents = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>().Where(CanDrawItem).ToList();
            LDtkSceneDrawerLevel.Draw(levelComponents);
            
            WorldDepthGUI.Draw();
        }

        private static bool CanDrawItem(Component component)
        {
            if (component == null)
            {
                return false;
            }

            if (!component.gameObject.activeInHierarchy)
            {
                return false;
            }

            SceneVisibilityManager vis = SceneVisibilityManager.instance;
            if (vis.IsPickingDisabled(component.transform.gameObject))
            {
                return false;
            }
            
            return true;
        }
    }
}