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
            LDtkProfiler.BeginSample("LDtkSceneGizmos");
            DrawSceneGizmos(objectTransform);
            LDtkProfiler.EndSample();
        }

        private static void DrawSceneGizmos(Transform objectTransform)
        {
            if (objectTransform.TryGetComponent<LDtkComponentLevel>(out var lvl))
            {
                LDtkProfiler.BeginSample("LDtkSceneDrawerLevel");
                new LDtkLevelDrawer(lvl).OnDrawHandles();
                LDtkProfiler.EndSample();
                return;
            }

            if (objectTransform.TryGetComponent<LDtkEntityDrawerComponent>(out var entity))
            {
                LDtkProfiler.BeginSample("LDtkSceneDrawerEntity");
                ProcessData(entity.EntityDrawer, LDtkSceneDrawerEntity.DrawEntity);
                LDtkProfiler.EndSample();
                
                LDtkProfiler.BeginSample("LDtkSceneDrawerField");
                foreach (LDtkFieldDrawerData data in entity.FieldDrawers)
                {
                    ProcessData(data, LDtkSceneDrawerField.DrawField);
                }
                LDtkProfiler.EndSample();

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