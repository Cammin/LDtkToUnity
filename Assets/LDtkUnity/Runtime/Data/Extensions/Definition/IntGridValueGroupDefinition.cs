using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueGroupDefinition : ILDtkIdentifier, ILDtkUid
    {
        /// <summary>
        /// User defined color
        /// </summary>
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}