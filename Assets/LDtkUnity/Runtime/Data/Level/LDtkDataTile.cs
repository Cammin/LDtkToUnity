// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Runtime.Data.Level
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#111-tile-instance--
    public struct LDtkDataTile
    {
        /// <summary>
        /// Internal data used by the editor.
        /// For auto-layer tiles: [ruleId, coordId].
        /// For tile-layer tiles: [coordId].
        /// </summary>
        [JsonProperty] public int[] d { get; private set; }
        
        /// <summary>
        /// "Flip flags", a 2-bits integer to represent the mirror transformations of the tile.
        /// -Bit 0 = X flip
        /// -Bit 1 = Y flip
        /// </summary>
        [JsonProperty] public int f { get; private set; }
        
        /// <summary>
        /// Pixel coordinates of the tile in the layer ([x,y] format). Don't forget optional layer offsets, if they exist!
        /// </summary>
        [JsonProperty] public int[] px { get; private set; }
        
        /// <summary>
        /// Pixel coordinates of the tile in the tileset ([x,y] format)
        /// </summary>
        [JsonProperty] public int[] src { get; private set; }
        
        /// <summary>
        /// The Tile ID in the corresponding tileset.
        /// </summary>
        [JsonProperty] public int t { get; private set; } //TODO implement getting the definition of this


        public bool FlipX => new BitArray(BitConverter.GetBytes(f)).Get(0);
        public bool FlipY => new BitArray(BitConverter.GetBytes(f)).Get(1);
        
        public Vector2Int LayerPixelPosition => px.ToVector2Int();
        public Vector2Int SourcePixelPosition => src.ToVector2Int();
        
        public int AutoLayerRuleID => d[0];
        public int AutoLayerCoordID => d[1];
        
        public int TileLayerCoordId => d[0];

    }
}