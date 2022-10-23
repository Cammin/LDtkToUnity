using System.Text.Json.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [JsonIgnore] public Color UnityColor => Color.ToColor();
    }
}