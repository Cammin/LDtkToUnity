using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkSceneDrawerPoints : LDtkSceneDrawerBase
    {
        public override void Draw()
        {
            List<Vector2> points = GetConvertedPoints();
            if (points.IsNullOrEmpty())
            {
                return;
            }

            switch (Mode)
            {
                case EditorDisplayMode.PointPath:
                    DrawPath(points);
                    break;
                
                case EditorDisplayMode.PointPathLoop:
                    DrawPathLoop(points);
                    break;
                
                case EditorDisplayMode.PointStar:
                    DrawStar(points);
                    break;
                
                case EditorDisplayMode.Points:
                    //already drawn below
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
                        
            DrawPoints(points);
        }

        private Vector2[] GetFieldPoints()
        {
            FieldInfo fieldInfo = GetFieldInfo();
            if (fieldInfo == null)
            {
                return null;
            }
                
            if (fieldInfo.FieldType.IsArray)
            {
                return (Vector2[]) fieldInfo.GetValue(Source);
            }

            Vector2 point = (Vector2)fieldInfo.GetValue(Source);
            return new[] { point };
        }

        private List<Vector2> GetConvertedPoints()
        {
            Vector2[] points = GetFieldPoints();
            if (points.IsNullOrEmpty())
            {
                return null;
            }
            
            List<Vector2> convertedRoute = Array.ConvertAll(points, input => new Vector2(input.x, input.y)).ToList();

            //round the starting position to the bottom left of the current tile
            Vector2 pos = Transform.position;
            pos += (Vector2.one * 0.001f);

            int left = Mathf.FloorToInt(pos.x);
            int right = Mathf.CeilToInt(pos.x);
            pos.x = Mathf.Lerp(left, right, 0.5f);

            int down = Mathf.FloorToInt(pos.y);
            int up = Mathf.CeilToInt(pos.y);
            pos.y = Mathf.Lerp(down, up, 0.5f);

            convertedRoute.Insert(0, pos);
            return convertedRoute;
        }
        
        /// <summary>
        /// Draw a daisy-chain of points
        /// </summary>
        private void DrawPath(List<Vector2> points)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 pointPos = points[i];
                Vector2 nextPointPos = points[i + 1];
                Gizmos.DrawLine(pointPos, nextPointPos);
            }
        }
        
        /// <summary>
        /// Draw a daisy-chain of points that loops back to the start
        /// </summary>
        private void DrawPathLoop(List<Vector2> points)
        {
            DrawPath(points);
            Gizmos.DrawLine(points.First(), points.Last());
        }
        
        /// <summary>
        /// Draw from the first point to all of the rest 
        /// </summary>
        private void DrawStar(List<Vector2> points)
        {
            Vector2 pointPos = points[0];

            for (int i = 1; i < points.Count; i++)
            {
                Vector2 nextPointPos = points[i];
                Gizmos.DrawLine(pointPos, nextPointPos);
            }
        }

        /// <summary>
        /// Draw points in isolation
        /// </summary>
        private void DrawPoints(List<Vector2> points)
        {
            foreach (Vector2 point in points)
            {
                Vector3 size = Vector2.one * 0.25f;
                Gizmos.DrawWireCube(point, size);
            }
        }
    }
}