using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerShapes : ILDtkGizmoDrawer
    {
        private readonly Transform _transform;
        private readonly RenderMode _entityMode;
        private readonly Vector2 _size;

        public LDtkEntityDrawerShapes(Transform transform, RenderMode entityMode, Vector2 size)
        {
            _transform = transform;
            _entityMode = entityMode;
            _size = size;
        }

        public void OnDrawGizmos()
        {
            switch (_entityMode)
            {
                case RenderMode.Cross:
                    Debug.Log("DRAW CROSS");
                    break;
                
                case RenderMode.Ellipse:
                    GizmoUtil.DrawAAEllipse(_transform.position, _size); //todo fix this to support any pivot point
                    break;
                
                case RenderMode.Rectangle:
                    GizmoUtil.DrawAABox(_transform.position, _size); //todo fix this to support any pivot point
                    break;
            }
        }

        private void DrawRectangle()
        {
            
        }
    }
}