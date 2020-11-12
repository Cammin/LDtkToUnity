// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#24-enum-definition
    public struct LDtkDefinitionEnumValue
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: [ x, y, width, height ]
        /// </summary>
        [JsonProperty] public int[] __tileSrcRect { get; private set; }

        /// <summary>
        /// Enum value
        /// </summary>
        [JsonProperty] public string id { get; private set; }
        
        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        [JsonProperty] public int? tileId { get; private set; }

        public Rect SourceRect => __tileSrcRect.ToRect();
    }
}