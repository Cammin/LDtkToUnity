using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Level Data
    /// </summary>
    public partial class LevelBackgroundPosition
    {
        /// <value>
        /// A rect describing the cropped sub-rectangle of the displayed background image. This cropping happens when original is larger than the level bounds.
        /// </value>
        [JsonIgnore] public Rect UnityCropRect => CropRect.ToRect();
        
        /// <value>
        /// Scale of the cropped background image, depending on `bgPos` option.
        /// </value>
        [JsonIgnore] public Vector2 UnityScale => Scale.ToVector2();
        
        /// <value>
        /// Pixel coordinates of the top-left corner of the cropped background image, depending on `bgPos` option.
        /// </value>
        [JsonIgnore] public Vector2Int UnityTopLeftPx => TopLeftPx.ToVector2Int();
    }
}
