using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class AtlasTileRectangle
    {
        /// <value>
        /// Rectangle of the tile in the Tileset atlas
        /// </value>
        [JsonIgnore] public Rect UnityRect => new Rect(X, Y, W, H);
    }
}