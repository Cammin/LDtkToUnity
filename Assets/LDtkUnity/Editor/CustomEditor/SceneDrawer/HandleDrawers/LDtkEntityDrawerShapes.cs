using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Draws the shape of the entity. Box, ellipse, cross.
    /// </summary>
    internal sealed class LDtkEntityDrawerShapes
    {
        private readonly LDtkComponentEntity _entity;

        public LDtkEntityDrawerShapes(LDtkComponentEntity entity)
        {
            _entity = entity;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowEntityShape)
            {
                return;
            }

            if (!_entity.Def.Hollow && LDtkPrefs.EntityOnlyHollow)
            {
                return;
            }
            
            Vector2 size = _entity.Size;
            Vector2 pos = (Vector2)_entity.transform.position + LDtkCoordConverter.EntityPivotOffset(_entity.Def.Pivot, _entity.Size);
            float fillAlpha = GetFillAlpha();
            float lineAlpha = _entity.Def.LineOpacity;
            
            float thickness = LDtkPrefs.EntityShapeThickness;
            switch (_entity.Def.RenderMode)
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

        private float GetFillAlpha()
        {
            if (_entity.Def.Hollow || LDtkPrefs.EntityOnlyBorders)
            {
                return 0;
            }
            return _entity.Def.FillOpacity;
        }
    }
}