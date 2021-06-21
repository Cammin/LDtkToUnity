using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkFieldDrawerPoints : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly Vector3 _middleCenter;

        public LDtkFieldDrawerPoints(LDtkFields fields, string identifier, EditorDisplayMode mode, Vector3 middleCenter)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _middleCenter = middleCenter;
        }
        
        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowFieldPoints)
            {
                return;
            }
            
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
            convertedRoute.RemoveAll(HandleAAUtil.IsIllegalPoint);

            //round the starting position to the bottom left of the current tile
            Vector2 pos = _middleCenter;
            /*pos += (Vector2.one * 0.001f);

            int left = Mathf.FloorToInt(pos.x);
            int right = Mathf.CeilToInt(pos.x);
            pos.x = Mathf.Lerp(left, right, 0.5f);

            int down = Mathf.FloorToInt(pos.y);
            int up = Mathf.CeilToInt(pos.y);
            pos.y = Mathf.Lerp(down, up, 0.5f);*/

            //if we actually have something, then draw our starting point from
            if (!convertedRoute.IsNullOrEmpty())
            {
                convertedRoute.Insert(0, pos);
            }
            return convertedRoute;
        }
        
        /// <summary>
        /// Draw a daisy-chain of points
        /// </summary>
        private void DrawPath(List<Vector2> points)
        {
            Vector3[] pathPoints = new Vector3[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                pathPoints[i] = points[i];
            }
            
            HandleAAUtil.DrawAAPath(pathPoints, LDtkPrefs.FieldPointsThickness);
        }
        
        /// <summary>
        /// Draw a daisy-chain of points that loops back to the start
        /// </summary>
        private void DrawPathLoop(List<Vector2> points)
        {
            DrawPath(points);
            HandleAAUtil.DrawAALine(points.First(), points.Last(), LDtkPrefs.FieldPointsThickness);
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
                HandleAAUtil.DrawAALine(pointPos, nextPointPos, LDtkPrefs.FieldPointsThickness);
            }
        }

        /// <summary>
        /// Draw points in isolation
        /// </summary>
        private void DrawPoints(List<Vector2> points)
        {
            
            
            
            const float boxUnitSize = 0.2f;
            
            float extraThickness = (LDtkPrefs.FieldPointsThickness - 1) * (boxUnitSize/2);
            
            Vector3 size = Vector2.one * (boxUnitSize + extraThickness);
            
            foreach (Vector2 point in points)
            {
                HandleAAUtil.DrawAABox(point, size, fillAlpha: 0, thickness: LDtkPrefs.FieldPointsThickness);
            }
        }
        
    }
}