using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Draws the point or point array. Only for entity, level never draws this
    /// </summary>
    internal sealed class LDtkFieldDrawerPoints : ILDtkHandleDrawer
    {
        private LDtkComponentEntity _entity;
        private LDtkField _field;

        
        
        public void OnDrawHandles(LDtkComponentEntity entity, LDtkField field)
        {
            if (!LDtkPrefs.ShowFieldPoints)
            {
                return;
            }
            
            _entity = entity;
            _field = field;
            
            List<Vector2> points = GetConvertedPoints();
            if (points.IsNullOrEmpty())
            {
                return;
            }

            switch (_field.Definition.EditorDisplayMode)
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
            if (_field == null)
            {
                return Array.Empty<Vector2>();
            }

            if (_field.IsArray)
            {
                LDtkFieldElement[] elements = _field.GetArray();
                List<Vector2> points = new List<Vector2>(elements.Length);
                foreach (LDtkFieldElement element in elements)
                {
                    if (!element.IsNull())
                    {
                        points.Add(element.GetPoint());
                    }
                }
                
                return points.ToArray();
            }

            LDtkFieldElement single = _field.GetSingle();
            if (single.IsNull())
            {
                return Array.Empty<Vector2>();
            }

            return new[] { single.GetPoint() };
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
            Vector2 pos = _entity.MiddleCenter;
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
            float boxUnitSize = 8f / _entity.PixelsPerUnit;
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
            LDtkHandleDrawerUtil.RenderSimpleLink(from, to, _entity.PixelsPerUnit);
        }
    }
}