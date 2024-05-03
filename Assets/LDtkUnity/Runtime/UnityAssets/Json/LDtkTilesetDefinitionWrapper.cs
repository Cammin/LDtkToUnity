using System;
using System.Collections.Generic;
using UnityEngine;
using Utf8Json;

namespace LDtkUnity
{
    /// <summary>
    /// A wrapper on the TilesetDefinition that contains some additional data in order to independently import certain tiles related to this tileset definition.
    /// We're making this because it's harder to generate an asset and additionally set it's importer's bonus metadata in the same pass.
    /// So we're writing our own text instead to provide that data.
    /// </summary>
    public class LDtkTilesetDefinitionWrapper
    {
        /// <summary>
        /// AdditionalRects; Contains all malformed tile rects (tiles that aren't equal in width nor height to the tilesets gridSize).
        /// These are not included with the sprite editor window integration, as not only do they overlap when trying to click on a sprite to edit, but also aren't gonna have tilemap assets generated for them anyways, as they wouldn't fit.
        /// These could be extra rects that are shaped like rectangles to slice, from tile field definitions, or icons maybe.
        /// </summary>
        public List<TilesetRect> Rects;
        
        /// <summary>
        /// The tileset definition. 
        /// </summary>
        public TilesetDefinition Def;

        public static LDtkTilesetDefinitionWrapper FromJson(string json)
        {
            return JsonSerializer.Deserialize<LDtkTilesetDefinitionWrapper>(json);
        }
        public byte[] ToJson()
        {
            byte[] serialize = JsonSerializer.Serialize(this);
            return JsonSerializer.PrettyPrintByteArray(serialize);
        }

        //needs to be public or else it's not serializable(?)
        [Serializable]
        public struct TilesetRect
        {
            public int x;
            public int y;
            public int w;
            public int h;

            public static TilesetRect FromRectInt(RectInt rectInt)
            {
                return new TilesetRect()
                {
                    x = rectInt.x,
                    y = rectInt.y,
                    w = rectInt.width,
                    h = rectInt.height
                };
            }
            public Rect ToRect()
            {
                return new Rect()
                {
                    x = x,
                    y = y,
                    width = w,
                    height = h
                };
            }
        }
    }
}