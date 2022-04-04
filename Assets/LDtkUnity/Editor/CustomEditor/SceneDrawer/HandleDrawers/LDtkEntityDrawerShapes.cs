using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkEntityDrawerShapes : ILDtkHandleDrawer
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
            float fillAlpha = _data.Hollow ? 0 : _data.FillOpacity; //todo currently as a tile, ldtk still draws the fill even if set as hollow. Is likely a issue with ldtk; wait for fix to arrive 
            
            Vector2 size = _data.Size;

            Vector2 offset = LDtkCoordConverter.EntityPivotOffset(_data.Pivot, _data.Size);
            Vector2 pos = (Vector2)_transform.position + offset;

            DrawShape(pos, size, fillAlpha, lineAlpha);
        }

        private void DrawShape(Vector2 pos, Vector2 size, float fillAlpha, float lineAlpha)
        {
            float thickness = LDtkPrefs.EntityShapeThickness;
            switch (_data.EntityMode)
            {
                case RenderMode.Cross:
                    HandleAAUtil.DrawAACross(pos, size, thickness);
                    break;

                case RenderMode.Ellipse:
                    HandleAAUtil.DrawAAEllipse(pos, size, thickness, fillAlpha, lineAlpha);
                    break;

                case RenderMode.Rectangle:
                    HandleAAUtil.DrawAABox(pos, size, thickness, fillAlpha, lineAlpha);
                    break;
                
                case RenderMode.Tile:
                    HandleAAUtil.DrawAABox(pos, size, thickness, fillAlpha, lineAlpha);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}