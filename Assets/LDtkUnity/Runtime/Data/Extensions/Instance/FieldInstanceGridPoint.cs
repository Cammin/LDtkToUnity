using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class GridPoint
    {
        /// <value>
        /// Grid-based coordinate
        /// </value>
        [IgnoreDataMember] public Vector2Int UnityCoord => new Vector2Int(Cx, Cy);
    }
}