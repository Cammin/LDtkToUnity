using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu("")]
    public class LDtkSceneDrawer : MonoBehaviour
    {
        [SerializeField] private Component _source;
        [SerializeField] private string _fieldName;
        
        [SerializeField] private EditorDisplayMode _mode;
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gridSize;

        private FieldInfo GetFieldInfo()
        {
            if (_source == null || string.IsNullOrEmpty(_fieldName))
            {
                Debug.LogError("GetField Error");
                return null;
            }

            Type type = _source.GetType();
            
            FieldInfo fieldInfo = type.GetField(_fieldName);
            return fieldInfo;
        }

        public void SetReference(Component source, FieldInfo field, EntityInstance entityData, EditorDisplayMode mode, int gridSize)
        {
            if (!Application.isEditor)
            {
                return;
            }

            _source = source;
            _fieldName = field.Name;
            _mode = mode;
            _gridSize = gridSize;

            SetGizmoColor(entityData);
        }

        private void SetGizmoColor(EntityInstance entityData)
        {
            Color gizmoColor = entityData.Definition.UnityColor;
            gizmoColor.a = 0.66f;
            const float incrementDifference = -0.1f;
            gizmoColor.r += incrementDifference;
            gizmoColor.g += incrementDifference;
            gizmoColor.b += incrementDifference;
            _gizmoColor = gizmoColor;
        }

        private Vector2[] GetPoints()
        {
            FieldInfo fieldInfo = GetFieldInfo();
            if (fieldInfo == null)
            {
                return null;
            }
            
            if (fieldInfo.FieldType.IsArray)
            {
                return (Vector2[]) fieldInfo.GetValue(_source);
            }

            Vector2 point = (Vector2)fieldInfo.GetValue(_source);
            return new[] { point };
        }

        private float GetRadius()
        {
            FieldInfo fieldInfo = GetFieldInfo();
            if (fieldInfo == null)
            {
                return default;
            }
            
            if (fieldInfo.FieldType == typeof(float))
            {
                return (float) fieldInfo.GetValue(_source);
            }
            if (fieldInfo.FieldType == typeof(int))
            {
                return (int) fieldInfo.GetValue(_source);
            }
            return default;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;

            switch (_mode)
            {
                case EditorDisplayMode.Hidden:
                case EditorDisplayMode.ValueOnly:
                case EditorDisplayMode.NameAndValue:
                case EditorDisplayMode.EntityTile:
                    //nothing
                    break;
                
                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                    DrawPoints();
                    break;
                
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    DrawRadius();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawRadius()
        {
            switch (_mode)
            {
                case EditorDisplayMode.RadiusPx:
                    DrawRadiusInternal(_gridSize);
                    break;

                case EditorDisplayMode.RadiusGrid:
                    DrawRadiusInternal(1);
                    break;
            }
        }

        private void DrawRadiusInternal(float pixelsPerUnit)
        {
            if (pixelsPerUnit == 0)
            {
                Debug.LogError("Did not draw, avoided dividing by zero");
                return;
            }
            float radius = GetRadius() / pixelsPerUnit; 
            
#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
#endif
            
        }
        
        private void DrawPoints()
        {
            Vector2[] points = GetPoints();
            if (points.NullOrEmpty())
            {
                return;
            }

            List<Vector2> convertedRoute = Array.ConvertAll(points, input => new Vector2(input.x, input.y)).ToList();

            //round the starting position to the bottom left of the current tile
            Vector2 pos = transform.position;
            pos += (Vector2.one * 0.001f);
            
            int left = Mathf.FloorToInt(pos.x);
            int right = Mathf.CeilToInt(pos.x);
            pos.x = Mathf.Lerp(left, right, 0.5f);
            
            int down = Mathf.FloorToInt(pos.y);
            int up = Mathf.CeilToInt(pos.y);
            pos.y = Mathf.Lerp(down, up, 0.5f);
            
            convertedRoute.Insert(0, pos);

            switch (_mode)
            {
                case EditorDisplayMode.PointPath:
                    DrawPath(convertedRoute);
                    break;
                case EditorDisplayMode.PointStar:
                    DrawStar(convertedRoute);
                    break;
            }
        }
        
        private void DrawPath(List<Vector2> convertedRoute)
        {
            for (int i = 0; i < convertedRoute.Count - 1; i++)
            {
                Vector2 pointPos = convertedRoute[i];
                Vector2 nextPointPos = convertedRoute[i + 1];
                Gizmos.DrawLine(pointPos, nextPointPos);
            }
        }
        private void DrawStar(List<Vector2> convertedRoute)
        {
            Vector2 pointPos = convertedRoute[0];

            for (int i = 1; i < convertedRoute.Count; i++)
            {
                Vector2 nextPointPos = convertedRoute[i];
                Gizmos.DrawLine(pointPos, nextPointPos);
            }
        }
    }
}