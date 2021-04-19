using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class IntGridValueDefinition : ILDtkIdentifier
    {
        /// <summary>
        /// The "color" field converted for use with Unity
        /// </summary>
        [JsonIgnore] public Color UnityColor => Color.ToColor();
    }
}