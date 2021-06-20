using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class HandleAAUtil
    {
        private const float DEFAULT_THICKNESS = LDtkPrefs.THICKNESS_DEFAULT;
        private const float DEFAULT_FILL_ALPHA = 0.1f;
        private const float DEFAULT_LINE_ALPHA = 1f;
        
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
            DrawAAPath(new []{start, end}, thickness);
        }
        
        public static void DrawAAPath(Vector3[] points, float thickness = DEFAULT_THICKNESS)
        {
            if (points.Any(IsIllegalPoint))
            {
                return;
            }
            
            Handles.DrawAAPolyLine(thickness, points);
        }
        
        public static void DrawAABox(Vector3 pos, Vector2 size, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            if (IsIllegalPoint(pos))
            {
                return;
            }
            
            float left = pos.x - size.x/2; //todo this is duped over at drawCross. refactor
            float right = pos.x + size.x/2;
            float top = pos.y + size.y/2;
            float bottom = pos.y - size.y/2;

            Vector3 topMiddle = new Vector3(pos.x, top, pos.z);
            Vector3 topLeft = new Vector3(left, top, pos.z);
            Vector3 topRight = new Vector3(right, top, pos.z);
            Vector3 bottomRight = new Vector3(right, bottom, pos.z);
            Vector3 bottomLeft = new Vector3(left, bottom, pos.z);

            Vector3[] points = 
            {
                topMiddle,
                topRight,
                bottomRight,
                bottomLeft,
                topLeft,
                topMiddle,
            };
            
            DrawAAShape(points, thickness, fillAlpha, lineAlpha);
        }
        
        public static void DrawAAEllipse(Vector3 pos, Vector2 size, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA, int pointCount = 50)
        {
            if (IsIllegalPoint(pos))
            {
                return;
            }
            
            Vector3[] points = new Vector3[pointCount+1];

            for (int i = 0; i < pointCount; i++)
            {
                float fraction = (i / (float)pointCount) * Mathf.PI * 2;
                Vector2 circ = new Vector2(Mathf.Cos(fraction), Mathf.Sin(fraction));
                circ *= size * 0.5f;
                /*float x = Mathf.Cos(fraction) * size.x * 0.5f;
                float y = Mathf.Sin(fraction) * size.y;*/
                Vector3 point = pos + new Vector3(circ.x, circ.y, pos.z);
                points[i] = point;
            }

            points[pointCount] = points[0];
            
            DrawAAShape(points, thickness, fillAlpha, lineAlpha);
        }
        
        private static void DrawAAShape(Vector3[] points, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            Color prevColor = Handles.color;

            //fill
            Color fillColor = prevColor;
            fillColor.a *= fillAlpha;
            Handles.color = fillColor;
            Handles.DrawAAConvexPolygon(points);

            //line
            Color lineColor = prevColor;
            lineColor.a = lineAlpha;
            Handles.color = lineColor;
            Handles.DrawAAPolyLine(thickness, points);
            
            Handles.color = prevColor;
        }

        public static bool IsIllegalPoint(Vector2 point)
        {
            return
                float.IsInfinity(point.x) ||
                float.IsInfinity(point.y);
        }
        public static bool IsIllegalPoint(Vector3 point)
        {
            return
                float.IsInfinity(point.x) ||
                float.IsInfinity(point.y) ||
                float.IsInfinity(point.z);
        }
    }
}