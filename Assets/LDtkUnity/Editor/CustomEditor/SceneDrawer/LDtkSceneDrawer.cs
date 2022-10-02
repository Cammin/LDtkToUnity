using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

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
            Profiler.BeginSample("WorldDepthGUI");
            WorldDepthGUI.Draw();
            Profiler.EndSample();
        }
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGizmos(Transform objectTransform, GizmoType gizmoType)
        {
            Profiler.BeginSample("LDtkSceneGizmos");
            Process(objectTransform);
            Profiler.EndSample();
        }

        private static void Process(Transform objectTransform)
        {
            if (SceneVisibilityManager.instance.IsPickingDisabled(objectTransform.gameObject))
            {
                return;
            }
            
            if (objectTransform.TryGetComponent<LDtkComponentLevel>(out var lvl))
            {
                Profiler.BeginSample("LDtkSceneDrawerLevel");
                new LDtkLevelDrawer(lvl).OnDrawHandles();
                Profiler.EndSample();
                return;
            }

            if (objectTransform.TryGetComponent<LDtkEntityDrawerComponent>(out var entity))
            {
                Profiler.BeginSample("LDtkSceneDrawerEntity");
                ProcessData(entity.EntityDrawer, LDtkSceneDrawerEntity.DrawEntity);
                Profiler.EndSample();
                
                Profiler.BeginSample("LDtkSceneDrawerField");
                foreach (LDtkFieldDrawerData data in entity.FieldDrawers)
                {
                    ProcessData(data, LDtkSceneDrawerField.DrawField);
                }
                Profiler.EndSample();

                return;
            }
        }

        private delegate ILDtkHandleDrawer DrawerGetter<in T>(T data) where T : LDtkSceneDrawerBase;
        private static void ProcessData<T>(T data, DrawerGetter<T> getter) where T : LDtkSceneDrawerBase
        {
            if (!data.Enabled)
            {
                return;
            }
            
            Handles.color = data.SmartColor;
            ILDtkHandleDrawer drawer = getter.Invoke(data);
            drawer?.OnDrawHandles();
        }
    }
}