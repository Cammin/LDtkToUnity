using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerPoints : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly Vector3 _middleCenter;
        private readonly float _pixelsPerUnit;

        public LDtkFieldDrawerPoints(LDtkFields fields, string identifier, EditorDisplayMode mode, Vector3 middleCenter, int pixelsPerUnit)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _middleCenter = middleCenter;
            _pixelsPerUnit = pixelsPerUnit;
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
            
            DrawIsolatedPoints(points);
        }

        private Vector2[] GetFieldPoints()
        {
            if (!_fields.ContainsField(_identifier))
            {
                LDtkDebug.LogWarning($"Fields component doesn't contain a field called {_identifier}, this should never happen. Try reverting prefab changes", _fields.gameObject);
                return Array.Empty<Vector2>();
            }
            
            if (_fields.IsFieldArray(_identifier))
            {
                _fields.TryGetPointArray(_identifier, out Vector2[] objs);

                List<Vector2> points = new List<Vector2>();

                for (int i = 0; i < objs.Length; i++)
                {
                    if (!_fields.IsNullAtArrayIndex(_identifier, i))
                    {
                        points.Add(objs[i]);
                    }
                }
                
                return points.ToArray();
            }

            if (_fields.IsNull(_identifier))
            {
                return Array.Empty<Vector2>();
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

        private void DrawPath(List<Vector2> points)
        {
            Vector3[] pathPoints = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                pathPoints[i] = points[i];
            }
            
            
            for (int i = 0; i < points.Count-1; i++)
            {
                Vector2 pointPos = pathPoints[i];
                Vector2 nextPointPos = pathPoints[i+1];
                DrawLine(pointPos, nextPointPos);
            }
        }
        
        private void DrawPathLoop(List<Vector2> points)
        {
            DrawPath(points);
            DrawLine(points.First(), points.Last());
        }
        
        private void DrawStar(List<Vector2> points)
        {
            Vector2 pointPos = points[0];
            
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 nextPointPos = points[i];
                DrawLine(pointPos, nextPointPos);
            }
        }
        
        private void DrawIsolatedPoints(List<Vector2> points)
        {
            float boxUnitSize = 8f / _pixelsPerUnit;
            float thickness = LDtkPrefs.FieldPointsThickness;
            float extraThickness = 0;//(thickness - 1) * (boxUnitSize/3);
            Vector3 size = Vector2.one * (boxUnitSize + extraThickness);
            
            foreach (Vector2 point in points)
            {
                HandleAAUtil.DrawAADiamond(point, size, fillAlpha: 0, thickness: thickness);
            }
        }
        
        //there was an idea co concat all these crooked paths together, but that can be saved for another day
        private void DrawLine(Vector3 from, Vector3 to)
        {
            LDtkHandleDrawerUtil.RenderSimpleLink(from, to, _pixelsPerUnit); //todo this gridSize should be the importer's pixels per unit instead to behave better
        }
    }
}