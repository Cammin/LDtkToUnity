using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal readonly struct TilemapKey : IEqualityComparer<TilemapKey>
    {
        public readonly string Tag;
        public readonly int LayerMask;
        public readonly PhysicsMaterial2D PhysicsMaterial;

        public TilemapKey(string tag, int layerMask, PhysicsMaterial2D physicsMaterial)
        {
            Tag = tag;
            LayerMask = layerMask;
            PhysicsMaterial = physicsMaterial;

            //this is to fix a reference issue and unity crash
            if (physicsMaterial == null)
            {
                PhysicsMaterial = null;
            }
        }
        
        public bool Equals(TilemapKey x, TilemapKey y)
        {
            return x.Tag == y.Tag && x.LayerMask == y.LayerMask && Equals(x.PhysicsMaterial, y.PhysicsMaterial);
        }

        public int GetHashCode(TilemapKey obj)
        {
            unchecked
            {
                int hashCode = (obj.Tag != null ? obj.Tag.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.LayerMask;
                hashCode = (hashCode * 397) ^ (obj.PhysicsMaterial != null ? obj.PhysicsMaterial.GetHashCode() : 0);
                return hashCode;
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

            if (PhysicsMaterial != null)
            {
                str.Append($"_{PhysicsMaterial.name}");
            }

            return str.ToString();
        }

        public override string ToString()
        {
            return $"TilemapKey: Tag:{Tag}, LayerMask:{LayerMask}, PhysicsMaterial:{PhysicsMaterial},";
        }
    }
}