using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal class LDtkParsedTile : ILDtkValueParser
    {
        private static LDtkProjectImporter _importer;
        
        public static void CacheRecentImporter(LDtkProjectImporter lDtkProjectImporter)
        {
            _importer = lDtkProjectImporter;
        }
        
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsTile;
        }

        public object ImportString(object input)
        {
            //input begins as a string in json format
            //example of a tile instance:
            //{ "tilesetUid": 104, "srcRect": [144,128,16,16] },

            TilesetRectangle tile = null;
            string inputString = input.ToString();
            
            try
            {
                tile = TilesetRectangle.FromJson(inputString);
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

            if (_importer == null)
            {
                Debug.LogError("LDtk: Couldn't parse point, importer was null");
                return null;
            }
            
            RectInt rect = tile.UnityRect;
            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            Texture2D srcTex = getter.GetRelativeAsset(tileset, _importer.assetPath);
            Sprite sprite = _importer.GetSprite(srcTex, rect, _importer.PixelsPerUnit);

            return sprite;
        }
    }
}