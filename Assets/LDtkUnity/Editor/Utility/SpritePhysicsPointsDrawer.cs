using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    public class SpritePhysicsPointsDrawer : IDisposable
    {
        private readonly GLDrawInstance _draw;
        
        private Sprite _sprite;
        private Rect _rect;

        private Vector2 _convertPos;
        private Vector2 _convertScale;

        public SpritePhysicsPointsDrawer()
        {
            _draw = new GLDrawInstance();
        }
        public void Dispose()
        {
            _draw.Dispose();
        }

        public void Draw(Sprite sprite, Rect rect)
        {
            _sprite = sprite;
            _rect = rect;
            
            CalculateConversionValues();
            DrawBackground();
            DrawShapes();
        }

        private void CalculateConversionValues()
        {
            //took a while to wrack my brain around these calculations
            _convertScale = _rect.size / (_sprite.rect.size / _sprite.pixelsPerUnit);
            _convertPos = _rect.size * (_sprite.pivot / _sprite.rect.size);
        }

        private void DrawShapes()
        {
            List<Vector2> shapePoints = new List<Vector2>();
            for (int i = 0; i < _sprite.GetPhysicsShapeCount(); i++)
            {
                _sprite.GetPhysicsShape(i, shapePoints);
                Vector2[] drawPoints = shapePoints.Select(ConvertPhysicsPoint).ToArray();
                _draw.DrawInInspector(_rect, () =>
                {
                    GLUtil.DrawLineStrip(drawPoints, Color.white, true);
                });

                shapePoints.Clear();
            }
        }

        private void DrawBackground()
        {
            _draw.DrawInInspector(_rect, () =>
            {
                GLUtil.DrawRect(_rect, new Color(0.32f, 0.32f, 0.32f));
            });
        }

        private Vector2 ConvertPhysicsPoint(Vector2 pos)
        {
            pos *= _convertScale;
            pos += _convertPos;

            //flip y
            pos.y *= -1;
            pos.y += _rect.height;
            
            return pos;
        }
    }
}