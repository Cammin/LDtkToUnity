using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class EnumValueDefinition
    {
        /// <value>
        /// Optional color
        /// </value>
        [IgnoreDataMember] public Color UnityColor => Color.ToColor(); //todo figure out that this actually works. could use with drawing the color in the imposter inspector?
    }
}