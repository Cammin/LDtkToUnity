using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerRadius : ILDtkHandleDrawer
    {
        private LDtkComponentEntity _entity;
        private LDtkField _field;

        public void OnDrawHandles(LDtkComponentEntity entity, LDtkField field)
        {
            if (!LDtkPrefs.ShowFieldRadius)
            {
                return;
            }
            
            _entity = entity;
            _field = field;
            
            switch (_field.Definition.EditorDisplayMode)
            {
                case EditorDisplayMode.RadiusPx:
                    DrawRadius(_entity.Parent.GridSize);
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

            HandleAAUtil.DrawAAEllipse(_entity.transform.position, Vector2.one * diameter, LDtkPrefs.FieldRadiusThickness, 0);
        }
        
        private float GetRadius()
        {
            if (_field.Type == LDtkFieldType.Float)
            {
                return _field.GetSingle().GetFloat();
            }
            if (_field.Type == LDtkFieldType.Int)
            {
                return _field.GetSingle().GetInt();
            }
            return 0;
        }
    }
}