using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal static class LDtkSceneDrawer
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGizmos(Transform objectTransform, GizmoType gizmoType)
        {
            Profiler.BeginSample("LDtkSceneGizmos");
            DrawSceneGizmos(objectTransform);
            Profiler.EndSample();
        }

        private static void DrawSceneGizmos(Transform objectTransform)
        {
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