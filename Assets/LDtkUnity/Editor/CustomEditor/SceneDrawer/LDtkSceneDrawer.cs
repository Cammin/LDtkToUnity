using UnityEditor;

namespace LDtkUnity.Editor
{
    [InitializeOnLoad]
    internal class LDtkSceneDrawer
    {
        private static readonly LDtkSceneDrawerWorldDepthGUI WorldDepthGUI = new LDtkSceneDrawerWorldDepthGUI();
        private static readonly LDtkSceneDrawerLevel Levels = new LDtkSceneDrawerLevel();
        private static readonly LDtkSceneDrawerEntity Entities = new LDtkSceneDrawerEntity();
        
        static LDtkSceneDrawer()
        {
            SceneView.duringSceneGui += CustomOnSceneGUI;
        }
        
        private static void CustomOnSceneGUI(SceneView view)
        {
            Entities.Draw();
            Levels.Draw();
            WorldDepthGUI.Draw();
        }
    }
}