using UnityEditor;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.Overlays;
#else
using UnityEngine.Profiling;
#endif

namespace LDtkUnity.Editor
{
#if UNITY_2021_2_OR_NEWER
    [Overlay(typeof(SceneView), nameof(LDtkWorldDepthGUIDrawer), "LDtk World Depth", true)]
    internal sealed class LDtkWorldDepthGUIDrawer : IMGUIOverlay
    {
        private readonly LDtkWorldDepthGUI _worldDepthGUI = new LDtkWorldDepthGUI();
        
        public override void OnGUI()
        {
            if (_worldDepthGUI.CanDraw())
            {
                _worldDepthGUI.DrawWindow();
                return;
            }
        }
    }
#else
    [InitializeOnLoad]
    internal sealed class LDtkWorldDepthGUIDrawer
    {
        private static readonly LDtkWorldDepthGUI WorldDepthGUI = new LDtkWorldDepthGUI();
        static LDtkWorldDepthGUIDrawer()
        {
            SceneView.duringSceneGui += CustomOnSceneGUI;
        }
        private static void CustomOnSceneGUI(SceneView view)
        {
            Profiler.BeginSample("WorldDepthGUI");
            WorldDepthGUI.Draw();
            Profiler.EndSample();
        }
    }
#endif
}
