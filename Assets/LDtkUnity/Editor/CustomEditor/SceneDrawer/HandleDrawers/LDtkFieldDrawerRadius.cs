using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkFieldDrawerRadius : ILDtkHandleDrawer
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

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowFieldRadius)
            {
                return;
            }
            
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

        private void DrawRadius(float gridSize)
        {
            if (gridSize == 0)
            {
                Debug.LogError("Did not draw, avoided dividing by zero");
                return;
            }
            
            float radius = GetRadius() / gridSize; 
            float diameter = radius * 2;
            
            if (_fields.GetFirstColor(out Color color))
            {
                UnityEditor.Handles.color = color;
            }
            
            HandleAAUtil.DrawAAEllipse(_fields.transform.position, Vector2.one * diameter, LDtkPrefs.FieldRadiusThickness, 0);
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