using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <summary>
        /// The "color" field converted for use with Unity
        /// </summary>
        [JsonIgnore] public Color UnityColor => Color.ToColor();
    }
}