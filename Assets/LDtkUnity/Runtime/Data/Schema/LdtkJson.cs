namespace LDtkUnity
{
    using Newtonsoft.Json;
    
    public partial class LdtkJson
    {
        /// <summary>
        /// Number of backup files to keep, if the `backupOnSave` is TRUE
        /// </summary>
        [JsonProperty("backupLimit")]
        public long BackupLimit { get; set; }

        /// <summary>
        /// If TRUE, an extra copy of the project will be created in a sub folder, when saving.
        /// </summary>
        [JsonProperty("backupOnSave")]
        public bool BackupOnSave { get; set; }

        /// <summary>
        /// Project background color
        /// </summary>
        [JsonProperty("bgColor")]
        public string BgColor { get; set; }

        /// <summary>
        /// Default grid size for new layers
        /// </summary>
        [JsonProperty("defaultGridSize")]
        public long DefaultGridSize { get; set; }

        /// <summary>
        /// Default background color of levels
        /// </summary>
        [JsonProperty("defaultLevelBgColor")]
        public string DefaultLevelBgColor { get; set; }

        /// <summary>
        /// Default new level height
        /// </summary>
        [JsonProperty("defaultLevelHeight")]
        public long DefaultLevelHeight { get; set; }

        /// <summary>
        /// Default new level width
        /// </summary>
        [JsonProperty("defaultLevelWidth")]
        public long DefaultLevelWidth { get; set; }

        /// <summary>
        /// Default X pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty("defaultPivotX")]
        public double DefaultPivotX { get; set; }

        /// <summary>
        /// Default Y pivot (0 to 1) for new entities
        /// </summary>
        [JsonProperty("defaultPivotY")]
        public double DefaultPivotY { get; set; }

        /// <summary>
        /// A structure containing all the definitions of this project
        /// </summary>
        [JsonProperty("defs")]
        public Definitions Defs { get; set; }

        /// <summary>
        /// If TRUE, all layers in all levels will also be exported as PNG along with the project
        /// file (default is FALSE)
        /// </summary>
        [JsonProperty("exportPng")]
        public bool ExportPng { get; set; }

        /// <summary>
        /// If TRUE, a Tiled compatible file will also be generated along with the LDtk JSON file
        /// (default is FALSE)
        /// </summary>
        [JsonProperty("exportTiled")]
        public bool ExportTiled { get; set; }

        /// <summary>
        /// If TRUE, one file will be saved for the project (incl. all its definitions) and one file
        /// in a sub-folder for each level.
        /// </summary>
        [JsonProperty("externalLevels")]
        public bool ExternalLevels { get; set; }

        /// <summary>
        /// An array containing various advanced flags (ie. options or other states). Possible
        /// values: `DiscardPreCsvIntGrid`, `IgnoreBackupSuggest`
        /// </summary>
        [JsonProperty("flags")]
        public Flag[] Flags { get; set; }

        /// <summary>
        /// File format version
        /// </summary>
        [JsonProperty("jsonVersion")]
        public string JsonVersion { get; set; }

        /// <summary>
        /// The default naming convention for level identifiers.
        /// </summary>
        [JsonProperty("levelNamePattern")]
        public string LevelNamePattern { get; set; }

        /// <summary>
        /// All levels. The order of this array is only relevant in `LinearHorizontal` and
        /// `linearVertical` world layouts (see `worldLayout` value). Otherwise, you should refer to
        /// the `worldX`,`worldY` coordinates of each Level.
        /// </summary>
        [JsonProperty("levels")]
        public Level[] Levels { get; set; }

        /// <summary>
        /// If TRUE, the Json is partially minified (no indentation, nor line breaks, default is
        /// FALSE)
        /// </summary>
        [JsonProperty("minifyJson")]
        public bool MinifyJson { get; set; }

        /// <summary>
        /// Next Unique integer ID available
        /// </summary>
        [JsonProperty("nextUid")]
        public long NextUid { get; set; }

        /// <summary>
        /// File naming pattern for exported PNGs
        /// </summary>
        [JsonProperty("pngFilePattern")]
        public string PngFilePattern { get; set; }

        /// <summary>
        /// Height of the world grid in pixels.
        /// </summary>
        [JsonProperty("worldGridHeight")]
        public long WorldGridHeight { get; set; }

        /// <summary>
        /// Width of the world grid in pixels.
        /// </summary>
        [JsonProperty("worldGridWidth")]
        public long WorldGridWidth { get; set; }

        /// <summary>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D
        /// space). Possible values: `Free`, `GridVania`, `LinearHorizontal`, `LinearVertical`
        /// </summary>
        [JsonProperty("worldLayout")]
        public WorldLayout WorldLayout { get; set; }
    }

    public partial class LdtkJson
    {
        /// <summary>
        /// Get a deserialized <see cref="LdtkJson"/> data class.
        /// </summary>
        /// <param name="json">
        /// The LDtk Json root in json string format.
        /// </param>
        /// <returns>
        /// A deserialized <see cref="LdtkJson"/> data class.
        /// </returns>
        public static LdtkJson FromJson(string json) => JsonConvert.DeserializeObject<LdtkJson>(json, LDtkUnity.Converter.Settings);
    }
}