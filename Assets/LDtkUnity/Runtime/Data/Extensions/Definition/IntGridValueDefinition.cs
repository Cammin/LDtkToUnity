using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <value>
        /// The "color" field converted for use with Unity
        /// </value>
        [JsonIgnore] public Color UnityColor => Color.ToColor();
    }
}