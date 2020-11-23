// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Providers;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#24-enum-definition
    public struct LDtkDefinitionEnum : ILDtkUid
    {
        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        [JsonProperty] public string externalRelPath { get; private set; }
        
        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        [JsonProperty] public int iconTilesetUid { get; private set; } //todo consider making nullable?
        
        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string identifier { get; private set; }
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [JsonProperty] public int uid { get; private set; }
        
        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        [JsonProperty] public LDtkDefinitionEnumValue[] values { get; private set; }

        public LDtkDefinitionTileset IconTileset => LDtkProviderUid.GetUidData<LDtkDefinitionTileset>(iconTilesetUid);
    }
}