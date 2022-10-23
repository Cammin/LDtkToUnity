using System.Text.Json.Serialization;
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

        /// <value>
        /// Optional color
        /// </value>
        [JsonIgnore] public Color UnityColor => Color.ToColor(); //todo figure out that this actually works. could use with drawing the color in the imposter inspector?
    }
}