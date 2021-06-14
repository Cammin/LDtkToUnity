using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [Serializable]
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerBase
    {
        protected readonly Transform Transform;
        protected readonly string Identifier;
        
        private readonly Color _gizmoColor;
        
        public LDtkSceneDrawerBase(Component fields, string identifier, Color gizmoColor)
        {
            Transform = fields.transform;
            Identifier = identifier;
            _gizmoColor = gizmoColor;
        }

        public virtual void Draw()
        {
            SetGizmoColor();
        }

        private void SetGizmoColor()
        {
            Gizmos.color = _gizmoColor;
#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
#endif
        }
        
    }
}