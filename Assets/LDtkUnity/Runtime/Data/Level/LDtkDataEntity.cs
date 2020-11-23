// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Providers;
using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#112-entity-instance
    public struct LDtkDataEntity
    {
        /// <summary>
        /// Grid-based coordinates ([x,y] format)
        /// </summary>
        [JsonProperty] public int[] __grid { get; private set; }

        /// <summary>
        /// Unique String identifier
        /// </summary>
        [JsonProperty] public string __identifier { get; private set; }

        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum).
        /// </summary>
        [JsonProperty] public LDtkDataEntityTile __tile { get; private set; }
        
        /// <summary>
        /// Reference of the Entity definition UID
        /// </summary>
        [JsonProperty] public int defUid { get; private set; }

        /// <summary>
        /// Array of Field instance
        /// </summary>
        [JsonProperty] public LDtkDataField[] fieldInstances { get; private set; }

        /// <summary>
        /// Pixel coordinates ([x,y] format). Don't forget optional layer offsets, if they exist!
        /// </summary>
        [JsonProperty] public int[] px { get; private set; }


        public LDtkDefinitionEntity Definition => LDtkProviderUid.GetUidData<LDtkDefinitionEntity>(defUid);
    }
}