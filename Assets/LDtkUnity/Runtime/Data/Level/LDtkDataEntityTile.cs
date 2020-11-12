// ReSharper disable InconsistentNaming

using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#112-entity-instance
    
    /// <summary>
    /// Optional Tile used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum).
    /// </summary>
    public struct LDtkDataEntityTile
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: [ x, y, width, height ]
        /// </summary>
        [JsonProperty] public int[] srcRect { get; private set; }
        
        /// <summary>
        /// Tileset ID
        /// </summary>
        [JsonProperty] public int tilesetUid { get; private set; }

        public LDtkDefinitionTileset Definition => LDtkUidDatabase.GetUidData<LDtkDefinitionTileset>(tilesetUid);
        public Rect SourceRect => srcRect.ToRect();
    }
}