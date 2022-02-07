using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    public class LDtkParsedTile : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsTile;
        }

        public object ImportString(object input)
        {
            //input begins as a string in json format
            //example of a tile instance:
            //{ "tilesetUid": 104, "srcRect": [144,128,16,16] },

            FieldInstanceTile tile = null;
            string inputString = input.ToString();
            
            try
            {
                tile = FieldInstanceTile.FromJson(inputString);
            }
            catch (Exception e)
            {
                Debug.LogError($"LDtk: Json error for tile:\n{e}");
                return null;
            }

            if (tile == null)
            {
                Debug.LogError($"LDtk: Tile was null");
                return null;
            }

            TilesetDefinition tileset = tile.Tileset;
            if (tileset == null)
            {
                Debug.LogError("LDtk: getting tileset was null");
                return null;
            }

            Sprite tileSprite = null;

            //LDtkTextureSpriteSlicer
            //read/write from the artifact assets to get the tile we're looking for. We only want to use the minimum tiles that are actually used to pack into the atlas.
            //LDtkCoordConverter.ImageSliceCoord()
            //todo do something to get from here with the casted tile data. 
            
            return tileSprite;
        }
    }
}