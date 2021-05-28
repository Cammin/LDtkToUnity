using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class EnumValueDefinition
    {
        /// <value>
        /// Rect that refers to the tile in the tileset image of this enum value's definition
        /// </value>
        [JsonIgnore] public Rect UnityTileSrcRect => TileSrcRect.ToRect();
    }
}