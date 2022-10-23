using System.Text.Json.Serialization;

namespace LDtkUnity
{
    /// <summary>
    /// This section contains all the level data. It can be found in 2 distinct forms, depending
    /// on Project current settings:  - If "*Separate level files*" is **disabled** (default):
    /// full level data is *embedded* inside the main Project JSON file, - If "*Separate level
    /// files*" is **enabled**: level data is stored in *separate* standalone `.ldtkl` files (one
    /// per level). In this case, the main Project JSON file will still contain most level data,
    /// except heavy sections, like the `layerInstances` array (which will be null). The
    /// `externalRelPath` string points to the `ldtkl` file.  A `ldtkl` file is just a JSON file
    /// containing exactly what is described below.
    /// </summary>
    public partial class Level
    {
        /// <summary>
        /// Background color of the level (same as `bgColor`, except the default value is
        /// automatically used here if its value is `null`)
        /// </summary>
        [JsonPropertyName("__bgColor")]
        public string BgColor { get; set; }

        /// <summary>
        /// Position informations of the background image, if there is one.
        /// </summary>
        [JsonPropertyName("__bgPos")]
        public LevelBackgroundPosition BgPos { get; set; }

        /// <summary>
        /// An array listing all other levels touching this one on the world map.<br/>  Only relevant
        /// for world layouts where level spatial positioning is manual (ie. GridVania, Free). For
        /// Horizontal and Vertical layouts, this array is always empty.
        /// </summary>
        [JsonPropertyName("__neighbours")]
        public NeighbourLevel[] Neighbours { get; set; }

        /// <summary>
        /// The "guessed" color for this level in the editor, decided using either the background
        /// color or an existing custom field.
        /// </summary>
        [JsonPropertyName("__smartColor")]
        public string SmartColor { get; set; }

        /// <summary>
        /// Background color of the level. If `null`, the project `defaultLevelBgColor` should be
        /// used.
        /// </summary>
        [JsonPropertyName("bgColor")]
        public string LevelBgColor { get; set; }

        /// <summary>
        /// Background image X pivot (0-1)
        /// </summary>
        [JsonPropertyName("bgPivotX")]
        public double BgPivotX { get; set; }

        /// <summary>
        /// Background image Y pivot (0-1)
        /// </summary>
        [JsonPropertyName("bgPivotY")]
        public double BgPivotY { get; set; }

        /// <summary>
        /// An enum defining the way the background image (if any) is positioned on the level. See
        /// `__bgPos` for resulting position info. Possible values: &lt;`null`&gt;, `Unscaled`,
        /// `Contain`, `Cover`, `CoverDirty`
        /// </summary>
        [JsonPropertyName("bgPos")]
        public BgPos? LevelBgPos { get; set; }

        /// <summary>
        /// The *optional* relative path to the level background image.
        /// </summary>
        [JsonPropertyName("bgRelPath")]
        public string BgRelPath { get; set; }

        /// <summary>
        /// This value is not null if the project option "*Save levels separately*" is enabled. In
        /// this case, this **relative** path points to the level Json file.
        /// </summary>
        [JsonPropertyName("externalRelPath")]
        public string ExternalRelPath { get; set; }

        /// <summary>
        /// An array containing this level custom field values.
        /// </summary>
        [JsonPropertyName("fieldInstances")]
        public FieldInstance[] FieldInstances { get; set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Unique instance identifier
        /// </summary>
        [JsonPropertyName("iid")]
        public string Iid { get; set; }

        /// <summary>
        /// An array containing all Layer instances. **IMPORTANT**: if the project option "*Save
        /// levels separately*" is enabled, this field will be `null`.<br/>  This array is **sorted
        /// in display order**: the 1st layer is the top-most and the last is behind.
        /// </summary>
        [JsonPropertyName("layerInstances")]
        public LayerInstance[] LayerInstances { get; set; }

        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        [JsonPropertyName("pxHei")]
        public long PxHei { get; set; }

        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        [JsonPropertyName("pxWid")]
        public long PxWid { get; set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// If TRUE, the level identifier will always automatically use the naming pattern as defined
        /// in `Project.levelNamePattern`. Becomes FALSE if the identifier is manually modified by
        /// user.
        /// </summary>
        [JsonPropertyName("useAutoIdentifier")]
        public bool UseAutoIdentifier { get; set; }

        /// <summary>
        /// Index that represents the "depth" of the level in the world. Default is 0, greater means
        /// "above", lower means "below".<br/>  This value is mostly used for display only and is
        /// intended to make stacking of levels easier to manage.
        /// </summary>
        [JsonPropertyName("worldDepth")]
        public long WorldDepth { get; set; }

        /// <summary>
        /// World X coordinate in pixels.<br/>  Only relevant for world layouts where level spatial
        /// positioning is manual (ie. GridVania, Free). For Horizontal and Vertical layouts, the
        /// value is always -1 here.
        /// </summary>
        [JsonPropertyName("worldX")]
        public long WorldX { get; set; }

        /// <summary>
        /// World Y coordinate in pixels.<br/>  Only relevant for world layouts where level spatial
        /// positioning is manual (ie. GridVania, Free). For Horizontal and Vertical layouts, the
        /// value is always -1 here.
        /// </summary>
        [JsonPropertyName("worldY")]
        public long WorldY { get; set; }
    }
}