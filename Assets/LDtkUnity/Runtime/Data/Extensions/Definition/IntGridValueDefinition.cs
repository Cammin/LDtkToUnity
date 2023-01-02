using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [IgnoreDataMember] public Color UnityColor => Color.ToColor();
    }
}