using System.Collections.Generic;
using System.Text;

namespace LDtkUnity.Editor
{
    public readonly struct TilemapKey : IEqualityComparer<TilemapKey>
    {
        public readonly string Tag;
        public readonly int LayerMask;

        public TilemapKey(string tag, int layerMask)
        {
            Tag = tag;
            LayerMask = layerMask;
        }

        public bool Equals(TilemapKey x, TilemapKey y)
        {
            return x.Tag == y.Tag && x.LayerMask == y.LayerMask;
        }

        public int GetHashCode(TilemapKey obj)
        {
            unchecked
            {
                return ((obj.Tag != null ? obj.Tag.GetHashCode() : 0) * 397) ^ obj.LayerMask;
            }
        }

        public string GetNameFormat(string startName)
        {
            StringBuilder str = new StringBuilder();
            str.Append(startName);
            
            if (Tag != "Untagged")
            {
                str.Append($"_{Tag}");
            }
            
            if (LayerMask != 0)
            {
                string name = UnityEngine.LayerMask.LayerToName(LayerMask);
                str.Append($"_{name}");
            }

            return str.ToString();
        }
    }
}