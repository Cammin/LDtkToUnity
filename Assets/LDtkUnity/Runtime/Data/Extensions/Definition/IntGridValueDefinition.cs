using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        public Color UnityColor => Color.ToColor();
    }
}