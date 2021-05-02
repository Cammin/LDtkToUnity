using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkSceneDrawerData
    {
        [SerializeField] private Component _source;
        [SerializeField] private string _fieldName;
        
        [SerializeField] private EditorDisplayMode _mode;
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gridSize;

        public LDtkSceneDrawerData(Component source, MemberInfo field, EntityInstance entityData, EditorDisplayMode mode, int gridSize)
        {
            _source = source;
            _fieldName = field.Name;
            _mode = mode;
            _gridSize = gridSize;

            SetGizmoColor(entityData);
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;

#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
#endif

            LDtkSceneDrawerBase drawer = GetDrawer();

            if (drawer != null)
            {
                drawer.SupplyReferences(_source, _fieldName, _mode, _gridSize);
                drawer?.Draw();
            }
        }

        private LDtkSceneDrawerBase GetDrawer()
        {
            switch (_mode)
            {
                case EditorDisplayMode.Hidden:
                case EditorDisplayMode.ValueOnly:
                case EditorDisplayMode.NameAndValue:
                case EditorDisplayMode.EntityTile:
                    //nothing ...yet
                    break;
                    
                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return new LDtkSceneDrawerPoints();
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return new LDtkSceneDrawerRadius();

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
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
    }
}