using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public sealed class LDtkFieldDrawerRadius : ILDtkGizmoDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly float _gridSize;

        public LDtkFieldDrawerRadius(LDtkFields fields, string identifier, EditorDisplayMode mode, float gridSize)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _gridSize = gridSize;
        }

        public void OnDrawGizmos()
        {
            switch (_mode)
            {
                case EditorDisplayMode.RadiusPx:
                    DrawRadius(_gridSize);
                    break;

                case EditorDisplayMode.RadiusGrid:
                    DrawRadius(1);
                    break;
            }
        }

        private void DrawRadius(float pixelsPerUnit)
        {
            if (pixelsPerUnit == 0)
            {
                Debug.LogError("Did not draw, avoided dividing by zero");
                return;
            }
            float radius = GetRadius() / pixelsPerUnit; 
                
#if UNITY_EDITOR
            if (_fields.GetFirstColor(out Color color))
            {
                UnityEditor.Handles.color = color;
            }
#endif
            
            GizmoAAUtil.DrawAAEllipse(_fields.transform.position, Vector2.one * radius, fillAlpha: 0);
        }
        
        private float GetRadius()
        {
            if (_fields.IsFieldOfType(_identifier, LDtkFieldType.Float))
            {
                return _fields.GetFloat(_identifier);
            }
            if (_fields.IsFieldOfType(_identifier, LDtkFieldType.Int))
            {
                return _fields.GetInt(_identifier);
            }
            return default;
        }


    }
}