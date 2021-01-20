using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Position informations of the background image, if there is one.
    ///
    /// A small object describing the level background image position, based on level settings.
    /// </summary>
    public partial class LdtkLevelBgPosInfos
    {
        /// <summary>
        /// An array containing the `[scaleX,scaleY]` values of the background image, depending on
        /// `bgPos` option.
        /// </summary>
        [JsonProperty("scale")]
        public double[] Scale { get; set; }

        /// <summary>
        /// An array of 4 float values describing the sub-rectangle of the displayed background
        /// image. This is useful when the initial image was cropped, because it was larger than the
        /// level bounds. Array format: `[ subX, subY, subWidth, subHeight ]`
        /// </summary>
        [JsonProperty("subRect")]
        public double[] SubRect { get; set; }

        /// <summary>
        /// An array containing the `[x,y]` pixel coordinates of the top-left corner of the
        /// background image, depending on `bgPos` option.
        /// </summary>
        [JsonProperty("topLeftPx")]
        public long[] TopLeftPx { get; set; }
    }
}