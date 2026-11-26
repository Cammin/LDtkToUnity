using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class HandleAAUtil
    {
        private const float DEFAULT_THICKNESS = LDtkPrefs.THICKNESS_DEFAULT;
        private const float DEFAULT_FILL_ALPHA = 0.1f;
        private const float DEFAULT_LINE_ALPHA = 1f;
        
        private static Vector3[] Points2 = new Vector3[2];
        private static Vector3[] Points6 = new Vector3[6];
        private static Vector3[] EllipseBuffer = new Vector3[ELLIPSE_POINTS+1];

        private const int ELLIPSE_POINTS = 32;
        
        public static void DrawAACross(Vector3 pos, Vector2 size, float thickness = DEFAULT_THICKNESS)
        {
            if (IsIllegalPoint(pos))
            {
                return;
            }
            
            float left = pos.x - size.x/2;
            float right = pos.x + size.x/2;
            float top = pos.y + size.y/2;
            float bottom = pos.y - size.y/2;
            
            Vector3 topLeft = new Vector3(left, top, pos.z);
            Vector3 topRight = new Vector3(right, top, pos.z);
            Vector3 bottomRight = new Vector3(right, bottom, pos.z);
            Vector3 bottomLeft = new Vector3(left, bottom, pos.z);

            DrawAALine(topLeft, bottomRight, thickness);
            DrawAALine(topRight, bottomLeft, thickness);
        }

        public static void DrawAALine(Vector3 start, Vector3 end, float thickness = DEFAULT_THICKNESS)
        {
            Points2[0] = start;
            Points2[1] = end;
            DrawAAPath(Points2, thickness);
        }
        
        public static void DrawAAPath(Vector3[] points, float thickness = DEFAULT_THICKNESS)
        {
            foreach (Vector3 point in points)
            {
                if (IsIllegalPoint(point)) return;
            }
            
            if (!HandleUtil.IsShapeVisible(points)) return;
            
            Color newColor = Handles.color;
            newColor.a = HandleUtil.GetAlphaForDistance();
            
            using (new Handles.DrawingScope(newColor))
            {
                LDtkProfiler.BeginSample("DrawAAPolyLine");
                Handles.DrawAAPolyLine(thickness, points);
                LDtkProfiler.EndSample();
            }
        }

        
        public static void DrawAABox(Vector3 center, Vector2 size, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            if (IsIllegalPoint(center))
            {
                return;
            }
            
            float left = center.x - size.x/2;
            float right = center.x + size.x/2;
            float top = center.y + size.y/2;
            float bottom = center.y - size.y/2;

            Vector3 topMiddle = new Vector3(center.x, top, center.z);
            Vector3 topLeft = new Vector3(left, top, center.z);
            Vector3 topRight = new Vector3(right, top, center.z);
            Vector3 bottomRight = new Vector3(right, bottom, center.z);
            Vector3 bottomLeft = new Vector3(left, bottom, center.z);

            Points6[0] = topMiddle;
            Points6[1] = topRight;
            Points6[2] = bottomRight;
            Points6[3] = bottomLeft;
            Points6[4] = topLeft;
            Points6[5] = topMiddle;
            
            DrawAAShape(Points6, thickness, fillAlpha, lineAlpha);
        }
        
        public static void DrawAADiamond(Vector3 pos, Vector2 size, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            if (IsIllegalPoint(pos))
            {
                return;
            }

            float left = pos.x - size.x / 2;
            float right = pos.x + size.x/2;
            float top = pos.y + size.y/2;
            float bottom = pos.y - size.y/2;

            Vector3 topRightPos = pos;
            topRightPos.x = right - size.x/4;
            topRightPos.y = top - size.y/4;
            
            Vector3 topPos = pos;
            topPos.y = top;
            
            Vector3 leftPos = pos;
            leftPos.x = left;
            
            Vector3 rightPos = pos;
            rightPos.x = right;
            
            Vector3 bottomPos = pos;
            bottomPos.y = bottom;
            
            Points6[0] = topRightPos;
            Points6[1] = rightPos;
            Points6[2] = bottomPos;
            Points6[3] = leftPos;
            Points6[4] = topPos;
            Points6[5] = topRightPos;
            
            DrawAAShape(Points6, thickness, fillAlpha, lineAlpha);
        }
        
        public static void DrawAAEllipse(Vector3 pos, Vector2 size, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            if (IsIllegalPoint(pos))
            {
                return;
            }
            
            for (int i = 0; i < ELLIPSE_POINTS; i++)
            {
                float fraction = (i / (float)ELLIPSE_POINTS) * Mathf.PI * 2;
                Vector2 circ = new Vector2(Mathf.Cos(fraction), Mathf.Sin(fraction));
                circ *= size * 0.5f;
                Vector3 point = pos + new Vector3(circ.x, circ.y, 0);
                EllipseBuffer[i] = point;
            }
            EllipseBuffer[ELLIPSE_POINTS] = EllipseBuffer[0];
            
            DrawAAShape(EllipseBuffer, thickness, fillAlpha, lineAlpha);
        }

        private static void DrawAAShape(Vector3[] points, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            if (!HandleUtil.IsShapeVisible(points)) return;
            
            Color newColor = Handles.color;
            
            
            newColor.a = HandleUtil.GetAlphaForDistance();

            if (newColor.a <= 0)
            {
                return;
            }
            
            //fill
            Color fillColor = newColor;
            fillColor.a *= fillAlpha;
            if (fillColor.a > 0)
            {
                using (new Handles.DrawingScope(fillColor))
                {
                    LDtkProfiler.BeginSample("DrawAAShape.Polygon");
                    Handles.DrawAAConvexPolygon(points);
                    LDtkProfiler.EndSample();
                }
            }
            
            //line
            Color lineColor = newColor;
            lineColor.a *= lineAlpha;
            if (lineColor.a > 0)
            {
                using (new Handles.DrawingScope(lineColor))
                {
                    LDtkProfiler.BeginSample("DrawAAShape.PolyLine");
                    Handles.DrawAAPolyLine(thickness, points);
                    LDtkProfiler.EndSample();
                }
            }
        }

        public static bool IsIllegalPoint(Vector2 point)
        {
            return
                IsIllegal(point.x) ||
                IsIllegal(point.y);
        }
        public static bool IsIllegalPoint(Vector3 point)
        {
            return
                IsIllegal(point.x) ||
                IsIllegal(point.y) ||
                IsIllegal(point.z);
        }
        
        private static bool IsIllegal(float value) =>
            float.IsNaN(value) || float.IsInfinity(value);
        
        
    }
}