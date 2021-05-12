using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    public class SpritePhysicsPointsDrawer : IDisposable
    {
        private Sprite _sprite;
        private Rect _rect;

        private readonly GLDrawInstance _draw;
        
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
            
            DrawInternal();
        }

        private void DrawInternal()
        {

            _draw.DrawInInspector(_rect, () =>
            {
                GL.Color(Color.cyan);
                //GL.Clear(true, false, Color.white);
                GLUtil.DrawRect(_rect, Color.black);
            });

            List<Vector2> list = new List<Vector2>();
            int shapeCount = _sprite.GetPhysicsShapeCount();
            
            for (int i = 0; i < shapeCount; i++)
            {
                _sprite.GetPhysicsShape(i, list);

                Vector2 interpolatedPivot = _sprite.pivot / _sprite.rect.size;
                list = list.Select(p => p =  (p + interpolatedPivot) * _rect.width).ToList();
                
                //DrawShape(list);
                _draw.DrawInInspector(_rect, () =>
                {
                    GLUtil.DrawLineStrip(list.ToArray(), Color.white, true);
                });
                
                list.Clear();
            }
            
        }

        /*private void DrawShape(List<Vector2> points)
        {
            for (int i = 0; i < points.Count-1; i++)
            {
                Vector2 start = points[i];
                Vector2 next = points[i+1];
                DrawLine(start, next);
            }

            Vector2 begin = points.First();
            Vector2 end = points.Last();
            DrawLine(begin, end);
        }

        
        
        private void DrawLine(Vector2 start, Vector2 end)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            
            GUI.BeginClip(_rect);
            GL.PushMatrix();
            GL.Clear(true, false, Color.black);
            
         
            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            GL.Vertex3(start.x, start.y, 0);
            GL.Vertex3(end.x, end.y, 0);
            GL.End();
            GL.PopMatrix();
            GUI.EndClip();
        }*/

        
    }
}