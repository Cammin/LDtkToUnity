
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class GLUtil
    {
        private static void DrawLineStrip(Vector2[] points)
        {
            using (new GLDrawScope(GL.LINE_STRIP))
            {
                foreach (Vector2 point in points)
                {
                    GL.Vertex3(point.x, point.y, 0);
                }
            }
        }
        private static void DrawLine(Vector2 start, Vector2 end)
        {
            using (new GLDrawScope(GL.LINE_STRIP))
            {
                GL.Vertex3(start.x, start.y, 0);
                GL.Vertex3(end.x, end.y, 0);
            }
        }

        private static void DrawRect(Rect rect)
        {
            using (new GLDrawScope(GL.QUADS))
            {
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(rect.width, 0, 0);
                GL.Vertex3(rect.width, rect.height, 0);
                GL.Vertex3(0, rect.height, 0);
            }
        }
    }
}