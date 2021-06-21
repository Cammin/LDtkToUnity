using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkEntityDrawerShapes : ILDtkHandleDrawer
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

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowEntityShape)
            {
                return;
            }

            if (!_data.Hollow && LDtkPrefs.EntityOnlyHollow)
            {
                return;
            }
            
            float lineAlpha = _data.LineOpacity;
            float fillAlpha = _data.Hollow ? 0 : _data.FillOpacity;
            
            Vector2 size = _data.Size;
            
            Vector2 pos = (Vector2)_transform.position + LDtkCoordConverter.EntityPivotOffset(_data.Pivot, _data.Size);

            DrawShape(pos, size, fillAlpha, lineAlpha);
        }

        private void DrawShape(Vector2 pos, Vector2 size, float fillAlpha, float lineAlpha)
        {
            switch (_data.EntityMode)
            {
                case RenderMode.Cross:
                    HandleAAUtil.DrawAACross(pos, size, LDtkPrefs.EntityShapeThickness);
                    break;

                case RenderMode.Ellipse:
                    HandleAAUtil.DrawAAEllipse(pos, size, LDtkPrefs.EntityShapeThickness, fillAlpha, lineAlpha);
                    break;

                case RenderMode.Rectangle:
                    HandleAAUtil.DrawAABox(pos, size, LDtkPrefs.EntityShapeThickness, fillAlpha, lineAlpha);
                    break;
            }
        }
    }
}