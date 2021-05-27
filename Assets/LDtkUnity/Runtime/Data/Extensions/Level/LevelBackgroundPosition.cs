using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Level Data
    /// </summary>
    public partial class LevelBackgroundPosition
    {
        /// <summary>
        /// A rect describing the cropped sub-rectangle of the displayed background image. This cropping happens when original is larger than the level bounds.
        /// </summary>
        [JsonIgnore] public Rect UnityCropRect => CropRect.ToRect();
        
        /// <summary>
        /// Scale of the cropped background image, depending on `bgPos` option.
        /// </summary>
        [JsonIgnore] public Vector2 UnityScale => Scale.ToVector2();
        
        /// <summary>
        /// Pixel coordinates of the top-left corner of the cropped background image, depending on `bgPos` option.
        /// </summary>
        [JsonIgnore] public Vector2Int UnityTopLeftPx => TopLeftPx.ToVector2Int();
    }
}
