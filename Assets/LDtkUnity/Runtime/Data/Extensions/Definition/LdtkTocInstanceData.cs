using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LdtkTocInstanceData
    {
        [IgnoreDataMember] public Vector2Int UnitySizePx => new Vector2Int(WidPx, HeiPx);
        [IgnoreDataMember] public Vector2Int UnityWorld => new Vector2Int(WorldX, WorldY);
    }
}