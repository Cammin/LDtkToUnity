using UnityEngine;

namespace LDtkUnity
{
    public partial class EntityDefinition : ILDtkUid, ILDtkIdentifier
    {
        public Color UnityColor => Color.ToColor();
        public Vector2Int UnitySize => new Vector2Int((int)Width, (int)Height);
        public Vector2 UnityPivot => new Vector2((float)PivotX, (float)PivotY);
    }
}