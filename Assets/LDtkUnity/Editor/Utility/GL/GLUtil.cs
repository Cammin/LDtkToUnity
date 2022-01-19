
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class GLUtil
    {
        public static void DrawLineStrip(Vector2[] points, Color color, bool wrap = false)
        {
            using (new GLDrawScope(GL.LINE_STRIP))
            {
                foreach (Vector2 point in points)
                {
                    GL.Color(color);
                    GL.Vertex3(point.x, point.y, 0);
                }
                
            }

            if (wrap)
            {
                GL.Color(color);
                DrawLine(points.First(), points.Last(), color);
            }
        }
        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            using (new GLDrawScope(GL.LINE_STRIP))
            {
                GL.Color(color);
                GL.Vertex3(start.x, start.y, 0);
                GL.Vertex3(end.x, end.y, 0);
            }
        }

        public static void DrawShape(Vector2[] points, Color color)
        {
            using (new GLDrawScope(GL.TRIANGLE_STRIP))
            {
                foreach (Vector2 point in points)
                {
                    GL.Color(color);
                    GL.Vertex3(point.x, point.y, 0);
                }
            }
        }
        
        public static void DrawHollowRect(Rect rect, Color color)
        {
            Vector2[] points =
            {
                rect.min,
                rect.min + Vector2.right * rect.width,
                rect.max,
                rect.min + Vector2.up * rect.height,
            };
            
            DrawLineStrip(points, color, true);
        }
        
        public static void DrawRect(Rect rect, Color color)
        {
            using (new GLDrawScope(GL.QUADS))
            {
                GL.Color(color);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(rect.width, 0, 0);
                GL.Vertex3(rect.width, rect.height, 0);
                GL.Vertex3(0, rect.height, 0);
            }
        }
        
    }
}