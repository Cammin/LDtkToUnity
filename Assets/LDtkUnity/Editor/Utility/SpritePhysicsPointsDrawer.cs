using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    public class SpritePhysicsPointsDrawer : IDisposable
    {
        private readonly Material _mat;
        
        private Sprite _sprite;
        private Rect _rect;
        
        public SpritePhysicsPointsDrawer()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            _mat = new Material(shader);
        }
        public void Dispose()
        {
            Object.DestroyImmediate(_mat);
        }

        public void Draw(Sprite sprite, Rect rect)
        {
            _sprite = sprite;
            _rect = rect;
            
            
            List<Vector2> list = new List<Vector2>();

            for (int i = 0; i < _sprite.GetPhysicsShapeCount(); i++)
            {
                _sprite.GetPhysicsShape(i, list);
                DrawShape(list);
                list.Clear();
            }
        }

        private void DrawShape(List<Vector2> points)
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
            _mat.SetPass(0);
         
            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            GL.Vertex3(start.x, start.y, 0);
            GL.Vertex3(end.x, end.y, 0);
            GL.End();
            GUI.EndClip();
        }

        
    }
}