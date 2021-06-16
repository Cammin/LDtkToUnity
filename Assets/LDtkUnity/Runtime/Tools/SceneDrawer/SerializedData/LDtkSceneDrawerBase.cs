using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerBase : ILDtkGizmoDrawer
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Color _gizmoColor;

        protected LDtkSceneDrawerBase(Color gizmoColor)
        {
            _gizmoColor = gizmoColor;
            AdjustGizmoColor();
        }

        public void OnDrawGizmos()
        {
            if (!_enabled)
            {
                return;
            }
            
            //AdjustGizmoColor(_gizmoColor);
            SetGizmoColor();

            ILDtkGizmoDrawer drawer = GetDrawer();
            drawer?.OnDrawGizmos();
        }

        protected abstract ILDtkGizmoDrawer GetDrawer();

        private void SetGizmoColor()
        {
            Gizmos.color = _gizmoColor;
#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
#endif
        }
        
        private void AdjustGizmoColor()
        {
            _gizmoColor.a = 0.66f;
            const float incrementDifference = -0.1f;
            _gizmoColor.r += incrementDifference;
            _gizmoColor.g += incrementDifference;
            _gizmoColor.b += incrementDifference;
        }
        
    }
}