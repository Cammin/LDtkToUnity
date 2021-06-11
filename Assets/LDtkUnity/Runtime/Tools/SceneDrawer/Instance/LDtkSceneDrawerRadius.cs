using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public sealed class LDtkSceneDrawerRadius : LDtkSceneDrawerField
    {
        private readonly float _gridSize;

        public LDtkSceneDrawerRadius(LDtkFields fields, string identifier, EditorDisplayMode mode, float gridSize, Color color) : base(fields, identifier, color)
        {
            _gridSize = gridSize;
        }

        public override void Draw()
        {
            switch (Mode)
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
            UnityEditor.Handles.DrawWireDisc(Transform.position, Vector3.forward, radius);
#endif
                
        }
        
        private float GetRadius()
        {
            if (Fields.IsFieldOfType(Identifier, LDtkFieldType.Float))
            {
                return Fields.GetFloat(Identifier);
            }
            if (Fields.IsFieldOfType(Identifier, LDtkFieldType.Int))
            {
                return Fields.GetInt(Identifier);
            }
            return default;
        }


    }
}