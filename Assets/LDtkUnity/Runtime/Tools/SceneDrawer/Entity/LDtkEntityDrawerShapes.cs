using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerShapes : ILDtkGizmoDrawer
    {
        private readonly Transform _transform;
        private readonly Data _data;

        public struct Data
        {
            public RenderMode EntityMode;
            public Vector2 Size;
            public Vector2 Pivot;
            public bool Hollow;
            public float FillOpacity;
            public float LineOpacity;
        } 

        public LDtkEntityDrawerShapes(Transform transform, Data data)
        {
            _data = data;
            _transform = transform;
        }

        public void OnDrawGizmos()
        {
            float lineAlpha = _data.LineOpacity;
            float fillAlpha = _data.Hollow ? 0 : _data.FillOpacity;

            Vector2 pivot = _data.Pivot;
            Vector2 size = _data.Size;
            //pivot.y *= -1;
            
            //Vector2 reversePivot = Vector2.one - _data.Pivot;

            //horizontalOffset.x *= _data.Size.x;

            Vector2 halfUnit = Vector2.one * 0.5f;
            Vector2 properPivot = pivot - halfUnit;
            
            Vector2 pivotSize = size * properPivot;
            //Vector2 halfPivotSize = _data.Size * pivot / 2;

            //Vector2 reverseSize = _data.Size * reversePivot;

            //Vector2 height = Vector2.up * _data.Size.y;
            
            //halfSize.y *= -1;

            //Vector2 halfUnit = _data.Size / 2;
            Vector2 offset = size;
            offset.y = 0;
            float thing = pivot.x - 1;
            offset.x *= -properPivot.x * 2;

            

            Vector2 pos = (Vector2)_transform.position + pivotSize + offset;// + halfUnit;// + reverseSize - height;

            switch (_data.EntityMode)
            {
                case RenderMode.Cross:
                    GizmoAAUtil.DrawAACross(pos, size, 4); //todo fix this to support any pivot point
                    break;
                
                case RenderMode.Ellipse:
                    GizmoAAUtil.DrawAAEllipse(pos, size, fillAlpha: fillAlpha, lineAlpha: lineAlpha); //todo fix this to support any pivot point
                    break;
                
                case RenderMode.Rectangle:
                    GizmoAAUtil.DrawAABox(pos, size, fillAlpha: fillAlpha, lineAlpha: lineAlpha); //todo fix this to support any pivot point
                    break;
            }
        }

        private void DrawRectangle()
        {
            
        }
    }
}