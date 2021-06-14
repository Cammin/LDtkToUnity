using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /*[ExcludeFromDocs]
    [Serializable]
    public class LDtkFieldDrawerData
    {
        
        //[SerializeField] private EditorDisplayMode _mode;
        
        

        private LDtkSceneDrawerBase _drawer;

        public LDtkFieldDrawerData(LDtkFields fields, string identifier, Color srcColor, EditorDisplayMode mode, int gridSize)
        {
            _fields = fields;
            _fieldName = identifier;
            _mode = mode;
            _gridSize = gridSize;

            _gizmoColor = srcColor;
        }

        public void Init()
        {
            _drawer = GetDrawer();
        }
        
        
        public void OnDrawGizmos()
        {
            if (_drawer == null)
            {
                //safe return, we never intended to draw this one if it was set null
                return;
            }
            
            if (_fields == null)
            {
                Debug.LogError("LDtk: Source is null, not drawing");
                return;
            }
            
            Gizmos.color = _gizmoColor;
#if UNITY_EDITOR
            UnityEditor.Handles.color = _gizmoColor;
#endif

            _drawer.Draw();
        }

        private LDtkSceneDrawerBase GetDrawer()
        {
            switch (_mode)
            {
                case EditorDisplayMode.Hidden: //show nothing
                case EditorDisplayMode.ValueOnly: //dont show the value todo unless we wanna with that neat value displayer?
                case EditorDisplayMode.NameAndValue: //dont show the value todo unless we wanna with that neat value displayer?
                    
                case EditorDisplayMode.EntityTile: //replaces the entity tile, so it's special todo target this eventually

                    break;
                    
                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return new LDtkSceneDrawerPoints(_fields, _fieldName, _mode);
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return new LDtkSceneDrawerRadius(_fields, _fieldName, _mode, _gridSize);

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
    }*/
}