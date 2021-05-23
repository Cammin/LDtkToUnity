using System;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkSceneDrawerData
    {
        [SerializeField] private LDtkFields _fields;
        [SerializeField] private string _fieldName;
        
        [SerializeField] private EditorDisplayMode _mode;
        [SerializeField] private Color _gizmoColor;
        [SerializeField] private float _gridSize;

        public LDtkSceneDrawerData(LDtkFields fields, string identifier, Color srcColor, EditorDisplayMode mode, int gridSize)
        {
            _fields = fields;
            _fieldName = identifier;
            _mode = mode;
            _gridSize = gridSize;
            
            SetGizmoColor(srcColor);
        }
        
        public void OnDrawGizmos()
        {
            if (_fields == null)
            {
                Debug.LogError("LDtk: Source is null, not drawing");
                return;
            }
            
            Gizmos.color = _gizmoColor;

#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
#endif

            LDtkSceneDrawerBase drawer = GetDrawer();

            if (drawer != null)
            {
                drawer.SupplyReferences(_fields, _fieldName, _mode, _gridSize);
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
        
        private void SetGizmoColor(Color gizmoColor)
        {
            gizmoColor.a = 0.66f;
            const float incrementDifference = -0.1f;
            gizmoColor.r += incrementDifference;
            gizmoColor.g += incrementDifference;
            gizmoColor.b += incrementDifference;
            _gizmoColor = gizmoColor;
        }
    }
}