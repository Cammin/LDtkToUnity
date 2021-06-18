using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerBase
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private Color _gizmoColor;

        public bool Enabled => _enabled;
        public Color GizmoColor => _gizmoColor;
        
        protected LDtkSceneDrawerBase(Color gizmoColor)
        {
            _gizmoColor = gizmoColor;
            AdjustGizmoColor();
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