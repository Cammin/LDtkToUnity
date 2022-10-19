using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerRadius : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly float _gridSize;
        private readonly Color _smartColor;

        public LDtkFieldDrawerRadius(LDtkFields fields, string identifier, EditorDisplayMode mode, float gridSize, Color smartColor)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _gridSize = gridSize;
            _smartColor = smartColor;
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
                LDtkDebug.LogError("Did not draw, avoided dividing by zero");
                return;
            }
            
            float radius = GetRadius() / gridSize; 
            float diameter = radius * 2;
            
            UnityEditor.Handles.color = _smartColor;

            HandleAAUtil.DrawAAEllipse(_fields.transform.position, Vector2.one * diameter, LDtkPrefs.FieldRadiusThickness, 0);
        }
        
        private float GetRadius()
        {
            if (!_fields.ContainsField(_identifier))
            {
                LDtkDebug.LogWarning($"Fields component doesn't contain a field called {_identifier}, this should never happen. Try reverting prefab changes", _fields.gameObject);
                return default;
            }
            
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