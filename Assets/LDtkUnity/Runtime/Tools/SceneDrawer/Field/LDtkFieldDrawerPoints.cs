using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public sealed class LDtkFieldDrawerPoints : ILDtkGizmoDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;

        public LDtkFieldDrawerPoints(LDtkFields fields, string identifier, EditorDisplayMode mode)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
        }
        
        public void OnDrawGizmos()
        {
            List<Vector2> points = GetConvertedPoints();
            if (points.IsNullOrEmpty())
            {
                return;
            }

            switch (_mode)
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
            if (_fields.IsFieldArray(_identifier))
            {
                return _fields.GetPointArray(_identifier);
            }

            Vector2 point = _fields.GetPoint(_identifier);
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


            //if the parsed point was nullable null, then it's going to be negative infinity. Don't draw these null values 
            convertedRoute.RemoveAll(p => p == Vector2.negativeInfinity);

            //round the starting position to the bottom left of the current tile
            Vector2 pos = _fields.transform.position;
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

            Vector3[] pointss = new Vector3[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                pointss[i] = points[i];
            }
            
            GizmoUtil.DrawAAPath(pointss);
        }
        
        /// <summary>
        /// Draw a daisy-chain of points that loops back to the start
        /// </summary>
        private void DrawPathLoop(List<Vector2> points)
        {
            DrawPath(points);
            GizmoUtil.DrawAALine(points.First(), points.Last());
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
                GizmoUtil.DrawAALine(pointPos, nextPointPos);
            }
        }

        /// <summary>
        /// Draw points in isolation
        /// </summary>
        private void DrawPoints(List<Vector2> points)
        {
            Vector3 size = Vector2.one * 0.25f;
            
            foreach (Vector2 point in points)
            {
                if (point == Vector2.negativeInfinity || point == Vector2.positiveInfinity)
                {
                    continue;
                }
                
                GizmoUtil.DrawAABox(point, size, alphaFactor: 0);
            }
        }
        
    }
}