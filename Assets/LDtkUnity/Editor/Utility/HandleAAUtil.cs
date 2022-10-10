using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class HandleAAUtil
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
            
            Color newColor = Handles.color;
            newColor.a = HandleUtil.GetAlphaForDistance();
            
            using (new Handles.DrawingScope(newColor))
            {
                Handles.DrawAAPolyLine(thickness, points);
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

            Vector3[] points = 
            {
                topRightPos,
                rightPos,
                bottomPos,
                leftPos,
                topPos,
                topRightPos
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
                Vector3 point = pos + new Vector3(circ.x, circ.y, pos.z);
                points[i] = point;
            }

            points[pointCount] = points[0];
            
            DrawAAShape(points, thickness, fillAlpha, lineAlpha);
        }

        private static void DrawAAShape(Vector3[] points, float thickness = DEFAULT_THICKNESS, float fillAlpha = DEFAULT_FILL_ALPHA, float lineAlpha = DEFAULT_LINE_ALPHA)
        {
            Color newColor = Handles.color;
            
            Vector3 guiPoint = HandleUtility.WorldToGUIPointWithDepth(points[0]);
            newColor.a = HandleUtil.GetAlphaForDistance();

            if (newColor.a <= 0)
            {
                return;
            }

            //fill
            Color fillColor = newColor;
            fillColor.a *= fillAlpha;
            using (new Handles.DrawingScope(fillColor))
            {
                Handles.DrawAAConvexPolygon(points);
            }
            
            //line
            Color lineColor = newColor;
            lineColor.a *= lineAlpha;
            using (new Handles.DrawingScope(lineColor))
            {
                Handles.DrawAAPolyLine(thickness, points);
            }
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