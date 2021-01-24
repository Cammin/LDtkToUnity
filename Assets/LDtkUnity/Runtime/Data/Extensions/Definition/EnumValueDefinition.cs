using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity
{
    public partial class EnumValueDefinition
    {
        public Rect SourceRect => TileSrcRect.ToRect();
    }
}