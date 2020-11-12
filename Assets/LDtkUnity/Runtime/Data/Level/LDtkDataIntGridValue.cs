// ReSharper disable InconsistentNaming

using Newtonsoft.Json;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#11-layer-instance
    public struct LDtkDataIntGridValue
    {
        /// <summary>
        /// Coordinate ID in the layer grid
        /// </summary>
        [JsonProperty] public int coordId { get; private set; }
        
        /// <summary>
        /// IntGrid value
        /// </summary>
        [JsonProperty] public int v { get; private set; }
    }
}