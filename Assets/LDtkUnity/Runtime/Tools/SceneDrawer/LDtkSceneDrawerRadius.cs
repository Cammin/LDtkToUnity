using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public class LDtkSceneDrawerRadius : LDtkSceneDrawerBase
    {
        public override void Draw()
        {
            switch (Mode)
            {
                case EditorDisplayMode.RadiusPx:
                    DrawRadius(GridSize);
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